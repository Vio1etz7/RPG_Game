using UnityEngine;

public class Enemy_GroundedState : EnemyState
{


    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        //Èç¹û¼ì²âµ½player£¬¾ÍÇÐ»»µ½¹¥»÷×´Ì¬£¨battleState£©
        if(enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.battleState);
    }

   
}
