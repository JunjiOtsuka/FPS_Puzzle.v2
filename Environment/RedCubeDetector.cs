using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCubeDetector : MonoBehaviour
{
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "RedCube")
		{
			GetComponent<Animator>().SetBool("OpenDoor", true);
		} 
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "RedCube")
		{
			GetComponent<Animator>().SetBool("OpenDoor", false);
		}
	}
}
