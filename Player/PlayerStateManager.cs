using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerMovementV2 playerMovement;
    public Rigidbody _rb;

    public Camera MC;
    public GroundDetector GD;
    public WallDetection WD;
    public InteractDetector ID;
    public GrappleDetector GrD;

    //All States
    public static PlayerState state = PlayerState.IDLE;
    public static JumpState JumpState;
    public static WallState wallState;
    public static WallRunState WRState;
    public static GroundState groundState;

    //Wallrun Conditions
    public bool AboveWRThreshold;
    public bool ByWall;
    public bool CanWallRun;
    public static bool CanWallJump;
    public bool CanWallClimb;
    public static bool IsWallRunning = false;

    public static bool wallRunning;
    bool wallStick;
    bool isInteracting;
    bool zipJump;

    void Awake() 
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Detector
        if (GD.isGrounded) 
        {
            groundState = GroundState.ONGROUND;
        } 
        else if (!GD.isGrounded) 
        {
            groundState = GroundState.INAIR;
        }

        //Wall Detector
        if (!WallDetection.rightWall && !WallDetection.leftWall && !WallDetection.backWall && !WallDetection.frontWall) 
        {
            wallState = WallState.NOWALL;
        }
        if (WallDetection.rightWall) 
        {
            wallState = WallState.RIGHTWALL;
        }
        if (WallDetection.leftWall) 
        {
            wallState = WallState.LEFTWALL;
        }
        if (WallDetection.frontWall) 
        {
            wallState = WallState.FRONTWALL;
        }
        if (WallDetection.backWall) 
        {
            wallState = WallState.BACKWALL;
        }

        float horizontalInput = PlayerMovementV2.movement.ReadValue<Vector2>().x;
        float verticalInput = PlayerMovementV2.movement.ReadValue<Vector2>().y;

        if (GD.isGrounded) {
            if (horizontalInput == 0 || verticalInput == 0) 
            {
                UpdatePlayerState(PlayerState.IDLE);
            }

            if ((horizontalInput != 0 || verticalInput != 0)) 
            {
                if (PlayerMovementV2.runAction.IsPressed()) 
                {
                    UpdatePlayerState(PlayerState.RUN);
                } 
                else 
                {
                    UpdatePlayerState(PlayerState.WALK);
                }
            }

            if (PlayerMovementV2.jumpAction.WasPerformedThisFrame()) 
            {
                UpdatePlayerState(PlayerState.JUMP);
                UpdateJumpState(JumpState.NORMALJUMP);
            }

            if (PlayerMovementV2.crouchAction.IsPressed()) 
            {
                UpdatePlayerState(PlayerState.CROUCH);
            }
        }

        //Wallrun Condition;
        if (ByWall && AboveWRThreshold) 
        {
            WRState = WallRunState.ABLE;
            CanWallRun = true;
        }
        if (!ByWall || !AboveWRThreshold) 
        {
            WRState = WallRunState.UNABLE;
            CanWallRun = false;
        }

        //WallJump condition
        if (checkPlayerState(PlayerState.WALLRUNNING))
            CanWallJump = true;
        if (!checkPlayerState(PlayerState.WALLRUNNING))
            CanWallJump = false;

        //Crouch condition
        if (checkPlayerState(PlayerState.CROUCH)) {
            if (PlayerMovementV2.jumpAction.WasPerformedThisFrame()) {
                UpdateJumpState(JumpState.CROUCHJUMP);
            }
        }

        if (CanWallRun) {
            if (PlayerMovementV2.jumpAction.WasPerformedThisFrame()) {
                UpdatePlayerState(PlayerState.WALLRUNNING);
            }
        }

        if (GrapplingHook.GrapplingState == GrapplingState.GRAPPLING) {
            UpdatePlayerState(PlayerState.GRAPPLING);
        } 

        // if (!GD.isGrounded) {
            // if (!wallStick && !wallRunning && !WD.rightWall && !WD.leftWall && !WD.frontWall && !WD.backWall && !isInteracting || state == PlayerState.JUMP) {
            //     UpdatePlayerState(PlayerState.MIDAIR);
            // }

            // if (PlayerMovementV2.jumpAction.WasPerformedThisFrame()) {
            //     if(PlayerStateManager.WRState == WallRunState.ABLE) { 
            //         UpdatePlayerState(PlayerState.WALLRUNNING);
            //     }

                // if (WD.frontWall) {
                //     UpdatePlayerState(PlayerState.wallStick);
                // }

                // if (wallRunning) {
                //     UpdatePlayerState(PlayerState.wallJump);
            //     } 
            // }
        // }

        // if (state == PlayerState.zipline && Input.GetButtonDown("Jump")) {
        //     state = PlayerState.zipJump;
        // }

        // if (wallStick) {
        //     if (verticalInput != 0) {
        //         state = PlayerState.wallClimb;
        //     }
        //     if (verticalInput == 0 || !WD.frontWall) {
        //         state = PlayerState.wallStick;
        //     }
        //     if (WD.rightWall || WD.leftWall || WD.backWall) {
        //         if (Input.GetButtonDown("Jump")) {
        //             state = PlayerState.wallJump;
        //         }
        //     }
        //     if (!WD.rightWall && !WD.leftWall && !WD.backWall && !WD.frontWall) {
        //         state = PlayerState.MIDAIR;
        //     }
        //     if (GD.isGrounded){
        //         state = PlayerState.IDLE;
        //     }
        // } 

        switch (groundState) 
        {
            case GroundState.ONGROUND:
            {
                AboveWRThreshold = false;
                break;
            }
            case GroundState.INAIR:
            {
                AboveWRThreshold = true;
                break;
            }
        }

        switch (wallState) 
        {
            case WallState.NOWALL:
            {
                ByWall = false;
                CanWallClimb = false;
                break;
            }
            case WallState.RIGHTWALL:
            {
                ByWall = true;
                CanWallClimb = false;
                break;
            }
            case WallState.LEFTWALL:
            {
                ByWall = true;
                CanWallClimb = false;
                break;
            }
            case WallState.FRONTWALL:
            {
                ByWall = false;
                CanWallClimb = true;
                break;
            }
            case WallState.BACKWALL:
            {
                ByWall = false;
                CanWallClimb = false;
                break;
            }
        }

        switch (state) {
            case PlayerState.IDLE:
            {
                DoIdle();
                // executeWallStick(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.WALLSTICK:
            {
                // executeWallStick(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.WALLCLIMBING:
            {
                // executeWallClimb(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.WALLRUNNING:
            {
                if (CanWallJump && PlayerMovementV2.jumpAction.WasPerformedThisFrame()) 
                {
                    UpdatePlayerState(PlayerState.WALLJUMP);
                    UpdateJumpState(JumpState.WALLJUMP);
                }
                break;
            }
            case PlayerState.WALLJUMP:
            {
                // DoWallJump();
                UpdatePlayerState(PlayerState.WALLJUMP);
                UpdateJumpState(JumpState.WALLJUMP);
                break;
            }
            case PlayerState.wallStick:
            {
                // executeWallStick(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.wallClimb:
            {
                // executeWallClimb(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.zipline:
            {
                // executeZipline();
                break;
            }
            case PlayerState.zipJump:
            {
                // executeZipJump();
                break;
            }
            case PlayerState.GRAPPLING:
            {
                // executeGrapple();
                break;
            }
            default:
                break;
        }
    }

    void resetBool () {
        if (wallRunning) {
            wallRunning = false;
        }

        if (wallStick) {
            wallStick = false;
        }

        if (isInteracting) {
            isInteracting = false;
        }
    }

    void DoIdle() 
    {
        // _rb.velocity = new Vector3 (0f, _rb.velocity.y, 0f);
    }

    void executeIdle() {
        resetBool();

        // velocity = new Vector3 (0f, -2f, 0f);

        //this is gravity
        // velocity.y += gravity * 1 * Time.deltaTime;

        //final velocity
        // controller.Move(velocity * Time.deltaTime);
    }

    // void executeWallStick(float horizontalInput, float verticalInput) {
    //     wallStick = true;

    //     move = transform.right * horizontalInput;

    //     move.Normalize();

    //     controller.Move(move * (speed / 5) * Time.deltaTime);

    //     velocity.y = 0;

    //     controller.Move(velocity * Time.deltaTime);
    // }

    // void executeWallClimb(float horizontalInput, float verticalInput) {
    //     move = transform.right * horizontalInput;

    //     move.Normalize();

    //     controller.Move(move * (speed / 5) * Time.deltaTime);

    //     if (verticalInput > 0) {
    //         velocity.y = 7f;
    //     } else if (verticalInput < 0) {
    //         velocity.y += gravity * 3f * Time.deltaTime;
    //     }

    //     controller.Move(velocity * Time.deltaTime);
    // }

    

    public void UpdatePlayerState(PlayerState newState) {
        PlayerStateManager.state = newState;
    }

    public void UpdateJumpState(JumpState newJumpState) {
        PlayerStateManager.JumpState = newJumpState;
    }

    public static bool checkPlayerState(PlayerState newState) {
        return PlayerStateManager.state == newState;
    }

    public static bool checkJumpState(JumpState newJumpState) {
        return PlayerStateManager.JumpState == newJumpState;
    }

    public static bool checkWallState(WallState newJumpState) {
        return PlayerStateManager.wallState == newJumpState;
    }

    public static bool checkWallRunState(WallRunState newJumpState) {
        return PlayerStateManager.WRState == newJumpState;
    }

    public static bool checkGroundState(GroundState newJumpState) {
        return PlayerStateManager.groundState == newJumpState;
    }

}
