using UnityEditor;
using UnityEngine;

public class FighterShipBattleController : ShipBattleController
{
    [SerializeField] public GameObject mothership;
    [SerializeField] private bool returning;
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        returning = false;
        var returnToMothership = new MoveToTargetState(this, _movementController, mothership);
        StateMachine = new FSM();
        StateMachine.AddTransition(_moveToTarget, returnToMothership, HasReachedTarget);
        StateMachine.SetState(_moveToTarget);
    }

    // Update is called once per frame
    private new void FixedUpdate()
    {
        StateMachine.Tick();
        if (returning && mothership != null && Vector2.Distance(transform.position, mothership.transform.position) < .5f)
        {
            Death();
        }
    }

    private new bool HasReachedTarget()
    {
        if (target == null)
        {
            target = DetectionController.DetectShip(aggroRange, gameObject);
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
