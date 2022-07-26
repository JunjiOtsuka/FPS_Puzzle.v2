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
    CapsuleCollider cc;

    [Header("Jumping")]
    public float jumpSpeed = 100f;

    [Header("WallRun")]
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

    private void Awake()
    {
        // playerInputAction = new PlayerInputAction();   
        _PlayerInput = GetComponent<PlayerInput>();   
        // _UICanvasManager = GetComponentInChildren<UICanvasManager>();

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
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
    }

    private void Update()
    {
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
        if (crouchAction.WasReleasedThisFrame()) 
        {
            DoStopCrouch();
        }
        if (!PlayerStateManager.checkGroundState(GroundState.ONGROUND))
        {
            SetMaxVelocity(20f);
        }
        if (PlayerStateManager.checkPlayerState(PlayerState.WALLRUNNING)) {
            DoWallrun();
            StartCoroutine(DoCameraTilt());
        }
        if (playerStateManager.CanWallJump && jumpAction.WasPerformedThisFrame()) {
            DoWallJump();
        }
        if (PlayerStateManager.WRState == WallRunState.UNABLE || 
            PlayerStateManager.wallState == WallState.NOWALL ||
            PlayerStateManager.state != PlayerState.WALLRUNNING)
        {
            StopWallRun();
            StartCoroutine(StopCameraTilt());
        }

        if (!PlayerUsingUI && options.WasPerformedThisFrame()) 
        {
            OnEnterOption();
        } 
        else if (PlayerUsingUI && InputManager.inputActions.UI.Cancel.WasPerformedThisFrame()) 
        {
            OnExitOption();
        }
    }

    private void FixedUpdate() 
    {
        if (movement.ReadValue<Vector2>().x != 0 || movement.ReadValue<Vector2>().y != 0) {
            OnWalk();
        }

        // Vector3 xzVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        // Vector3 yVel = new Vector3(0, _rb.velocity.y, 0);

        // xzVel = Vector3.ClampMagnitude(xzVel, 10);
        // yVel = Vector3.ClampMagnitude(yVel, 50);

        // _rb.velocity = xzVel + yVel;
    }

    private void DoLook() 
    {
        if (PlayerUsingUI) {
            return;
        }
        transform.Rotate(0, mouseInput.x * mouseSensitivity, 0);
    	limitVertCameraRotation -= mouseInput.y * mouseSensitivity;
    	limitVertCameraRotation = Mathf.Clamp(limitVertCameraRotation, -maxUpDown, maxUpDown);
    	mainCam.transform.localRotation = Quaternion.Euler(limitVertCameraRotation, 0, tilt);
    }

    private void OnWalk()
    {
        if (PlayerStateManager.state == PlayerState.WALLRUNNING) {
            return;
        }
        moveDirection = (transform.right * movement.ReadValue<Vector2>().x + transform.forward * movement.ReadValue<Vector2>().y).normalized;
        _rb.AddForce(moveDirection * walkSpeed, ForceMode.Acceleration);

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
        cc.height = cc.height * 2;
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

        CameraTiltInProgress();
    }

    public bool CameraTiltInProgress() 
    {
        return (tilt == camTilt || tilt == -camTilt);
    }

    public bool CameraTiltFinished() 
    {
        return (tilt == 0);
    }

    public IEnumerator DoCameraTilt() 
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / camTiltTime;

        // Debug.Log($"START: tilt: {tilt} camtilt: {camTilt}");
        if (PlayerStateManager.wallState == WallState.RIGHTWALL) 
        {
            tilt = Mathf.Lerp(tilt, camTilt, Mathf.SmoothStep(0, 1, percentageComplete));
        } 
        else if (PlayerStateManager.wallState == WallState.LEFTWALL) 
        {
            tilt = Mathf.Lerp(tilt, -camTilt, Mathf.SmoothStep(0, 1, percentageComplete));
        }
        yield return new WaitUntil(() => CameraTiltInProgress());
    }

    public IEnumerator StopCameraTilt()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / camTiltTime;
        tilt = Mathf.Lerp(tilt, 0, Mathf.SmoothStep(0, 1, percentageComplete));
        // Debug.Log($"STOP: tilt: {tilt} camtilt: {camTilt}");
        yield return new WaitUntil(() => CameraTiltFinished());
    }

    public void DoWallJump() {
        Vector3 wallNormal = PlayerStateManager.wallState == WallState.RIGHTWALL ? WallDetection.rightWallhit.normal : WallDetection.leftWallhit.normal;
        Vector3 forceToApply = transform.forward * wallJumpForwardForce + transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        _rb.AddForce(forceToApply * wallRunForce * wallRunForceMultiplier, ForceMode.Impulse);

        StopWallRun();
        CameraTiltFinished();
    }

    public void StopWallRun() {
        _rb.useGravity = true;
        CameraTiltFinished();
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerUsingUI = true;
        InputManager.ToggleActionMap(InputManager.inputActions.UI);
    }

    private void OnExitOption()
    {
        UI_Master.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerUsingUI = false;
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
        InputManager.inputActions.Player.Enable();
        InputManager.inputActions.UI.Disable();
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
