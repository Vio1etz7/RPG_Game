using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player_BasicAttackState : EntityState
{
    private float attackVelocityTimer;
    private float lastAttackTime;
    
    private bool comboAttackQueued;
    private int attackDir;
    private int comboIndex = 1;
    private int comboLimit = 3 ;
    private const int FirstComboIndex = 1; //这是在动画器中使用的第一个comboindex



    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if(comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("Adjusted combo limit to match attackVelocity array!");
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();

        attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;

        anim.SetInteger("basicAttackindex", comboIndex);
        ApplyAttackVelocity();
    }

    
    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        //预输入？ 在update中检查是否在这次攻击时按下攻击键，如果按下就直接进入attackstate，没有就进入idlestate
        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if (triggerCalled)
            HandleStateExit();
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;

        //在这里使用保存当前的时间
        lastAttackTime = Time.time;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }   

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }


    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if(attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);

    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];

        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }


    private void ResetComboIndexIfNeeded()
    {
        //在这里检查时间是否 > lastAttackTime + comboResetTime
        //如果是就resetcombo
        if (Time.time > lastAttackTime + player.comboResetTime)
            comboIndex = FirstComboIndex;

        if (comboIndex > comboLimit)
            comboIndex = FirstComboIndex;
    }

}
