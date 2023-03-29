using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProjectileSpawner : MonoBehaviour
{
    public static GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(projectilePrefab, position, rotation);
    }
}
