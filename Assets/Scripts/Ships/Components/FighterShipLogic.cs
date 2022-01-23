using UnityEngine;

public class FighterShipLogic : ShipLogic
{
    public ShipListScriptableObject MothershipList;
    [SerializeField] public GameObject mothership;
    [SerializeField] private bool returning;

    private new void Start()
    {
        base.Start();
        returning = false;
        var returnToMothership = new MoveToTargetState(this, _movementController, mothership);
        StateMachine = new FSM();
        StateMachine.AddTransition(_moveToTarget, returnToMothership, HasReachedTarget);
        StateMachine.SetState(_moveToTarget);
        MothershipList = ShipSpawner.MothershipList;
    }

    private new void FixedUpdate()
    {
        StateMachine.Tick();
        if (mothership == null && MothershipList.ShipList.Count > 0)
        {
            mothership = MothershipList.ShipList[0];
        }

        if (returning && mothership != null &&
            Vector2.Distance(transform.position, mothership.transform.position) < .5f)
        {
            Destroy(gameObject);
        }
    }

    private new bool HasReachedTarget()
    {
        var shipData = ShipSpawner.ShipDictionary.GetShip(gameObject.GetInstanceID());
        if (target == null)
        {
            target = DetectionController.DetectShip(shipData.AggroRange, gameObject);
            if (target != null)
            {
                _moveToTarget.Target = target;
                _moveToTarget.OnEnter();
            }
        }

        if (target == null || Vector2.Distance(transform.position, target.transform.position) < .5f)
        {
            returning = true;
            foreach (AttackCommand command in _attackCommands)
            {
                command.StopAttack();
            }

            return true;
        }

        return false;
    }
}