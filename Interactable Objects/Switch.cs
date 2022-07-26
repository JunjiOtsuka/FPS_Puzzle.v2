using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    public bool IsInteracting = false;
    public bool SwitchOn = false;
    public GameObject SwitchWiredTo;
    public float DoorTimer = 3f;

    public void Interact()
    {
        if (!SwitchOn && PlayerMovementV2.interactClick.WasPerformedThisFrame())
        {
            OnSwitchOn();
        } 
        if (SwitchOn) {
            StartCoroutine(OnSwitchOff());
        }
    }

    public void OnSwitchOn()
    {
        Debug.Log("SwitchOn");
        SwitchWiredTo.GetComponent<Animator>().SetBool("OpenDoor", true);
        SwitchOn = true;
    }

    IEnumerator OnSwitchOff()
    {
        yield return new WaitForSeconds(DoorTimer);
        Debug.Log("SwitchOff");
        SwitchWiredTo.GetComponent<Animator>().SetBool("OpenDoor", false);
        SwitchOn = false;
    }

}
