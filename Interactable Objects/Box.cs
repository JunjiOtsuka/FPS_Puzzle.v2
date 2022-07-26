using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    public bool IsInteracting = false;
    public bool JointConnected = false;
    GameObject interactZone;

    public void Interact()
    {
        interactZone = GameObject.Find("Interactable Zone");
        if (GetComponent<FixedJoint>() == null)
        {
            interactZone.transform.DetachChildren();
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            IsInteracting = false;
            JointConnected = false;
        }
        if (!IsInteracting && PlayerMovementV2.interactClick.WasPerformedThisFrame())
        {
            GrabBox();
        } 
        else if (IsInteracting && PlayerMovementV2.rightClick.WasPerformedThisFrame())
        {
            ReleaseBox();
        }
        else if (IsInteracting && PlayerMovementV2.leftClick.WasPerformedThisFrame())
        {
            ThrowBox();
        }
    }



    public void GrabBox()
    {
        Debug.Log("Grabbing");
        if (GetComponent<FixedJoint>() == null)
        {
            // GameObject player = GameObject.Find("PlayerNewInput");
            // GameObject interactZone = GameObject.Find("Interactable Zone");

            gameObject.AddComponent<FixedJoint>();
            FixedJoint jointComponent = GetComponent<FixedJoint>();
            jointComponent.connectedBody = interactZone.GetComponent<Rigidbody>();
            jointComponent.breakForce = 1000;
            jointComponent.breakTorque = 1000;
            jointComponent.enableCollision = true;

            //make the object a child
            transform.parent = interactZone.transform;
            //take away the box collider so it wont interact with other objects.
            
            //in order to make it look like its holding an object make
            //make the object position to this game objects position
            transform.position = interactZone.transform.position;

            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            IsInteracting = true;
            JointConnected = true;
        }
    }

    public void ReleaseBox() 
    {
        Debug.Log("Release");
        Destroy (GetComponent<FixedJoint>());

        interactZone.transform.DetachChildren();

        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        IsInteracting = false;
        JointConnected = false;
    }

    public void ThrowBox()
    {
        Debug.Log("Throwing");
        Destroy (GetComponent<FixedJoint>());

        GetComponent<Rigidbody>().AddForce(interactZone.transform.forward * 100, ForceMode.VelocityChange);
        interactZone.transform.DetachChildren();

        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        IsInteracting = false;
        JointConnected = false;
    }

    IEnumerator BoxPickUpCooldown() {
        yield return new WaitForSeconds(0.2f);
        IsInteracting = true;
    }

    IEnumerator BoxReleaseCooldown() {
        yield return new WaitForSeconds(0.2f);
        IsInteracting = false;
    }
}
