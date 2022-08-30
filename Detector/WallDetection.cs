using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    public LayerMask mask;
    public float angle;
    public float sideWallRayDistance;
    public float frontBackWallRayDistance;

    public static bool leftWall;
    public static bool rightWall;
    public static bool frontWall;
    public static bool backWall;

    public static RaycastHit leftWallhit;
    public static RaycastHit rightWallhit;
    public static RaycastHit frontWallhit;
    public static RaycastHit backWallhit;

    public static WallState state;

    // Update is called once per frame
    void Update()
    {
        Ray rayLeft   = new Ray (transform.position, -transform.right);
        Ray rayLeft2  = new Ray (transform.position, Quaternion.Euler(0, angle, 0) * -transform.right);
        Ray rayLeft3  = new Ray (transform.position, Quaternion.Euler(0, -angle, 0) * -transform.right);
        Ray rayRight  = new Ray (transform.position, transform.right);
        Ray rayRight2 = new Ray (transform.position, Quaternion.Euler(0, angle, 0) * transform.right);
        Ray rayRight3 = new Ray (transform.position, Quaternion.Euler(0, -angle, 0) * transform.right);
        Ray rayFront = new Ray (transform.position, transform.forward);
        Ray rayBehind = new Ray (transform.position, -transform.forward);

        RaycastHit hit2;
        RaycastHit hit3;

        if (Physics.Raycast (rayLeft,  out leftWallhit, sideWallRayDistance, mask) 
         && Physics.Raycast (rayLeft2, out hit2, sideWallRayDistance, mask) 
         && Physics.Raycast (rayLeft3, out hit3, sideWallRayDistance, mask)) {
            // create wall layer so the player can run on wall 
            // Within this condition give the player wall run
            leftWall = true;
            state = WallState.LEFTWALL;
            Debug.DrawLine(rayLeft.origin, leftWallhit.point, Color.red);
        } else {
            leftWall = false;
            Debug.DrawLine(rayLeft.origin, rayLeft.origin + rayLeft.direction * sideWallRayDistance, Color.green);
            Debug.DrawLine(rayLeft2.origin, rayLeft2.origin + rayLeft2.direction * sideWallRayDistance, Color.green);
            Debug.DrawLine(rayLeft3.origin, rayLeft3.origin + rayLeft3.direction * sideWallRayDistance, Color.green);
        }

        if (Physics.Raycast (rayRight,  out rightWallhit, sideWallRayDistance, mask) 
         && Physics.Raycast (rayRight2, out hit2, sideWallRayDistance, mask) 
         && Physics.Raycast (rayRight3, out hit3, sideWallRayDistance, mask)) {
            rightWall = true;
            state = WallState.RIGHTWALL;
            Debug.DrawLine(rayRight.origin, rightWallhit.point, Color.red);
        } else {
            rightWall = false;
            Debug.DrawLine(rayRight.origin, rayRight.origin + rayRight.direction * sideWallRayDistance, Color.green);
            Debug.DrawLine(rayRight2.origin, rayRight2.origin + rayRight2.direction * sideWallRayDistance, Color.green);
            Debug.DrawLine(rayRight3.origin, rayRight3.origin + rayRight3.direction * sideWallRayDistance, Color.green);
        }

        if (Physics.Raycast (rayFront, out frontWallhit, frontBackWallRayDistance, mask)) {
            frontWall = true;
            state = WallState.FRONTWALL;
            Debug.DrawLine(rayFront.origin, frontWallhit.point, Color.red);
        } else {
            frontWall = false;
            Debug.DrawLine(rayFront.origin, rayFront.origin + rayFront.direction * frontBackWallRayDistance, Color.green);
        }

        if (Physics.Raycast (rayBehind, out backWallhit, frontBackWallRayDistance, mask)) {
            backWall = true;
            state = WallState.BACKWALL;
            Debug.DrawLine(rayBehind.origin, backWallhit.point, Color.red);
        } else {
            backWall = false;
            Debug.DrawLine(rayBehind.origin, rayBehind.origin + rayBehind.direction * frontBackWallRayDistance, Color.green);
        }

        if (!rightWall && !leftWall && !backWall && !frontWall) 
        {
            state = WallState.NOWALL;
        }

        switch (WallDetection.state) 
        {
            case WallState.NOWALL:
            {
                PlayerStateManager.ByWall = false;
                PlayerStateManager.CanWallClimb = false;
                break;
            }
            case WallState.RIGHTWALL:
            {
                PlayerStateManager.ByWall = true;
                PlayerStateManager.CanWallClimb = false;
                break;
            }
            case WallState.LEFTWALL:
            {
                PlayerStateManager.ByWall = true;
                PlayerStateManager.CanWallClimb = false;
                break;
            }
            case WallState.FRONTWALL:
            {
                PlayerStateManager.ByWall = false;
                PlayerStateManager.CanWallClimb = true;
                break;
            }
            case WallState.BACKWALL:
            {
                PlayerStateManager.ByWall = false;
                PlayerStateManager.CanWallClimb = false;
                break;
            }
        }
    }
}
