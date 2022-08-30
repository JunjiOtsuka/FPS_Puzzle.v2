public class PlayerStateFactory
{
    PlayerStateMachine _context;
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Initial(){
        return new PlayerInitialState(_context, this);
    }
    public PlayerBaseState Idle(){
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Walk(){
        return new PlayerWalkState(_context, this);
    }
    public PlayerBaseState Crouch(){
        return new PlayerCrouchState(_context, this);
    }
    public PlayerBaseState WallRun(){
        return new PlayerWallRunState(_context, this);
    }
    public PlayerBaseState WallJump(){
        return new PlayerWallJumpState(_context, this);
    }
    public PlayerBaseState Jump(){
        return new PlayerJumpState(_context, this);
    }
    // public PlayerBaseState Grounded(){
    //     return new PlayerGroundedState(_context, this);
    // }
}
