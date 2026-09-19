using UnityEngine;

//所有状态的父类
public abstract class EntityState 
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string stateName;


    public EntityState(Player player,StateMachine stateMachine,string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        //每次切换状态时调用，用作初始化
        Debug.Log("I entered " + stateName);
    }

    public virtual void Update()
    {
        //
        Debug.Log("I run update of " + stateName);
    }

    public virtual void Exit()
    {
        //每次退出当前状态时调用
        Debug.Log("I exit " + stateName);
    }
}
