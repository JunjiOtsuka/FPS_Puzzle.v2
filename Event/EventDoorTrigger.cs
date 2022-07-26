using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDoorTrigger : MonoBehaviour
{
    EventArea _EventArea;
    void Start()
    {
        EventArea.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        EventArea.current.onDoorwayTriggerExit += OnDoorwayClose;
        _EventArea = gameObject.transform.root.Find("Door(EnterEvent)").GetComponent<EventArea>();
    }

    private void OnDoorwayOpen()
    {
        gameObject.GetComponent<Animator>().SetBool("OpenDoor", true);
    }

    private void OnDoorwayClose()
    {
        StartCoroutine(OnCloseDoorCooldown());
    }

    IEnumerator OnCloseDoorCooldown() 
    {
        yield return new WaitForSeconds(_EventArea.doorCooldown);
        gameObject.GetComponent<Animator>().SetBool("OpenDoor", false);
    } 
}
