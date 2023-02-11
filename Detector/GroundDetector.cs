using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public LayerMask mask;
    public float angle;
    public float rayDistance;
    public float distanceFromPlayer;

    public static RaycastHit hit;
    public static bool isGrounded;
    public static GroundState state;
    
    void Update()
    {
        Ray rayBottom = new Ray (transform.position, -transform.up);
        Ray rayBottom2 = new Ray (transform.position + new Vector3 (distanceFromPlayer, 0, -distanceFromPlayer), -transform.up);
        Ray rayBottom3 = new Ray (transform.position + new Vector3 (0, 0, distanceFromPlayer),  -transform.up);
        Ray rayBottom4 = new Ray (transform.position + new Vector3 (-distanceFromPlayer, 0, -distanceFromPlayer),  -transform.up);

        if (Physics.Raycast (rayBottom, out hit, rayDistance, mask) 
        || Physics.Raycast (rayBottom2, out hit, rayDistance, mask) 
        || Physics.Raycast (rayBottom3, out hit, rayDistance, mask)
        || Physics.Raycast (rayBottom4, out hit, rayDistance, mask)
        ) {
            //reset acceleration to 0
            isGrounded = true;
            state = GroundState.ONGROUND;
            Debug.DrawLine(rayBottom.origin, hit.point, Color.red);
        } else {
            isGrounded = false;
            state = GroundState.INAIR;
            Debug.DrawLine(rayBottom.origin, rayBottom.origin + rayBottom.direction * rayDistance, Color.green);
            // Debug.DrawLine(rayBottom2.origin, rayBottom2.origin + rayBottom2.direction * rayDistance, Color.green);
            // Debug.DrawLine(rayBottom3.origin, rayBottom3.origin + rayBottom3.direction * rayDistance, Color.green);
            // Debug.DrawLine(rayBottom4.origin, rayBottom4.origin + rayBottom4.direction * rayDistance, Color.green);
        }

        switch (GroundDetector.state) 
        {
            case GroundState.ONGROUND:
            {
                PlayerStateManager.AboveWRThreshold = false;
                break;
            }
            case GroundState.INAIR:
            {
                PlayerStateManager.AboveWRThreshold = true;
                break;
            }
        }
    }
}
