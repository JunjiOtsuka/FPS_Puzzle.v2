using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRotate : MonoBehaviour
{
    public Transform from;
    public Transform target;
    float speed = 0.01f;
    float timeCount = 0.0f;
    public static float currentRotation;
    float elapsedTime = 0;
    float waitTime = 1f;
    bool UpdateRotation = false;

    void Update()
    {
        // transform.rotation = Quaternion.Lerp(from.rotation, target.rotation, (elapsedTime / waitTime));
        // elapsedTime += Time.deltaTime;
        if (Switch.FloorSwitchOn)
        {
            OnFloorSwitchOn();
        }
    }

    // public void OnFloorSwitchOn()
    // {
    //     transform.Rotate(0f, 90f, 0f);
    //     Switch.FloorSwitchOn = false;
    // }

    public void OnFloorSwitchOn()
    {
        if (UpdateRotation == false)
        {
            // update rotation
            from.rotation = transform.rotation;
            target.rotation = transform.rotation;
            // update target rotation by 90 degrees
            target.transform.Rotate(0f, 90f, 0f);
            UpdateRotation = true;
            Debug.Log("Start");
        }

        Debug.Log("Rotating");
        transform.rotation = Quaternion.Lerp(from.rotation, target.rotation, (elapsedTime / waitTime));
        elapsedTime += Time.deltaTime;

        // check if two rotations are matched
        // update bool to false
        if (Quaternion.Dot(transform.rotation, target.transform.rotation) >= 1) {
            Debug.Log("End");
            Switch.FloorSwitchOn = false;
            UpdateRotation = false;
            elapsedTime = 0;
        }
    }

    // public static void OnFloorSwitchOff()
    // {
    //     currentRotation = to.rotation.y - 90;
    //     to.rotation = Quaternion.Euler(0, currentRotation, 0);
    // }

    // IEnumerator OnFloorRotationCooldown()
    // {
    //     float elapsedTime = 0;
    //     float waitTime = 1f;
 
    //     while (elapsedTime < waitTime)
    //     {
    //         transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, (elapsedTime / waitTime));
    //         elapsedTime += Time.deltaTime;
        
    //         // Yield here
    //         yield return null;
    //     }  
        
    //     Switch.FloorSwitchOn = false;
    //     yield return null;
    // }

    // IEnumerator OnFloorSwitchOff()
    // {
    //     float elapsedTime = 0;
    //     float waitTime = 1f;
 
    //     while (elapsedTime < waitTime)
    //     {
    //         SwitchWiredTo.transform.rotation = Quaternion.Lerp(SwitchWiredTo.transform.rotation, origin.rotation, (elapsedTime / waitTime));
    //         elapsedTime += Time.deltaTime;
        
    //         // Yield here
    //         yield return null;
    //     }  
        
    //     Switch.SwitchOn = false;
    //     yield return null;
    // }
}
