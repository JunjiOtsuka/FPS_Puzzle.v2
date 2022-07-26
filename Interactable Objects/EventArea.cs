using System;
using UnityEngine;

public class EventArea : MonoBehaviour
{
    public static EventArea current;
    public float doorCooldown;

    private void Awake() {
        current = this;
    }

    public event Action onDoorwayTriggerEnter;
    public void DoorwayTriggerEnter()
    {
        if (onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter();
        }
    }

    public event Action onDoorwayTriggerExit;
    public void DoorwayTriggerExit()
    {
        if (onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit();
        }
    }
}
