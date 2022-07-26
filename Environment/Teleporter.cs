using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 teleportDestination;

	void OnTriggerEnter(Collider other)
    {
    	if (other.name == "Player")
    	{
	    	// other.transform.position = teleportDestination;
	    	other.gameObject.GetComponent<CharacterController>().enabled = false;
			other.gameObject.GetComponent<CharacterController>().transform.position = teleportDestination;
			other.gameObject.GetComponent<CharacterController>().enabled = true;
    	}
    }
}
