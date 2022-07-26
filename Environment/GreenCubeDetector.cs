using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCubeDetector : MonoBehaviour
{
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "GreenCube")
		{
			GetComponent<Animator>().SetBool("OpenDoor", true);
		} 
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "GreenCube")
		{
			GetComponent<Animator>().SetBool("OpenDoor", false);
		}
	}
}
