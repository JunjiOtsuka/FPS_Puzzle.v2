using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRotate : MonoBehaviour
{
    public Transform from;
    public Transform target;
    public float targetRotation;
    public static float currentRotation;
    public float elapsedTime = 0;
    public float waitTime = 1f;
    public bool boolReset;
    public bool boolStartRotation = true;
    public bool boolUpdateRotation = false;
    public bool boolStopRotation;
    public static int getAllTargets { get; set; }
    public bool FloorSwitchOn = false;

    void Update()
    {
        if (FloorSwitchOn)
        {
            if (boolReset)
            {
                ResetCondition();
            }
            OnFloorSwitchOn();
        } 
    }

    public void OnFloorSwitchOn()
    {
        if (boolReset)
        {
            Debug.Log("reset");
            FloorRotate.getAllTargets = 0;
            boolStartRotation = true;
            boolUpdateRotation = false;
            boolStopRotation = false;
            boolReset = false;
        }

        // start rotation
        if (boolStartRotation)
        {
            Debug.Log("start");
            StartRotation();
        }

        //update rotation here
        // Debug.Log("update");
        if (boolUpdateRotation)
        {
            UpdateRotation();
        }

        // stop rotation when it reaches the destination rotation
        // Debug.Log("stop");
        if (Quaternion.Angle(transform.rotation, target.transform.rotation) <= 0 && !boolStopRotation) {
            StopRotation();
        }
    }

    public void StartRotation()
    {
        from.rotation = transform.rotation;
        target.rotation = transform.rotation;
        // update target rotation by 90 degrees
        target.transform.Rotate(0f, targetRotation, 0f);
        boolStartRotation = false;
        boolUpdateRotation = true;
        boolStopRotation = false;
    }

    public void UpdateRotation()
    {
        transform.rotation = Quaternion.Lerp(from.rotation, target.rotation, (elapsedTime / waitTime));
        elapsedTime += Time.deltaTime;
    }

    public void StopRotation()
    {
        boolStartRotation = false;
        boolUpdateRotation = false;
        boolStopRotation = true;

        boolReset = true;

        Debug.Log("stop");
        //stop the master switch when all floors reaches destination
        FloorSwitchOn = false;

        elapsedTime = 0;
        getAllTargets++;
    }

    public void OnFloorSwitchOff()
    {
        // currentRotation = to.rotation.y - 90;
        // to.rotation = Quaternion.Euler(0, currentRotation, 0);
        // Switch.FloorSwitchOn = false;
    }

    public void ResetCondition()
    {
        Debug.Log("reset");
        FloorRotate.getAllTargets = 0;
        boolStartRotation = true;
        boolUpdateRotation = false;
        boolStopRotation = false;
        boolReset = false;
    }

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
