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
    public static PlayerInput _PlayerInput;
    public PlayerStateManager playerStateManager;
    public GameObject UI_Master;

    public Rigidbody _rb;

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
        InputManager.inputActions.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        InputManager.inputActions.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        InputManager.inputActions.Player.Run.performed += DoRun;
        InputManager.inputActions.Player.Run.canceled += DoWalk;
    }

    private void Start()
    {
        mainCam = GetComponentInChildren<Camera>();
    	Cursor.lockState = CursorLockMode.Locked;
        initialSpeed = walkSpeed;
        crouchSpeed = initialSpeed / 2;
        runSpeed = initialSpeed * 2;
        PlayerActionMapManager.ToggleActionMap(InputManager.inputActions.Player, "Player");
        // InputManager.ToggleActionMap(InputManager.inputActions.Player);
        DisableCursor();
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
        if (jumpAction.WasPerformedThisFrame()) 
        {
            if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && 
                PlayerStateManager.checkGroundState(GroundState.ONGROUND)) 
            {
                DoJump();
            }
        }
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
        if (!PlayerStateManager.checkGroundState(GroundState.ONGROUND))
        {
            SetMaxVelocity(20f);
        }
        if (PlayerStateManager.checkPlayerState(PlayerState.WALLRUNNING)) 
        {
            DoWallrun();
        }
        if (PlayerStateManager.checkWallRunState(WallRunState.ABLE)) 
        {
            DoCameraTilt();
            // StartCameraTilt();
        }
        else if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && tilt != 0)
        {
            StopCameraTilt();
            // CancelCameraTilt();
        }
        if (PlayerStateManager.CanWallJump && jumpAction.WasPerformedThisFrame()) {
            DoWallJump();
            // if (!playerStateManager.IsWallRunning)
            // {
            //     StopCameraTilt();
            // }
        }
        if (PlayerStateManager.WRState == WallRunState.UNABLE && 
            PlayerStateManager.wallState == WallState.NOWALL &&
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
        if (movement.ReadValue<Vector2>().x != 0 || movement.ReadValue<Vector2>().y != 0) {
            OnWalk();
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
        moveDirection = (transform.right * movement.ReadValue<Vector2>().x + transform.forward * movement.ReadValue<Vector2>().y).normalized;
        _rb.AddForce(moveDirection * walkSpeed, ForceMode.Acceleration);

        if (PlayerStateManager.checkPlayerState(PlayerState.WALLRUNNING)) {
            SetMaxVelocity(50f);
        }

        if (PlayerStateManager.state == PlayerState.RUN) {
            SetMaxVelocity(20f);
        } else if (PlayerStateManager.state == PlayerState.WALK) {
            SetMaxVelocity(10f);
        } 
        
    }

    private void OnRun()
    {
        if (PlayerStateManager.state != PlayerState.CROUCH) {
            walkSpeed = runSpeed;
        }
    }

    private void DoStartCrouch() 
    {
        cc.height = cc.height/2;
        cc.center = new Vector3 (0f, cc.center.y / 2, 0f);
        float groundVelocity = new Vector2(_rb.velocity.x, _rb.velocity.z).magnitude;
        mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (groundVelocity > 7) {
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


    private void DoJump() 
    {
        _rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }

    private void DoWalk(InputAction.CallbackContext callback) 
    {
        walkSpeed = initialSpeed;
    }

    private void DoRun(InputAction.CallbackContext callback) 
    {
        
    }

    public void DoWallrun() 
    {
        _rb.useGravity = false;
        _rb.velocity = new Vector3 (_rb.velocity.x, 0f, _rb.velocity.z);

        Vector3 wallNormal = PlayerStateManager.wallState == WallState.RIGHTWALL ? WallDetection.rightWallhit.normal : WallDetection.leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        _rb.AddForce(wallForward * wallRunForce, ForceMode.Impulse);
        
        _rb.AddForce(-wallNormal * 100, ForceMode.Force);

        PlayerStateManager.IsWallRunning = true;

    }

    public void DoCameraTilt() 
    {
        if (PlayerStateManager.wallState == WallState.RIGHTWALL) 
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        } 
        
        if (PlayerStateManager.wallState == WallState.LEFTWALL) 
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
    }

    public void StopCameraTilt()
    {
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

    public void DoWallJump() {
        Vector3 wallNormal = PlayerStateManager.wallState == WallState.RIGHTWALL ? WallDetection.rightWallhit.normal : WallDetection.leftWallhit.normal;
        Vector3 forceToApply = transform.forward * wallJumpForwardForce + transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        _rb.AddForce(forceToApply * wallRunForceMultiplier, ForceMode.Impulse);
        StopWallRun();
    }

    public void StopWallRun() {
        _rb.useGravity = true;
        PlayerStateManager.IsWallRunning = false;
        playerStateManager.UpdatePlayerState(PlayerState.WALLJUMP);
        playerStateManager.UpdateJumpState(JumpState.WALLJUMP);
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
