﻿using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public Transform target;
    public LayerMask whatIsGrappleable;
    public Rigidbody rb;
    public float pullForce = 10f;
    public Transform gunTip, camera, player;
    private float maxDistance = 50f;
    private static SpringJoint joint;
    private static ConfigurableJoint configJoint;
    private Vector3 currentGrapplePosition;
    public static GrapplingState GrapplingState;
    public float springF = 100f;
    public float springFConfigJoint = 50f;
    public float damperF = 100f;
    public float massScaleF = 100f;
    RaycastHit hit;
    public static bool CanGrapple = false;

    void OnEnable()
    {
        lr.enabled = true;
        rb = GetComponentInParent<Rigidbody>();
        // if (WeaponStateManager.WeaponState == WeaponState.GRAPPLINGHOOK)
        // {
        //     WeaponStateManager.WeaponState = WeaponState.BAREHAND;
        // }
    }

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        if (WeaponStateManager.WeaponState != WeaponState.GRAPPLINGHOOK) {
            return;
        }

        GrappleDetection();

        if (IsGrappling())
        {
            rb.useGravity = false;

            //apply forward force towards hit point
            // rb.AddForce((hit.point - transform.position) * pullForce, ForceMode.Acceleration);

            //if the angle between my line of sight and hit point is greater than 90 degrees
            //break the joint
            Vector3 targetDir = grapplePoint - transform.position;
            Debug.Log(Vector3.Angle(targetDir, camera.forward));
            if (Vector3.Angle(targetDir, camera.forward) > 90) 
            { 
                // StopGrapple(); 
                StopGrapple_vConfigJoint();
            }

            //break joint if spring is greater than its max distance
            // if (Vector3.Distance(player.position, grapplePoint) > maxDistance) 
            // {
            //     StopGrapple();
            // }
        }
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    void OnDisable()
    {
        lr.enabled = false;
        StopGrapple();
    }

    void GrappleDetection()
    {
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            CanGrapple = true;
        } else {
            CanGrapple = false;
        }
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        if (!CanGrapple) return;

        GrapplingState = GrapplingState.GRAPPLING;
    
        grapplePoint = hit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        joint.breakTorque = 100f;


        //The distance grapple will try to keep from grapple point. 
        // float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

        joint.minDistance = 0f;
        joint.maxDistance = 15f;

        // joint.minDistance = distanceFromPoint * 0f;
        // joint.minDistance = distanceFromPoint * 0.25f;

        //Adjust these values to fit your game.
        joint.spring = springF;
        joint.damper = damperF;
        joint.massScale = massScaleF;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;

        lr.enabled = true;
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        rb.useGravity = true;
        lr.enabled = false;
        lr.positionCount = 0;
        Destroy(joint);
        GrapplingState = GrapplingState.NONE;
    }

    void StartGrapple_vConfigJoint() {
        if (!CanGrapple) return;

        GrapplingState = GrapplingState.GRAPPLING;
    
        grapplePoint = hit.point;
        configJoint = player.gameObject.AddComponent<ConfigurableJoint>();
        configJoint.autoConfigureConnectedAnchor = false;
        configJoint.connectedAnchor = grapplePoint;

        configJoint.breakTorque = 100f;

        configJoint.targetPosition = hit.point;

        //Adjust these values to fit your game.


        //initialize joint drive
        //x drive
        var jointDrive = configJoint.xDrive;
        jointDrive.positionSpring = springFConfigJoint;
        //update x drive
        configJoint.xDrive = jointDrive;

        //y drive
        jointDrive = configJoint.yDrive;
        jointDrive.positionSpring = 25f;
        //update
        configJoint.yDrive = jointDrive;

        //z drive
        jointDrive = configJoint.zDrive;
        jointDrive.positionSpring = springFConfigJoint;
        //update
        configJoint.zDrive = jointDrive;

        //damper
        jointDrive.positionDamper = damperF;

        //mass scale
        configJoint.massScale = massScaleF;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;

        lr.enabled = true;
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple_vConfigJoint() {
        rb.useGravity = true;
        lr.enabled = false;
        lr.positionCount = 0;
        Destroy(configJoint);
        GrapplingState = GrapplingState.NONE;
    }

    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint && !configJoint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public static bool IsGrappling() {
        return joint != null || configJoint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }

    public void EquipWeapon()
    {
        CurrentlyEquipped.LeftClickWeaponAction = LeftClick;
        CurrentlyEquipped.RightClickWeaponAction = RightClick;
        WeaponStateManager.WeaponState = WeaponState.GRAPPLINGHOOK;
    }

    public void LeftClick()
    {
        if (!IsGrappling() && PlayerMovementV2.leftClick.WasPerformedThisFrame()) {
            // StartGrapple();
            StartGrapple_vConfigJoint();
        }
    }

    public void RightClick()
    {
        if (IsGrappling() && PlayerMovementV2.rightClick.WasPerformedThisFrame()) {
            // StopGrapple();
            StopGrapple_vConfigJoint();
        }
    }
}
