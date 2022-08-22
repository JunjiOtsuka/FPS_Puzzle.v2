using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderGizmo : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        IInteractable action = other.GetComponent<IInteractable>();
        

    	if(other.tag == "Cube" || other.tag == "RedCube" || other.tag == "GreenCube")
    	{
            if (action == null) return;
            
                // action.Interact();

    	}
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Cube" || other.tag == "RedCube" || other.tag == "GreenCube")
        {
            // Destroy (other.gameObject.GetComponent<FixedJoint>());
            
            this.transform.DetachChildren();
            
        }
    }
}
