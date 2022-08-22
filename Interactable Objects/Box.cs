using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    public static bool IsInteracting = false;
    public bool JointConnected = false;
    GameObject interactZone;
    GameObject dropZone;

    public void Update()
    {
        if (IsInteracting && WeaponStateManager.WeaponState != WeaponState.BAREHAND) {
            ReleaseBox();
            return;
        }
        if (IsInteracting && PlayerMovementV2.rightClick.WasPerformedThisFrame())
        {
            ReleaseBox();
        }
        else if (IsInteracting && PlayerMovementV2.leftClick.WasPerformedThisFrame())
        {
            ThrowBox();
        }
    }

    public void Interact()
    {
        if (WeaponStateManager.WeaponState != WeaponState.BAREHAND) return;

        interactZone = GameObject.Find("Interactable Zone");
        if (GetComponent<FixedJoint>() == null)
        {
            DoStopInteracting();
        }
        if (!IsInteracting && PlayerMovementV2.interactClick.WasPerformedThisFrame())
        {
            GrabBox();
        } 
    }



    public void GrabBox()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().drag = 0;
        gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
        //make the object a child
        transform.parent = interactZone.transform;
        //take away the box collider so it wont interact with other objects.
        
        //in order to make it look like its holding an object make
        //make the object position to this game objects position
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        IsInteracting = true;
    }

    public void ReleaseBox() 
    {
        dropZone = GameObject.Find("DropZone");
        DoStopInteracting();
        transform.localPosition = dropZone.transform.position;
    }

    public void ThrowBox()
    {
        DoStopInteracting();
        GetComponent<Rigidbody>().AddForce(interactZone.transform.forward * 100, ForceMode.VelocityChange);
    }

    public void DoStopInteracting()
    {
        interactZone.transform.DetachChildren();

        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        gameObject.GetComponent<Rigidbody>().drag = 1;

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
