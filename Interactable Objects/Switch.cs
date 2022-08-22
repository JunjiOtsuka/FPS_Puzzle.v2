using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    public enum SwitchType {
        DOOR, 
        FLOOR
    }
    public SwitchType _SwitchType;
    public bool IsInteracting = false;
    public static bool SwitchOn = false;
    public GameObject SwitchWiredTo;
    public Transform origin;
    public Transform target;
    public float DoorTimer = 3f;
    public static bool FloorSwitchOn = false;
    public FloorRotate _FloorRotate;

    public void Interact()
    {
        // if (!SwitchOn && PlayerMovementV2.interactClick.WasPerformedThisFrame())
        // {
        //     if (_SwitchType == SwitchType.DOOR) OnSwitchOn();
        //     if (_SwitchType == SwitchType.FLOOR) OnFloorSwitchOn();
        // } 
        // if (SwitchOn) {
        //     if (_SwitchType == SwitchType.DOOR) StartCoroutine(OnSwitchOff());
        //     if(PlayerMovementV2.interactClick.WasPerformedThisFrame())
        //     {
        //         if (_SwitchType == SwitchType.FLOOR) OnFloorSwitchOff();
        //     }
        // }

        if (_SwitchType == SwitchType.DOOR)
        {
            if (!SwitchOn && PlayerMovementV2.interactClick.WasPerformedThisFrame())
            {
                OnSwitchOn();
            } 
            if (SwitchOn) {
                StartCoroutine(OnSwitchOff());
            }
        }
        if (_SwitchType == SwitchType.FLOOR) 
        {
            if (!SwitchOn && PlayerMovementV2.interactClick.WasPerformedThisFrame()) 
            {
                FloorSwitchOn = true;
            } 
            // else if (SwitchOn && PlayerMovementV2.interactClick.WasPerformedThisFrame())
            // {
            //     FloorRotate.OnFloorSwitchOff();
            // } 
        }
    }

    public void OnSwitchOn()
    {
        SwitchWiredTo.GetComponent<Animator>().SetBool("OpenDoor", true);
        SwitchOn = true;
    }

    IEnumerator OnSwitchOff()
    {
        yield return new WaitForSeconds(DoorTimer);
        SwitchWiredTo.GetComponent<Animator>().SetBool("OpenDoor", false);
        SwitchOn = false;
    }

}
