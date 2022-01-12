public class MotherShipLogic : ShipLogic
{
    public ShipListScriptableObject MothershipList;
    private new void Start()
    {
        base.Start();
        StateMachine.AddTransition(_moveForward, _moveToTarget,DetectEnemy);
        StateMachine.AddTransition(_moveToTarget, _moveForward,HasReachedTarget);
        StateMachine.AddTransition(_moveToPosition, _moveForward, HasReachedPosition);
        StateMachine.SetState(_moveForward);
        
        MothershipList = ShipSpawner.MothershipList;
        MothershipList.AddShip(gameObject);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnDestroy()
    {
        if (MothershipList != null)
        {
            MothershipList.RemoveShip(gameObject);
        }
    }
}