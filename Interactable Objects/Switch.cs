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
    public List<FloorRotate> targets;
    public int TargetListSize;
    public float DoorTimer = 3f;
    public static bool FloorSwitchOn = false;

    public void Interact()
    {
        if (targets == null) return;
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
            if (!FloorSwitchOn && PlayerMovementV2.interactClick.WasPerformedThisFrame())
            {
                Debug.Log("on");
                foreach (FloorRotate target in targets)
                {
                    target.FloorSwitchOn = true;
                }
            } 
            if (FloorSwitchOn && targets.Count == FloorRotate.getAllTargets)
            {
                Debug.Log("off");
                foreach (FloorRotate target in targets)
                {
                    target.FloorSwitchOn = false;
                }
            }
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
