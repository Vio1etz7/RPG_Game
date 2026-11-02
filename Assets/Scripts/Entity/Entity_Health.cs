using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour , IDamagable
{
    private Slider healthBar;
    private Entity_VFX entityVfx;
    private Entity entity;

    [SerializeField] protected float currentHp;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected bool isDead;

    [Header("On Damage Kockback")]
    [SerializeField] private Vector2 kockbackPower = new Vector2(1.5f,2.5f);
    [SerializeField] private Vector2 heveayKnockbackPower = new Vector2(7,7);
    [SerializeField] private float kockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshhold = .3f; //可以是为heavydamage的你所损失的生命值百分比，即一次性损失30%生命值被视为一次重攻击

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        healthBar = GetComponentInChildren<Slider>();

        currentHp = maxHp;
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage,Transform damageDealer)
    {
        if (isDead)
            return;

        Vector2 knockBack = CalculateKnockback(damage,damageDealer);
        float duration = CalculateDuration(damage);

        entity?.ReveiveKnockBack(knockBack, duration);
        entityVfx?.PlayOnDamageVfx();
        ReduceHp(damage);
    }
    private void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath(); 
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHp / maxHp;
    }
    public Vector2 CalculateKnockback(float damage,Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockBack = IsHeavyDamage(damage) ? heveayKnockbackPower : kockbackPower;

        knockBack.x = knockBack.x * direction;

        return knockBack;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : kockbackDuration;

    private bool IsHeavyDamage(float damage) => damage / maxHp > heavyDamageThreshhold;
}
