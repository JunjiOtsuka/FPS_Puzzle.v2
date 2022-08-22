using UnityEngine;

public class EventDoorDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" ||
            other.tag == "Cube")
        {
            EventArea.current.DoorwayTriggerEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" ||
            other.tag == "Cube")
        {
            EventArea.current.DoorwayTriggerExit();
        }
    }
}
