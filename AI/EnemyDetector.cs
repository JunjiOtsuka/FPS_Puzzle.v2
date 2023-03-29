using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDetector : MonoBehaviour
{
    public CooldownManager m_CooldownManager = new CooldownManager();
    bool EnemyInRange = false;

    public GameObject projectilePrefab;
    public GameObject projectileSpawner;
    Transform target;

    void Update()
    {
        if (EnemyInRange)
        {
            // Debug.Log("Enemy Detected");
            if (!target.transform.gameObject.activeInHierarchy) return;
            
            projectileSpawner.transform.LookAt(target);
            ShootProjectile();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyAI")
        {
            target = other.transform;
            EnemyInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "EnemyAI")
        {
            EnemyInRange = false;
        }
    }

    void ShootProjectile()
    {
        m_CooldownManager.SetCDTimer(1.0f);
        m_CooldownManager.StartCDTimer(m_CooldownManager);

        if (m_CooldownManager.bCDEnd())
        {
            // Debug.Log("Shooting Projectile");
            var instance = AIProjectileSpawner.SpawnProjectile(projectilePrefab, projectileSpawner.transform.position, projectileSpawner.transform.rotation);

            if (ProjectileDirection(
                target.transform.position, 
                projectileSpawner.transform.position, 
                target.gameObject.GetComponent<NavMeshAgent>().velocity, 
                projectilePrefab.GetComponent<AIProjectile>().projectileSpeed, 
                out var direction ))
            {
                instance.GetComponent<Rigidbody>().velocity = direction * projectilePrefab.GetComponent<AIProjectile>().projectileSpeed;
            } else {
                Debug.Log("Invalid speed");
            }
            
            m_CooldownManager.ResetCDTimer();
        }
    }

    public bool ProjectileDirection(Vector3 a, Vector3 b, Vector3 vA, float sB, out Vector3 result)
    {
        // Vector from a to b
        var aToB = b - a;
        var dC = aToB.magnitude;
        var alpha = Vector3.Angle(aToB, vA) * Mathf.Deg2Rad;
        var sA = vA.magnitude;
        var r = sA / sB;

        if (GetQuadratic(1-r*r, 2*r*dC*Mathf.Cos(alpha), -(dC*dC), out var root1, out var root2) == 0)
        {
            result = Vector3.zero;
            return false;
        } 
        var dA = Mathf.Max(root1, root2);
        var t = dA / sB;
        var c = a + vA * t;
        result = (c - b).normalized;
        return true;
    }

    public int GetQuadratic(float a, float b, float c, out float root1, out float root2)
    {
        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            root1 = Mathf.Infinity;
            root2 = -root1;
            return 0;
        }

        root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

        return discriminant > 0 ? 2 : 1;
    }
}
