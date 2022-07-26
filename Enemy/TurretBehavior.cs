using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    // The target marker.
    public GameObject target;

    // Angular speed in radians per sec.
    public float speed = 1.0f;
    public GameObject turret;
    public Material original;
    public Material caution;
    MeshRenderer cubeRenderer;

    void Start()
    {
        cubeRenderer = turret.GetComponent<MeshRenderer>();
        target = MySceneManager.player;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("upon enter");
            cubeRenderer.material = caution;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("upon stay");
            TurnTowardsPlayer();
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("upon exit");
            cubeRenderer.material = original;
        }
    }

    void TurnTowardsPlayer() 
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = new Vector3(target.transform.position.x, 0f, target.transform.position.z) - new Vector3(transform.root.position.x, 0f, transform.root.position.z);

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.root.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.root.rotation = Quaternion.LookRotation(newDirection);
    }
}
