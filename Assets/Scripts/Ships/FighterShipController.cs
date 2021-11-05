using UnityEditor;
using UnityEngine;

public class FighterShipController : ShipController
{
    [SerializeField] public GameObject target;
    [SerializeField] public GameObject mothership;

    [SerializeField] private MoveToTargetState _moveToTarget;
    [SerializeField] private bool returning;
    [SerializeField] private float aggroRange;
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        returning = false;
        _moveToTarget = new MoveToTargetState(this, _movementController, target);
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

    private bool HasReachedTarget()
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
            _attackCommand.StopAttack();
            return true;
        }
        return false;
    }
}
