using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitialState : PlayerBaseState
{
    public PlayerInitialState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        Debug.Log("initial");
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchState()
    {
        if (PlayerStateManager.state == PlayerState.IDLE) {
            SwitchState(_factory.Idle());
        }
        if (PlayerStateManager.state == PlayerState.WALK || PlayerStateManager.state == PlayerState.RUN) {
            SwitchState(_factory.Walk());
        }
        if (_ctx.jumpAction.WasPerformedThisFrame()) 
        {
            if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && 
                PlayerStateManager.checkGroundState(GroundState.ONGROUND)) 
            {
                SwitchState(_factory.Jump());
            }
        }
        if (_ctx.crouchAction.IsPressed()) 
        {
            SwitchState(_factory.Crouch());
        } 
        if (!PlayerStateManager.checkGroundState(GroundState.ONGROUND))
        {
            _ctx.SetMaxVelocity(20f);
        }
        // if (PlayerStateManager.checkWallRunState(WallRunState.ABLE)) 
        // {
        //     SwitchState(_factory.WallRun());
        // }
        // if (PlayerStateManager.checkWallRunState(WallRunState.ABLE)) 
        // {
        //     DoCameraTilt();
        //     if (_ctx.jumpAction.WasPerformedThisFrame())
        //     {
        //         PlayerStateManager.UpdatePlayerState(PlayerState.WALLRUNNING);
        //     }
        // }
        // else if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && _ctx.tilt != 0)
        // {
        //     StopCameraTilt();
        // }
        // if (PlayerStateManager.checkPlayerState(PlayerState.IDLE) && PlayerStateManager.checkJumpState(JumpState.NOT_JUMPING))
        // {
        //     if (LiftStateManager.liftState == LiftStateManager.LiftState.NONE)
        //     {
        //         if (_ctx._rb.useGravity == false)
        //         {
        //             _ctx._rb.useGravity = true;
        //         }
        //     }
        // }
        // if (PlayerStateManager.CanWallJump && _ctx.jumpAction.WasPerformedThisFrame()) {
        //     DoWallJump();
        // }
        // if (PlayerStateManager.WRState == WallRunState.UNABLE && 
        //     WallDetection.state == WallState.NOWALL &&
        //     PlayerStateManager.state == PlayerState.WALLRUNNING)
        // {
        //     DoWallJump();
        // }
    }
}
