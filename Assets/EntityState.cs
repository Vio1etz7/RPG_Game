using UnityEngine;

//所有状态的父类
public abstract class EntityState 
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected PlayerInputSet input;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(Player player,StateMachine stateMachine,string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName,true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        anim.SetFloat("yVelocity",rb.linearVelocity.y);

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
        
    }

    public virtual void Exit()
    {
        //每次退出当前状态时调用
        anim.SetBool(animBoolName,false);
    }

    public void CallAimationTrigger()
    {
        triggerCalled = true;
    }

    private bool CanDash()
    {
        if (player.wallDetected)
            return false;
       
        if (stateMachine.currentState == player.dashState)
            return false;
            
        return true;
    }
}
