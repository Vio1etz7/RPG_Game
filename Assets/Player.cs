using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMachine stateMachine;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState {  get; private set; }
    private PlayerInputSet input;
    public Vector2 moveInput { get; private set; }

    private void Awake()
    {
        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        idleState = new Player_IdleState(this,stateMachine,"idel");
        moveState = new Player_MoveState(this,stateMachine, "move");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // += 事件注册，理解：按下movement执行 moveInput = ctx.ReadValue<Vector2>()这个函数
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.UpdateActiveState();
    }
}
