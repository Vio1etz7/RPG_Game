using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;
    private ElementType currentEffect = ElementType.None;

    [Header("Electrify effect connectionDetails")]
    [SerializeField] private GameObject LightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
    }

    public void ApplyElectrifyEffect(float duration,float damage,float charge)
    {
        float LightningResistance = entityStats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - LightningResistance);

        //累计电荷量
        currentCharge = currentCharge + finalCharge;
        //如果电荷量足够就进行雷击 LightningStrike

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifyEffect();
            return;
        }

        //如果电荷不足
        //重新开始累计电荷
        if(electrifyCo != null)
            StopCoroutine(electrifyCo);

        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllEffects();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(LightningStrikeVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(damage);
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();
    }

    public void ApplyBurnEffect(float duration, float fireDamge)
    {
        float fireResistance = entityStats.GetElementalResistance(ElementType.Fire);
        float finalDamgae   = fireDamge * (1f - fireResistance);

        StartCoroutine(BurnEffectCo(duration, finalDamgae));
    }

    public IEnumerator BurnEffectCo(float duration,float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);


        int ticksPerSecond = 2;
        int ticksCount = Mathf.RoundToInt(duration * ticksPerSecond);
    
        float damagePerTick = totalDamage / ticksCount;
        float tickInterval = 1f / ticksPerSecond;

        for(int i = 0; i < ticksCount; i++)
        {
            //减少实体的hp
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }

    public void  ApplyChillEffect (float duration,float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1f - iceResistance);

        StartCoroutine(ChillEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(slowMultiplier, duration);
        currentEffect = ElementType.Ice;
        //apply Vfx
        entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }


    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

       return currentEffect == ElementType.None ;
    }
}
