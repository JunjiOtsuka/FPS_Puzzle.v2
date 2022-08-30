using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // public PlayerInputAction playerInputAction;
    public InputAction movement { get; set; }
    public InputAction crouchAction { get; set; }
    public InputAction runAction { get; set; }
    public InputAction jumpAction { get; set; }
    public InputAction interactClick { get; set; }
    public InputAction leftClick { get; set; }
    public InputAction rightClick { get; set; }
    public InputAction options;
    public InputAction Weapon1;
    public InputAction Weapon2;
    public PlayerStateManager playerStateManager;
    public GameObject UI_Master;

    PlayerBaseState _currentState;
    PlayerStateFactory _states;
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Rigidbody _rb { get; set; }

    [Header("Walking")]
	float runSpeed; 
	public float walkSpeed = 10;
	public float initialSpeed { get; set; }
    public Vector3 moveDirection { get; set; }

    [Header("Crouching")]
    public float crouchSpeed;
    public static float maxHeight = 3;
    public CapsuleCollider cc { get; set; }

    [Header("Jumping")]
    public float jumpSpeed = 100f;
    public bool isJumping { get; set; }

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
    public float tilt;
    public bool cameraTiltProgress = false;
    public float wallRunCameraTilt, maxWallRunCameraTilt, wallRunningCameraTiltMultiplier;
    
    [Header("Camera")]
    public float maxUpDown;
    public Camera mainCam { get; set; }
    float limitVertCameraRotation = 0;
    [Header("MouseSetting")]
    public float mouseSensitivity = 100f;
    public Vector2 mouseInput;
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
        _CurrentlyEquipped = GetComponent<CurrentlyEquipped>();

        _rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();

        //set state here
        _states = new PlayerStateFactory(this);
        _currentState = _states.Initial();
        _currentState.EnterState();

        //initialize value
        moveDirection = Vector3.zero;

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

        _currentState.UpdateState();

        if (mouseInput.x != 0 || mouseInput.y != 0) 
        {
            DoLook();
        }

        if (!PlayerStateManager.checkGroundState(GroundState.ONGROUND))
        {
            SetMaxVelocity(20f);
        }

        // if (PlayerStateManager.checkPlayerState(PlayerState.IDLE) && PlayerStateManager.checkJumpState(JumpState.NOT_JUMPING))
        // {
        //     if (LiftStateManager.liftState == LiftStateManager.LiftState.NONE)
        //     {
        //         if (_rb.useGravity == false)
        //         {
        //             _rb.useGravity = true;
        //         }
        //     }
        // }

        if (PlayerStateManager.checkWallRunState(WallRunState.ABLE)) 
        {
            DoCameraTilt();
            if (jumpAction.WasPerformedThisFrame())
            {
                PlayerStateManager.UpdatePlayerState(PlayerState.WALLRUNNING);
            }
        }
        else if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && tilt != 0)
        {
            StopCameraTilt();
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
        _currentState.FixedUpdateState();
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

    private void OnRun()
    {
        if (PlayerStateManager.state != PlayerState.CROUCH) {
            walkSpeed = runSpeed;
        }
    }

    private void DoWalk(InputAction.CallbackContext callback) 
    {
        walkSpeed = initialSpeed;
    }

    private void DoRun(InputAction.CallbackContext callback) 
    {
        
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
    
    public void SetMaxVelocity(float maxGroundVel) 
    {
        Vector3 xzVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        Vector3 yVel = new Vector3(0, _rb.velocity.y, 0);

        xzVel = Vector3.ClampMagnitude(xzVel, maxGroundVel);
        yVel = Vector3.ClampMagnitude(yVel, maxYVelocity);

        _rb.velocity = xzVel + yVel;
    }
}
