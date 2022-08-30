using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("hello from crouch");
        // DoStartCrouch();
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
        if (CrouchDetector.canStandUp) {
            DoStopCrouch();
        }
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
        if (!_ctx.crouchAction.IsPressed()) 
        {
            SwitchState(_factory.Initial());
        } 
    }

    private void DoStartCrouch() 
    {
        _ctx.cc.height = _ctx.cc.height/2;
        _ctx.cc.center = new Vector3 (0f, _ctx.cc.center.y / 2, 0f);
        float groundVelocity = new Vector2(_ctx._rb.velocity.x, _ctx._rb.velocity.z).magnitude;
        _ctx.mainCam.transform.position = new Vector3(_ctx.transform.position.x, _ctx.transform.position.y + 1, _ctx.transform.position.z);
        if (groundVelocity > 7) {
            _ctx.walkSpeed = _ctx.crouchSpeed; //sliding
        } else if (groundVelocity <= 7) {
            _ctx.walkSpeed = _ctx.initialSpeed * 0.75f; // crouch walking
            _ctx.SetMaxVelocity(5f);
        }
    }

    private void DoStopCrouch()
    {
        _ctx.cc.height = 3;
        _ctx.cc.center = new Vector3 (0f, _ctx.cc.center.y * 2, 0f);
        _ctx.mainCam.transform.position = new Vector3(_ctx.transform.position.x, _ctx.transform.position.y + 2, _ctx.transform.position.z);
        _ctx.walkSpeed = _ctx.initialSpeed;
    } 
}
