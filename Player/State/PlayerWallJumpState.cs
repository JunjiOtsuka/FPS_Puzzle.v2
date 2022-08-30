using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerBaseState
{
    public PlayerWallJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("wall jump");
        DoWallJump();
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
        if (PlayerStateManager.state == PlayerState.IDLE)
        {
            SetSubState(_factory.Idle());
        } 
        else if (PlayerStateManager.state != PlayerState.IDLE) {
            SwitchState(_factory.Walk());
        }
    }

    public override void CheckSwitchState()
    {
        if (PlayerStateManager.state == PlayerState.IDLE) {
            SwitchState(_factory.Idle());
        }
    }

    public void DoWallJump() 
    {
        Vector3 wallNormal = WallDetection.state == WallState.RIGHTWALL ? WallDetection.rightWallhit.normal : WallDetection.leftWallhit.normal;
        Vector3 forceToApply = _ctx.transform.forward * _ctx.wallJumpForwardForce + _ctx.transform.up * _ctx.wallJumpUpForce + wallNormal * _ctx.wallJumpSideForce;
        _ctx._rb.AddForce(forceToApply * _ctx.wallRunForceMultiplier, ForceMode.Impulse);
        StopWallRun();
    }

    public void StopWallRun() 
    {
        _ctx._rb.useGravity = true;
        PlayerStateManager.IsWallRunning = false;
        PlayerStateManager.UpdatePlayerState(PlayerState.WALLJUMP);
        PlayerStateManager.UpdateJumpState(JumpState.WALLJUMP);
        _ctx.StopCameraTilt();
    }
}
