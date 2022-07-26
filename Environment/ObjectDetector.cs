using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
	// void OnTriggerEnter(Collider other)
	// {
	// 	if (other.name == "Player" || other.tag == "Interactable")
	// 	{
	// 		GetComponent<Animator>().SetBool("OpenDoor", true);
	// 	} 
	// }

	void OnTriggerStay(Collider other)
	{
		if (other.name == "Player" || other.tag == "Cube")
		{
			GetComponent<Animator>().SetBool("OpenDoor", true);
		} 
	}

	void OnTriggerExit(Collider other)
	{
		if (other.name == "Player" || other.tag == "Cube")
		{
			GetComponent<Animator>().SetBool("OpenDoor", false);
		}
	}
}
