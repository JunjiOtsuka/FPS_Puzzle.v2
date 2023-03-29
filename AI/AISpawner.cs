using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject m_Destination;

    public void SpawnMonster(GameObject prefab)
    {
        GameObject monster = Instantiate(prefab, transform.position, Quaternion.identity);
        monster.GetComponent<AIBehavior>().target = m_Destination.transform;
    }
}
