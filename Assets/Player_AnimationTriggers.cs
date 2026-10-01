using UnityEngine;

public class Player_AnimationTriggers : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void CurrentStateTrigger()
    {
       //访问player并让currentstate知道我们现在想退出state
        player.CallAnimationTrigger();
    }
}
