using UnityEngine;

public class FighterShipLogic : ShipLogic
{
    [SerializeField] public GameObject mothership;
    [SerializeField] private bool returning;

    private void Start()
    {
        returning = false;
        var returnToMothership = new MoveToTargetState(this, _movementController, mothership);
        StateMachine = new FSM();
        StateMachine.AddTransition(_moveToTarget, returnToMothership, HasReachedTarget);
        StateMachine.SetState(_moveToTarget);
    }

    private new void FixedUpdate()
    {
        StateMachine.Tick();
        if (mothership == null ||
            returning && Vector2.Distance(transform.position, mothership.transform.position) < .5f)
        {
            Destroy(gameObject);
        }
    }

    private new bool HasReachedTarget()
    {
        if (target == null)
        {
            target = DetectionController.DetectShip(_data.SensorRange, gameObject);
            if (target != null)
            {
                _moveToTarget.Target = target;
                _moveToTarget.OnEnter();
            }
        }

        if (target == null || Vector2.Distance(transform.position, target.transform.position) < .5f)
        {
            returning = true;
            foreach (var command in _attackCommands)
            {
                command.StopAttack();
            }

            return true;
        }

        return false;
    }
}
