using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("walk");
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        CheckSwitchState();
        OnWalk();
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
        if (_ctx.jumpAction.WasPerformedThisFrame()) 
        {
            if (PlayerStateManager.checkWallRunState(WallRunState.UNABLE) && 
                PlayerStateManager.checkGroundState(GroundState.ONGROUND)) 
            {
                SwitchState(_factory.Jump());
            }
        }
    }

    private void OnWalk()
    {
        if (PlayerStateManager.state == PlayerState.WALLRUNNING) {
            return;
        }

        _ctx.moveDirection = (_ctx.transform.right * _ctx.movement.ReadValue<Vector2>().x + _ctx.transform.forward * _ctx.movement.ReadValue<Vector2>().y).normalized;
        _ctx._rb.AddForce(_ctx.moveDirection * _ctx.walkSpeed, ForceMode.Acceleration);

        if (PlayerStateManager.checkPlayerState(PlayerState.WALLRUNNING)) {
            _ctx.SetMaxVelocity(50f);
        }

        if (PlayerStateManager.state == PlayerState.RUN) {
            _ctx.SetMaxVelocity(20f);
        } else if (PlayerStateManager.state == PlayerState.WALK) {
            _ctx.SetMaxVelocity(10f);
        } 
    }
}
