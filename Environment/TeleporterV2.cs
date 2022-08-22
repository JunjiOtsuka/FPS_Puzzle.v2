using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterV2 : MonoBehaviour
{
    public Transform destination;

    void OnTriggerEnter(Collider other)
    {
    	if (other.tag == "Player")
    	{
	    	// other.transform.position = teleportDestination;
	    	// other.gameObject.GetComponent<CharacterController>().enabled = false;
			other.gameObject.transform.position = destination.position;
			other.gameObject.transform.rotation = destination.rotation;
			// other.gameObject.GetComponent<CharacterController>().enabled = true;
    	}
    }
}
