using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }

    public void CurrentStateTrigger()
    {
       //访问entity并让currentstate知道我们现在想退出state 
        entity.CurrentStateAnimationTrigger();
    }

    public void AttackTrigger()
    {
        entityCombat.PerforAttack();
    }
}
