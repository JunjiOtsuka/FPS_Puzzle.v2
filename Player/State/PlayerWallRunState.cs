using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunState : PlayerBaseState
{
    public PlayerWallRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        PlayerStateManager.UpdatePlayerState(PlayerState.WALLRUNNING);
    }

    public override void UpdateState()
    {
        if (PlayerStateManager.checkWallRunState(WallRunState.ABLE))
        {
            DoWallrun();
        }
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
        // if (PlayerStateManager.state == PlayerState.IDLE)
        // {
        //     SetSubState(_factory.Idle());
        // } 
        // else if (PlayerStateManager.state != PlayerState.IDLE) {
        //     SwitchState(_factory.Walk());
        // }
    }

    public override void CheckSwitchState()
    {
        if (!PlayerStateManager.checkWallRunState(WallRunState.ABLE))
        {
            SwitchState(_factory.WallJump());
        }
        if (PlayerStateManager.state == PlayerState.IDLE) {
            SwitchState(_factory.Idle());
        }
        if (PlayerStateManager.CanWallJump && _ctx.jumpAction.WasPerformedThisFrame()) {
            SwitchState(_factory.WallJump());
        }
        if (PlayerStateManager.WRState == WallRunState.UNABLE && 
            WallDetection.state == WallState.NOWALL &&
            PlayerStateManager.state == PlayerState.WALLRUNNING)
        {
            SwitchState(_factory.WallJump());
        }
    }

    public void DoWallrun() 
    {
        _ctx._rb.useGravity = false;
        _ctx._rb.velocity = new Vector3 (_ctx._rb.velocity.x, 0f, _ctx._rb.velocity.z);

        Vector3 wallNormal = WallDetection.state == WallState.RIGHTWALL ? WallDetection.rightWallhit.normal : WallDetection.leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, _ctx.transform.up);

        if ((_ctx.transform.forward - wallForward).magnitude > (_ctx.transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        _ctx._rb.AddForce(wallForward * _ctx.wallRunForce, ForceMode.Impulse);
        
        _ctx._rb.AddForce(-wallNormal * 100, ForceMode.Force);

        PlayerStateManager.IsWallRunning = true;
    }
}
