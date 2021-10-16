using UnityEngine;

public class FighterShipController : ShipController
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject mothership;
    [SerializeField] private bool returning;
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        returning = false;
        var moveToTarget = new MoveToTargetState(this, _movementController, target);
        var returnToMothership = new MoveToTargetState(this, _movementController, mothership);
        StateMachine = new FSM();
        StateMachine.AddTransition(moveToTarget, returnToMothership, HasReachedTarget);
        StateMachine.SetState(moveToTarget);
    }

    // Update is called once per frame
    private new void FixedUpdate()
    {
        StateMachine.Tick();
        if (returning && Vector2.Distance(transform.position, mothership.transform.position) < .5f)
        {
            Death();
        }
    }

    private bool HasReachedTarget()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < .5f)
        {
            returning = true;
            return true;
        }
        return false;
    }
}
