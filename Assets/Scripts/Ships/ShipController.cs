using UnityEngine;
using UnityEngine.EventSystems;

public class ShipController : MonoBehaviour, IDamageable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected MovementController _movementController;
    private AttackCommand _attackCommand;
    [SerializeField] protected ProjectileAttack projectileAttack;
    [SerializeField] private bool move = true;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector2 moveTarget;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private LayerMask layerMask;
    protected FSM StateMachine;
    private void Awake()
    {
        _movementController = new MovementController(gameObject, speed, rotationSpeed, layerMask);
    }

    protected void Start()
    {
        var moving = new MoveForwardState(this, _movementController);
        StateMachine = new FSM();
        StateMachine.SetState(moving);
        ShipManager.Instance.AddShip(gameObject);
        _attackCommand = projectileAttack.MakeAttack();
        StartCoroutine(_attackCommand.DoAttack(gameObject));
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        StateMachine.Tick();

        if (move)
        {
            if (!_movementController.Move(moveTarget, speed))
            {
                move = false;
            }
        }
    }
    
    public void TakeDamage(Damage dmg)
    {
        health -= dmg.RawDamage;
        if (health == 0)
        {
            Death();
        }
    }

    protected void Death()
    {
        ShipManager.Instance.RemoveShip(gameObject);
        InputManager.Instance.DeselectShip(gameObject);
        _attackCommand.StopAttack();
        Destroy(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Hello - Mouse Enter");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPlayer)
        {
            var color = GetComponent<SpriteRenderer>().color;
            if (color.Equals(Color.white))
            {
                GetComponent<SpriteRenderer>().color = Color.cyan;
                InputManager.Instance.SelectShip(gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Hello - Mouse Exit");
    }
}
