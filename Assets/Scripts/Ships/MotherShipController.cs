public class MotherShipController : ShipController
{
    private new void Start()
    {
        base.Start();
        StateMachine.AddTransition(_moveForward, _moveToTarget,DetectEnemy);
        StateMachine.SetState(_moveForward);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}