using Unity.VisualScripting;
using UnityEngine;

public class Player_Grounded_State : EntityState
{
    public Player_Grounded_State(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    override public void Update()
    {
        base.Update();

        if(rb.linearVelocity.y < 0 && player.groundDetected == false )
            stateMachine.ChangeState(player.fallState);

        if (player.input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        if (player.input.Player.Attack.WasPressedThisFrame()) 
            stateMachine.ChangeState(player.basicAttackState);
    }
}
