using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // public PlayerStateMachine _PlayerStateMachine;
    public Camera MC;

    //All States
    public static PlayerState state = PlayerState.IDLE;
    public static JumpState JumpState;
    public static WallRunState WRState;

    //Wallrun Conditions
    public static bool AboveWRThreshold;
    public static bool ByWall;
    public static bool CanWallRun;
    public static bool CanWallJump;
    public static bool CanWallClimb;
    public static bool IsWallRunning = false;

    public static bool wallRunning;
    bool wallStick;
    bool isInteracting;
    bool zipJump;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = PlayerMovementV2.movement.ReadValue<Vector2>().x;
        float verticalInput = PlayerMovementV2.movement.ReadValue<Vector2>().y;

        // float horizontalInput = _PlayerStateMachine.movement.ReadValue<Vector2>().x;
        // float verticalInput = _PlayerStateMachine.movement.ReadValue<Vector2>().y;

        if (GroundDetector.state == GroundState.ONGROUND) {
            UpdateJumpState(JumpState.NOT_JUMPING);

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
        else if (!ByWall || !AboveWRThreshold || (!ByWall && !AboveWRThreshold))
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

        switch (state) {
            case PlayerState.IDLE:
            {
                DoIdle();
                break;
            }
            case PlayerState.WALLSTICK:
            {
                // executeWallStick(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.WALLCLIMB:
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
                if (GroundDetector.state == GroundState.ONGROUND) 
                {
                    UpdatePlayerState(PlayerState.IDLE);
                    UpdateJumpState(JumpState.NOT_JUMPING);
                }
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

    public static void UpdatePlayerState(PlayerState newState) {
        PlayerStateManager.state = newState;
    }

    public static void UpdateJumpState(JumpState newJumpState) {
        PlayerStateManager.JumpState = newJumpState;
    }

    public static bool checkPlayerState(PlayerState newState) {
        return PlayerStateManager.state == newState;
    }

    public static bool checkJumpState(JumpState newJumpState) {
        return PlayerStateManager.JumpState == newJumpState;
    }

    public static bool checkWallState(WallState newJumpState) {
        return WallDetection.state == newJumpState;
    }

    public static bool checkWallRunState(WallRunState newJumpState) {
        return PlayerStateManager.WRState == newJumpState;
    }

    public static bool checkGroundState(GroundState newJumpState) {
        return GroundDetector.state == newJumpState;
    }

}
