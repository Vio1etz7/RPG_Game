using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter attack details")]
    [SerializeField] private float couterRecovery = .1f;

    public bool CounterAttackPerformed()
    {
        bool hasPerformedCountered = false;

        foreach(var target in GetDetectedColliders())
        {
            ICounterable counterabale = target.GetComponent<ICounterable>();

            if (counterabale == null)
                continue;

            if (counterabale.CanBeCountered) 
            { 
                counterabale.HandleCounter();
                hasPerformedCountered = true;
            }
        }

        return hasPerformedCountered;
    }

    public float GetCounterRecoveryDuration() => couterRecovery;
    
}
