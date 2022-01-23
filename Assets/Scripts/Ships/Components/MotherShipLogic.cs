public class MotherShipLogic : ShipLogic
{
    private void Start()
    {
        StateMachine.AddTransition(_moveForward, _moveToTarget, DetectEnemy);
        StateMachine.AddTransition(_moveToTarget, _moveForward, HasReachedTarget);
        StateMachine.AddTransition(_moveToPosition, _moveForward, HasReachedPosition);
        StateMachine.SetState(_moveForward);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}