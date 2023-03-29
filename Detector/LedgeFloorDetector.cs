using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeFloorDetector : MonoBehaviour
{
    public LayerMask mask;
    public float frontBackWallRayDistance;

    public static bool bLedgeFloorHit;

    public static RaycastHit ledgeFloorHit;

    public static LedgeState state;

    public enum LedgeState
    {
        NONE,
        LEDGEFLOOR,
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayFront = new Ray (transform.position, transform.forward);

        if (Physics.Raycast (rayFront, out ledgeFloorHit, frontBackWallRayDistance, mask)) {
            bLedgeFloorHit = true;
            state = LedgeState.LEDGEFLOOR;
            Debug.DrawLine(rayFront.origin, ledgeFloorHit.point, Color.red);
        } else {
            bLedgeFloorHit = false;
            Debug.DrawLine(rayFront.origin, rayFront.origin + rayFront.direction * frontBackWallRayDistance, Color.green);
        }
    }
}
