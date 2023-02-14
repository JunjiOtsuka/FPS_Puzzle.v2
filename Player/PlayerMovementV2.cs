using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementV2 : MonoBehaviour
{
    // public PlayerInputAction playerInputAction;
    public static InputAction movement;
    public static InputAction crouchAction;
    public static InputAction runAction;
    public static InputAction jumpAction;
    public static InputAction interactClick;
    public static InputAction leftClick;
    public static InputAction rightClick;
    public static InputAction options;
    public static InputAction Weapon1;
    public static InputAction Weapon2;
    public static InputAction Tactical1;
    public static PlayerInput _PlayerInput;
    public PlayerStateManager playerStateManager;
    public GameObject UI_Master;

    public Rigidbody _rb;

    [Header("CooldownTimer")]
    public CooldownManager m_CD;
    public CooldownManager m_SlideCD;

    [Header("Walking")]
	float runSpeed; 
	public float walkSpeed = 10;
	float initialSpeed;
    Vector3 moveDirection = Vector3.zero;

    [Header("Crouching")]
    public float crouchSpeed;
    public static float maxHeight = 3;
    CapsuleCollider cc;

    [Header("Jumping")]
    public float jumpSpeed = 100f;


    [Header("WallAction")]
    public PhysicMaterial slippery;
    public PhysicMaterial maxFriction;
	public float WallSideWalkSpeed = 200f;
    public bool bWallStick;
    public bool bWallBounce;
    public bool bWallMoveInput;
    public float wallClimbSpeed = 10000f;
    public Vector3 SavedVelocity;
    [Header("WallRunSpeed")]
    public float wallRunForce;
    [Header("WallJump")]
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallJumpForwardForce;
    public float wallRunForceMultiplier;

    [Header("WallRunCamera")]
    public float camTilt;
    public float elapsedTime;
    public float camTiltTime;
    public float tilt { get; private set; }
    public bool cameraTiltProgress = false;
    public float wallRunCameraTilt, maxWallRunCameraTilt, wallRunningCameraTiltMultiplier;
    
    [Header("Camera")]
    public float maxUpDown;
    Camera mainCam;
    float limitVertCameraRotation = 0;
    [Header("MouseSetting")]
    public float mouseSensitivity = 100f;
    Vector2 mouseInput;
    float xRotation = 0f;
    float mouseX, mouseY;

    [Header("PlayerUI")]
    public UICanvasManager _UICanvasManager;
    bool PlayerUsingUI = false;

    [Header("Max Y Velocity")]
    public float maxYVelocity;

    //Interacting
    bool IsInteracting = false;

    [Header("CurrentlyEquipped")]
    public CurrentlyEquipped _CurrentlyEquipped;

    private void Awake()
    {
 
        _PlayerInput = GetComponent<PlayerInput>();   
        _CurrentlyEquipped = GetComponent<CurrentlyEquipped>();

        _rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();

        //movement related variables
        movement = InputManager.inputActions.Player.Movement;
        crouchAction = InputManager.inputActions.Player.Crouch;
        runAction = InputManager.inputActions.Player.Run;
        jumpAction = InputManager.inputActions.Player.Jump;
        interactClick = InputManager.inputActions.Player.Interact;
        leftClick = InputManager.inputActions.Player.Fire1;
        rightClick = InputManager.inputActions.Player.Fire2;
        options = InputManager.inputActions.Player.Options;
        Weapon1 = InputManager.inputActions.Player.Weapon1;
        Weapon2 = InputManager.inputActions.Player.Weapon2;
        Tactical1 = InputManager.inputActions.Player.Tactical1;
        InputManager.inputActions.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        InputManager.inputActions.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        // InputManager.inputActions.Player.Run.performed += DoRun;
        // InputManager.inputActions.Player.Run.canceled += DoWalk;
    }

    private void Start()
    {
        mainCam = GetComponentInChildren<Camera>();
    	Cursor.lockState = CursorLockMode.Locked;
        initialSpeed = walkSpeed;
        crouchSpeed = initialSpeed / 1.1f;
        runSpeed = initialSpeed * 1.1f;
        PlayerActionMapManager.ToggleActionMap(InputManager.inputActions.Player, "Player");
        // InputManager.ToggleActionMap(InputManager.inputActions.Player);
        DisableCursor();

        m_CD = new CooldownManager();
        m_SlideCD = new CooldownManager();
        // if (WeaponStateManager.WeaponState == WeaponState.BAREHAND) EnableBareHand();
        // if (WeaponStateManager.WeaponState == WeaponState.GRAPPLINGHOOK) EnableGrapple();
    }

    private void Update()
    {
        if (PlayerActionMapManager.playerInput.currentActionMap.name != "Player") 
        {
            return;
        }

        if (mouseInput.x != 0 || mouseInput.y != 0) 
        {
            DoLook();
        }

        /*************JUMP***************/
        if (jumpAction.WasPerformedThisFrame()) 
        {
            if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && 
                PlayerStateManager.checkGroundState(GroundState.ONGROUND)) 
            {
                DoJump();
            }
        }

        /*************CROUCHING************/
        if (crouchAction.IsPressed()) 
        {
            DoStartCrouch();
        } 
        else
        {
            if (CrouchDetector.canStandUp) {
                DoStopCrouch();
            }
        }

        /*************SLIDING****************/
        if (PlayerStateManager.checkPlayerState(PlayerState.SLIDE))
        {
            DoSlide();
        } 

        /********Wall actions**********/
        DoWallAction();

        //limit character speed on ground
        if (!PlayerStateManager.checkGroundState(GroundState.ONGROUND))
        {
            SetMaxVelocity(20f);
        }
        //if player state is wallrunning, do wallrun.
        if (PlayerStateManager.checkPlayerState(PlayerState.WALLRUNNING)) 
        {
            DoWallrun();
        }
        //can player wallrun?
        if (PlayerStateManager.checkWallRunState(WallRunState.ABLE)) 
        {
            //do camera tilt
            DoCameraTilt();
            //when player inputs jump next to a wall do wall run.
            if (jumpAction.WasPerformedThisFrame())
            {
                PlayerStateManager.UpdatePlayerState(PlayerState.WALLRUNNING);
            }
        }
        else if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && tilt != 0)
        {
            //otherwise stop camera tilt
            StopCameraTilt();
        }
        if (PlayerStateManager.checkPlayerState(PlayerState.IDLE) && PlayerStateManager.checkJumpState(JumpState.NOT_JUMPING))
        {
            if (LiftStateManager.liftState == LiftStateManager.LiftState.NONE)
            {
                if (_rb.useGravity == false)
                {
                    _rb.useGravity = true;
                }
            }
        }
        //Wallrun Jump
        if (PlayerStateManager.CanWallJump && jumpAction.WasPerformedThisFrame()) {
            DoWallJump();
        }
        //Wallrun Jump
        if (PlayerStateManager.WRState == WallRunState.UNABLE && 
            WallDetection.state == WallState.NOWALL &&
            PlayerStateManager.state == PlayerState.WALLRUNNING)
        {
            DoWallJump();
            // if (!playerStateManager.IsWallRunning)
            // {
            //     StopCameraTilt();
            // }
        }
        if (!PlayerUsingUI && options.WasPerformedThisFrame()) 
        {
            OnEnterOption();
        } 
        else if (PlayerUsingUI && InputManager.inputActions.UI.Cancel.WasPerformedThisFrame()) 
        {
            OnExitOption();
        }
        if (Weapon1.WasPerformedThisFrame())
        {
            _CurrentlyEquipped.EnableBareHand();
        }
        if (Weapon2.WasPerformedThisFrame())
        {
            _CurrentlyEquipped.EnableGrapple();
        }

        //Update camera tilt
    	mainCam.transform.localRotation = Quaternion.Euler(limitVertCameraRotation, 0, tilt);
    }

    private void FixedUpdate() 
    {
        /*********WALKING**********/
        if (movement.ReadValue<Vector2>().x != 0 || movement.ReadValue<Vector2>().y != 0) {
            OnWalk();
        }

        //player no inputs
        if (movement.ReadValue<Vector2>().x == 0 && movement.ReadValue<Vector2>().y == 0) {
            //when on ground
            if (!PlayerStateManager.AboveWRThreshold)
            {
                if (!PlayerStateManager.checkPlayerState(PlayerState.SLIDE))
                {
                    //per frame retain 85% velocity 
                    _rb.velocity = _rb.velocity * 0.85f;
                } 
            }
        }
    }

    private void DoLook() 
    {
        if (PlayerUsingUI) {
            return;
        }
        transform.Rotate(0, mouseInput.x * mouseSensitivity, 0);
    	limitVertCameraRotation -= mouseInput.y * mouseSensitivity;
    	limitVertCameraRotation = Mathf.Clamp(limitVertCameraRotation, -maxUpDown, maxUpDown);
    }

    private void OnWalk()
    {
        if (PlayerStateManager.state == PlayerState.WALLRUNNING) {
            return;
        }
        var vertical = movement.ReadValue<Vector2>().y;
        var horizontal = movement.ReadValue<Vector2>().x;

        moveDirection = (transform.right * movement.ReadValue<Vector2>().x + transform.forward * movement.ReadValue<Vector2>().y).normalized;
        
        _rb.AddForce(moveDirection * walkSpeed - _rb.velocity, ForceMode.Acceleration);

        if (PlayerStateManager.checkPlayerState(PlayerState.WALLRUNNING)) {
            SetMaxVelocity(50f);
        }

        if (PlayerStateManager.checkPlayerState(PlayerState.RUN)) {
            DoRun();
        } else if (PlayerStateManager.checkPlayerState(PlayerState.WALK)) {
            DoWalk();
        } 
    }

    private void OnRun()
    {
        var vertical = movement.ReadValue<Vector2>().y;
        if (vertical > 0)
        {
            if (PlayerStateManager.state != PlayerState.CROUCH) {
                walkSpeed = runSpeed;
            }
        }
    }

    private void DoStartCrouch() 
    {
        //adjust camera height
        cc.height = cc.height/2;
        cc.center = new Vector3 (0f, cc.center.y / 2, 0f);
        float groundVelocity = new Vector2(_rb.velocity.x, _rb.velocity.z).magnitude;
        mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (groundVelocity > 7) {
            PlayerStateManager.UpdatePlayerState(PlayerState.SLIDE);
            walkSpeed = crouchSpeed; //sliding
        } else if (groundVelocity <= 7) {
            walkSpeed = initialSpeed * 0.75f; // crouch walking
            SetMaxVelocity(5f);
        }
    }

    private void DoStopCrouch()
    {
        cc.height = 3;
        cc.center = new Vector3 (0f, cc.center.y * 2, 0f);
        mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        walkSpeed = initialSpeed;
    } 

    void DoSlide()
    {
        m_SlideCD.SetCDTimer(0.7f);
        m_SlideCD.StartCDTimer(m_SlideCD);

        //cooldown ended
        if (m_SlideCD.bCDEnd())
        {
            //reset velocity to zero
            _rb.velocity = Vector3.zero;
            m_SlideCD.EndCDTimer();
            m_SlideCD.ResetCDTimer();
        }
        //ongoing cooldown timer
        else 
        {
            //conditions to reset timer
                //player crouch spamming
                //above threshold
            if (crouchAction.WasPerformedThisFrame()
               || PlayerStateManager.AboveWRThreshold  )
            {
                m_SlideCD.ResetCDTimer();
            }
            if (PlayerStateManager.AboveWRThreshold) 
            {
                m_SlideCD.ResetCDTimer();
            }
        }
    }

    private void DoJump() 
    {
        _rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }

    private void DoWalk() 
    {
        // walkSpeed = initialSpeed;
        SetMaxVelocity(10f);
    }

    private void DoRun() 
    {
        var vertical = movement.ReadValue<Vector2>().y;
        //forward input
        if (vertical > 0)
        {
            //run speed
            if (PlayerStateManager.state != PlayerState.CROUCH) {
                // walkSpeed = runSpeed;
                SetMaxVelocity(20f);
            } 
        }
        //other input
        else 
        {
            //walk speed
            DoWalk();
        }
    }

    void DoWallAction()
    {
        /********Wall actions**********/

        //reset condition when player is on ground
        if (!PlayerStateManager.AboveWRThreshold) 
        {
            cc.material = slippery;
            bWallBounce = false;
            bWallStick = false;
            bWallMoveInput = false;
            m_CD.ResetCDTimer();
            return;
        }

        //condition when player is climbing and have their back to the wall
        if (WallDetection.backWallhit.transform) 
        {
            if (PlayerStateManager.AboveWRThreshold && !PlayerStateManager.checkPlayerState(PlayerState.WALLRUNNING))
            {
                _rb.useGravity = true;
                cc.material = slippery;
                bWallBounce = false;
                bWallStick = false;
                bWallMoveInput = false;
                m_CD.ResetCDTimer();
            }
        }

        if (!WallDetection.frontWallhit.transform) 
        {
            if (PlayerStateManager.AboveWRThreshold)
            {
                _rb.useGravity = true;
                cc.material = slippery;
                bWallBounce = false;
                bWallStick = false;
                bWallMoveInput = false;
                m_CD.ResetCDTimer();
                return;
            }
        }
        var target = WallDetection.frontWallhit.transform.position - transform.position;
        //get the angle between rigidbody velocity and negative hit normal
        var angle = Vector3.Angle(target, transform.forward);
        //player looking away from the wall
        if (angle >= 90) return;

        //velocity angle towards wall
        var VelocityTowardsWall = Vector2.Angle(new Vector2(target.x, target.z), new Vector2(_rb.velocity.x, _rb.velocity.z));
        //velocity magnitude
        var VelocityMagnitude = _rb.velocity.magnitude;
        
        //player is not sticking to a wall
        if(!bWallStick) 
        {
            _rb.useGravity = true;

            //conditions to wall stick

            //when player is coming in at a certain velocity towards wall
            if (VelocityTowardsWall < 90 && !m_CD.bCDEnd())
            {
                SavedVelocity = _rb.velocity;
                bWallStick = true;
            }
            //when player input forward action
            if (movement.ReadValue<Vector2>().y > 0 && !m_CD.bCDEnd())
            {
                bWallStick = true;
            }
        }
        //player is sticking to a wall
        else if(bWallStick) 
        {
            //wall stick characteristic
            _rb.useGravity = false;
            _rb.velocity = Vector3.zero;
            _rb.AddForce(-WallDetection.frontWallhit.normal * 800f); //apply force towards the wall to compensate player drifting away from wall

            //set and start cooldown when player initially sticks to wall
            m_CD.SetCDTimer(1/3f);
            m_CD.StartCDTimer(m_CD);

            //within this time frame if there is no input
            if  (!bWallBounce && !bWallMoveInput)
            {
                if (m_CD.bCDEnd())
                {
                    cc.material = slippery;
                    bWallStick = false;
                    _rb.useGravity = true;
                    m_CD.EndCDTimer();
                }
                else 
                {
                    //change physics material
                    cc.material = maxFriction;
                }
            } 
            

            //player side input to wall 
            if (movement.ReadValue<Vector2>().x != 0)
            {
                //set and start cooldown when player sticks to wall
                m_CD.SetCDTimer(2.5f);
                m_CD.StartCDTimer(m_CD);

                if (m_CD.bCDEnd())
                {
                    cc.material = slippery;
                    bWallStick = false;
                    _rb.useGravity = true;
                    m_CD.EndCDTimer();
                }
                else 
                {
                    //change physics material
                    cc.material = maxFriction;
                    Debug.Log("Move to side");
                    walkSpeed = WallSideWalkSpeed;
                    bWallMoveInput = true;
                }
            }

            //player climbing (forward input to wall) 
            if (movement.ReadValue<Vector2>().y > 0) 
            {
                //set and start cooldown when player sticks to wall
                m_CD.SetCDTimer(2.5f);
                m_CD.StartCDTimer(m_CD);

                if (m_CD.bCDEnd())
                {
                    DoJump();

                    cc.material = slippery;
                    bWallStick = false;
                    _rb.useGravity = true;
                    m_CD.EndCDTimer();
                }
                else 
                {
                    //change physics material
                    cc.material = maxFriction;
                    Debug.Log("climb");
                    _rb.AddForce(transform.up * wallClimbSpeed);
                    bWallMoveInput = true;
                }
            }

            //player away input from wall
            if (movement.ReadValue<Vector2>().y < 0) 
            {
                bWallStick = false;
                _rb.useGravity = true;
                bWallMoveInput = true;
                cc.material = slippery;
            }

            if (bWallMoveInput)
            {
                if (m_CD.bCDEnd())
                {
                    cc.material = slippery;
                    bWallStick = false;
                    _rb.useGravity = true;
                    m_CD.EndCDTimer();
                }
                else 
                {
                    //change physics material
                    cc.material = maxFriction;
                }
            }

            if (!bWallMoveInput) {
                //player jump input
                if (jumpAction.WasPerformedThisFrame() && !bWallMoveInput && !bWallBounce) 
                {
                    _rb.useGravity = true;
                    bWallStick = false;
                    
                    //reset physics material
                    cc.material = slippery;

                    //reflect and apply new velocity
                    _rb.velocity = Vector3.Reflect(SavedVelocity, WallDetection.frontWallhit.normal);
                    //Do jump action
                    DoJump();

                    bWallBounce = true;
                    //reset saved velocity
                    SavedVelocity = Vector3.zero;
                }
            }
        }
    }

    public void DoWallrun() 
    {
        _rb.useGravity = false;
        _rb.velocity = new Vector3 (_rb.velocity.x, 0f, _rb.velocity.z);

        Vector3 wallNormal = WallDetection.state == WallState.RIGHTWALL ? WallDetection.rightWallhit.normal : WallDetection.leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        _rb.AddForce(wallForward * wallRunForce, ForceMode.Impulse);
        
        _rb.AddForce(-wallNormal * 100, ForceMode.Force);

        PlayerStateManager.IsWallRunning = true;

    }

    public void DoCameraTilt() 
    {
        if (WallDetection.state == WallState.RIGHTWALL) 
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        } 
        
        if (WallDetection.state == WallState.LEFTWALL) 
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
    }

    public void StopCameraTilt()
    {
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

    public void DoWallJump() 
    {

        Vector3 wallNormal = WallDetection.state == WallState.RIGHTWALL ? WallDetection.rightWallhit.normal : WallDetection.leftWallhit.normal;
        Vector3 forceToApply = transform.forward * wallJumpForwardForce + transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        _rb.AddForce(forceToApply * wallRunForceMultiplier, ForceMode.Impulse);
        StopWallRun();
    }

    public void StopWallRun() 
    {
        _rb.useGravity = true;
        PlayerStateManager.IsWallRunning = false;
        PlayerStateManager.UpdatePlayerState(PlayerState.WALLJUMP);
        PlayerStateManager.UpdateJumpState(JumpState.WALLJUMP);
        StopCameraTilt();
    }

    private void DoFire1(InputAction.CallbackContext callback) 
    {
        Debug.Log("Shooting");
    }

    private void DoFire2(InputAction.CallbackContext callback) 
    {
        Debug.Log("ADSing");
    }

    private void DoInteract(InputAction.CallbackContext callback) 
    {
        Debug.Log("Interacting");
        // if (IsInteracting == false) {
        //     IsInteracting = true;
        // } else if (IsInteracting == true) {
        //     IsInteracting = false;
        // }
    }

    private void OnEnterOption()
    {
        UI_Master.SetActive(true);
        _UICanvasManager.UIEnableButtons("UI:Options");
        _UICanvasManager.UIEnableButtons("UI:Background");
        EnableCursor();
        PlayerUsingUI = true;
        PlayerActionMapManager.ToggleActionMap(InputManager.inputActions.UI, "UI");
        // InputManager.ToggleActionMap(InputManager.inputActions.UI);
        InputManager.inputActions.Player.Disable();
        InputManager.inputActions.UI.Enable();
    }

    private void OnExitOption()
    {
        UI_Master.SetActive(false);
        DisableCursor();
        PlayerUsingUI = false;
        PlayerActionMapManager.ToggleActionMap(InputManager.inputActions.Player, "Player");
        // InputManager.ToggleActionMap(InputManager.inputActions.Player);
        InputManager.inputActions.Player.Enable();
        InputManager.inputActions.UI.Disable();
    }

    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void SetMaxVelocity(float maxGroundVel) 
    {
        Vector3 xzVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        Vector3 yVel = new Vector3(0, _rb.velocity.y, 0);

        xzVel = Vector3.ClampMagnitude(xzVel, maxGroundVel);
        yVel = Vector3.ClampMagnitude(yVel, maxYVelocity);

        _rb.velocity = xzVel + yVel;
    }



}
