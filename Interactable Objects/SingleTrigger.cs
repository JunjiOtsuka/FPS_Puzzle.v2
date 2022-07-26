using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTrigger : MonoBehaviour
{
    public bool IsInteracting = false;
    public bool SwitchOn = false;
    public GameObject SwitchWiredTo;
    public float DoorTimer = 3f;

    public void OnSwitchOn()
    {
        Debug.Log("SwitchOn");
        SwitchOn = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" ||
            other.tag == "Cube")
        {
            Debug.Log("player entered");
            SwitchWiredTo.GetComponent<Animator>().SetBool("OpenDoor", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" ||
            other.tag == "Cube")
        {
            Debug.Log("player stayed");
            SwitchWiredTo.GetComponent<Animator>().SetBool("OpenDoor", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" ||
            other.tag == "Cube")
        {
            Debug.Log("player exited");
            StartCoroutine(OnSwitchOff());
        }
    }

    IEnumerator OnSwitchOff()
    {
        yield return new WaitForSeconds(DoorTimer);
        SwitchWiredTo.GetComponent<Animator>().SetBool("OpenDoor", false);
    }
}
