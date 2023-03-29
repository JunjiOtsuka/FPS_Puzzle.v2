using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public LayerMask mask;
    public float frontBackWallRayDistance;

    public static bool bLedgeHit;

    public static RaycastHit ledgeHit;

    public static LedgeState state;

    public enum LedgeState
    {
        NONE,
        LEDGE,
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayFront = new Ray (transform.position, transform.forward);

        if (Physics.Raycast (rayFront, out ledgeHit, frontBackWallRayDistance, mask)) {
            bLedgeHit = true;
            state = LedgeState.LEDGE;
            Debug.DrawLine(rayFront.origin, ledgeHit.point, Color.red);
        } else {
            bLedgeHit = false;
            state = LedgeState.NONE;
            Debug.DrawLine(rayFront.origin, rayFront.origin + rayFront.direction * frontBackWallRayDistance, Color.green);
        }

        // switch (state) 
        // {
        //     case LedgeState.NONE:
        //     {
        //         Debug.Log("no ledge");
        //         break;
        //     }
        //     case LedgeState.LEDGE:
        //     {
        //         Debug.Log("ledge");
        //         Rigidbody rb = transform.root.GetComponent<Rigidbody>();

        //         break;
        //     }
        // }
    }
}
