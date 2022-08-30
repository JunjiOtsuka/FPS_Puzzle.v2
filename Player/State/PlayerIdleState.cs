using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("idle");
        //play idle animation
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
        if (PlayerStateManager.state != PlayerState.IDLE) {
            SwitchState(_factory.Initial());
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

}
