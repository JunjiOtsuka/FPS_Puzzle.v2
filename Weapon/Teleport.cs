using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportPrefab;
    public LayerMask whatIsGround;
    public bool GroundCollision;
    public float sphereRadius;
    Vector3 origin;
    Vector3 direction;
    float distanceToObstacle;
    public float maxDistance = 30f;
    RaycastHit hit;

    [Header("Circle")]
    float _A11; float _A12;
    float _A21; float _A22;
    Vector3 teleportTo;
    Vector3 m_CharacterHeight;
    float WorldRotationDegrees;
    float AngleFromGround;
    float AngleFromWall;
    public float radius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        teleportPrefab = Instantiate(teleportPrefab);
        m_CharacterHeight = new Vector3(0f, 1.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        direction = transform.forward;
        origin = transform.position;

        CalculateAngleFromGround();

        CalculateRotation();

        GroundCollision = TeleportDetection(whatIsGround, GroundCollision);

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        if (GroundCollision) {
            //if player looking above horizon
            if (AngleFromGround <= 90) {
                if (PlayerMovementV2.Tactical1.WasPerformedThisFrame())
                {
                    transform.root.position = hit.point + m_CharacterHeight;
                }
            } else {
                //if angle is greater than 90 degrees and target location is not a slope
                if (PlayerMovementV2.Tactical1.WasPerformedThisFrame())
                {
                    //teleport the player to the ledge
                    //sum of sphere collided position + the normal of collision + height of character
                    transform.root.position = hit.point + new Vector3(-hit.normal.x, 0f, -hit.normal.z) * 3f + m_CharacterHeight;
                }
            }
        }
    }

    bool TeleportDetection(LayerMask layer, bool bCheck)
    {
        if (Physics.SphereCast(origin, 1f, transform.forward, out hit, maxDistance, layer))
        {
            distanceToObstacle = hit.distance;
            teleportPrefab.transform.position = hit.point + m_CharacterHeight;
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                bCheck = true;
                teleportPrefab.SetActive(true);
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                bCheck = false;
                teleportPrefab.SetActive(false);
            }
        } else {
            teleportPrefab.SetActive(false);
            distanceToObstacle = maxDistance;
            bCheck = false;
        }
        return bCheck;
    }

    void CalculateAngleFromGround()
    {
        //How far, in degrees angle, are we looking away from player ground
        if (hit.transform == null) return;

        //Get the angle from ground
        //grab a vector we want as a target
        Vector3 targetDir = transform.root.position - hit.point;
        //compare above target to ground's up direction
        AngleFromGround = Vector3.Angle(targetDir, hit.transform.up);

        //Get the angle from wall
        //get target xz vector
        Vector2 targetDir2D = new Vector2(transform.root.position.x, transform.root.position.z) - new Vector2(hit.point.x, hit.point.z);
        //compare angle to the wall
        AngleFromWall = Vector2.Angle(targetDir2D, new Vector2(hit.normal.x, hit.normal.z));

    }

    void CalculateRotation()
    {
        //update player world rotation
        //World rotation
        WorldRotationDegrees = (transform.root.eulerAngles.y) * Mathf.Deg2Rad;
        //update xz relative to four quadrants
        var x = Mathf.Sin(WorldRotationDegrees);
        var z = Mathf.Cos(WorldRotationDegrees);
        //when x is 0
        if (x == 0)
        {
            //check world rotation between angles
            if (WorldRotationDegrees >= 0 && WorldRotationDegrees < 180)
            {
                //x quadrant I and IV
                x = 1;
            }
            if (WorldRotationDegrees >= 180 && WorldRotationDegrees < 360)
            {
                //x quadrant II and III
                x = -1;
            }
        }
        //same thing for z
        if (z == 0)
        {
            if (WorldRotationDegrees >= 270 && WorldRotationDegrees < 90)
            {
                //z quadrant I and II
                z = 1;
            }
            if (WorldRotationDegrees >= 90 && WorldRotationDegrees < 270)
            {
                //z quadrant III and IV
                z = -1;
            }
        }

        //Angle between player to wall
        AngleFromWall = Mathf.Floor(AngleFromWall);
        if (AngleFromWall != 0 && AngleFromWall < 90) 
        {
            AngleFromWall += 90;
        }
        // Debug.Log(AngleFromWall);
        AngleFromWall = (AngleFromWall) * Mathf.Deg2Rad;
        _A11 = Mathf.Cos(AngleFromWall); _A12 = -Mathf.Sin(AngleFromWall);
        _A21 = Mathf.Sin(AngleFromWall); _A22 = Mathf.Cos(AngleFromWall);

        var _Ax = (_A11 + _A12);
        var _Ay = (_A21 + _A22);
        //update teleport location
        teleportTo = new Vector3((float)_Ax * x, 1.5f, (float)_Ay * z);

        // var _Ax = Math.Round(_A11 + _A12);
        // var _Ay = Math.Round(_A21 + _A22);
        // teleportTo = new Vector3((float)_Ax * x, 1.5f, (float)_Ay * z);
        Debug.Log(hit.normal);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * distanceToObstacle);
        Gizmos.DrawWireSphere(origin + direction * distanceToObstacle, sphereRadius);
    }
}
