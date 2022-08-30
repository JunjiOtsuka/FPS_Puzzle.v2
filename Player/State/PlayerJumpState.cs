using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("jump");
        DoJump();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        if (PlayerStateManager.checkGroundState(GroundState.INAIR))
        {
            _ctx.isJumping = true;  
        }
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
        //revert conditions from enter state. ex) set bool true to false;
        //leave blank if theres nothing to change
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
        if (PlayerStateManager.checkWallRunState(WallRunState.ABLE) && _ctx.jumpAction.WasPerformedThisFrame())
        {
            SwitchState(_factory.WallRun());
        }
        if (PlayerStateManager.checkGroundState(GroundState.ONGROUND) && _ctx.isJumping) {
            SwitchState(_factory.Initial());
            _ctx.isJumping = false;
        }
    }

    void DoJump()
    {
        _ctx._rb.AddForce(_ctx.transform.up * _ctx.jumpSpeed, ForceMode.Impulse);
    }
}