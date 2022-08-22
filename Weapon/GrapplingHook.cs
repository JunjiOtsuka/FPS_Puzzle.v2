using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 50f;
    private static SpringJoint joint;
    private Vector3 currentGrapplePosition;
    public static GrapplingState GrapplingState;
    public float springF = 100f;
    public float damperF = 100f;
    public float massScaleF = 100f;
    RaycastHit hit;
    public static bool CanGrapple = false;

    void OnEnable()
    {
        lr.enabled = true;
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
            if (Vector3.Distance(player.position, grapplePoint) > maxDistance) {
                StopGrapple();
            }
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

        float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.4f;
        joint.minDistance = distanceFromPoint * 0.25f;

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
        lr.enabled = false;
        lr.positionCount = 0;
        Destroy(joint);
        GrapplingState = GrapplingState.NONE;
    }

    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public static bool IsGrappling() {
        return joint != null;
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
            StartGrapple();
        }
    }

    public void RightClick()
    {
        if (IsGrappling() && PlayerMovementV2.rightClick.WasPerformedThisFrame()) {
            StopGrapple();
        }
    }
}
