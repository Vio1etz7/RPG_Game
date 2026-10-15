using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void CurrentStateTrigger()
    {
       //访问entity并让currentstate知道我们现在想退出state 
        entity.CurrentStateAnimationTrigger();
    }
}
