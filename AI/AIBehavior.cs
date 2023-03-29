using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehavior : MonoBehaviour
{
    //AI behavior
    public Transform target;
    Vector3 destination;
    public NavMeshAgent agent;

    MonsterData monster;

    bool bDestination = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!target) return;

        name = GetComponent<Stats>().name;

        if (MonsterData.data.Count == 0) return;
        
        if (MonsterData.data.ContainsKey(name))
        {
            agent.speed = MonsterData.data[name].speed;
            agent.angularSpeed = MonsterData.data[name].angSpeed;
            agent.acceleration = MonsterData.data[name].acceleration;
        }

        //Set destination
        if (!bDestination)
        {
            agent.SetDestination(target.position);
            bDestination = true;
        }

        //pending navmesh path
        if(agent.pathPending) return;
        
        //navmesh path completed 

        // Update destination if the target moves one unit
        if (Vector3.Distance(destination, target.position) > 1.0f)
        {
            destination = target.position;
            agent.destination = destination;
        }

        //agent reached destination
        if (agent.remainingDistance <= 1f)
        {
            //deactivate gameobject
            this.gameObject.SetActive(false);
        }
    }

    
}
