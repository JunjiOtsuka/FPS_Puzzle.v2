using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;

	void OnTriggerEnter(Collider other)
    {
    	if (other.tag == "Player")
    	{
			other.transform.position = teleportDestination.position;
			other.transform.rotation = teleportDestination.rotation;
    	}
    }
}
