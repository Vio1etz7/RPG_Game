using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;

    public Stat_ResourceGroup resources;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;
    public Stat_MajorGroup major;

    public float GetElementalDamage(out ElementType elementType, float scaleFactor = 1f)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        float bonusElementalDamage = major.intelligence.GetValue() * 1f; //每一点智力(intelligence)增加1点元素伤害

        float highestElementalDamage = fireDamage;
        elementType = ElementType.Fire;

        if (iceDamage > highestElementalDamage)
        {
            highestElementalDamage = iceDamage;
            elementType = ElementType.Ice;
        }

        if (lightningDamage > highestElementalDamage)
        {
            highestElementalDamage = lightningDamage;
            elementType = ElementType.Lightning;
        }

        if (highestElementalDamage <= 0f)
        {
            elementType = ElementType.None; //没有元素伤害时，元素类型为None
            return 0f; //如果没有任何元素伤害，直接返回0(防止没有元素伤害，但有intelligence加成的情况)
        }

        float bonusFire = (elementType == ElementType.Fire) ? 0 : fireDamage * 0.5f;
        float bonusIce = (elementType == ElementType.Ice) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (elementType == ElementType.Lightning) ? 0 : lightningDamage * 0.5f;

        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning;
        float finalDamage = highestElementalDamage + weakerElementalDamage + bonusElementalDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0f;
        float bonusResistance = major.intelligence.GetValue() * .5f; //每一点智力(intelligence)增加0.5%的元素抗性

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
            default:
                return 0f; //如果没有元素类型，返回0
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f; //元素抗性上限为75%
        float finalResistance = Mathf.Clamp(resistance, 0f, resistanceCap) / 100; //将最终抗性转换为一个乘数，eg： 50 / 100 = 0.5，表示可以抵消50%的元素伤害

        return finalResistance;
    }



    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1f)
    {
        //计算基础伤害 = 基础伤害 + 力量加成
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue() * 1; //每一点力量增加1点物理伤害
        float totalBaseDamage = baseDamage + bonusDamage;

        //计算暴击率 = 基础暴击率 + 敏捷加成
        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f; //每一点敏捷增加0.3%的暴击率
        float totalCritChance = baseCritChance + bonusCritChance;

        //计算暴击伤害（一个乘数） = 基础暴击伤害 + 力量加成   
        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f; //每一点力量增加0.5%的暴击伤害
        float totalCritPower = (baseCritPower + bonusCritPower) / 100f; //总暴击伤害作为一个乘数直接*totalBaseDamage （eg： 150 /100 = 1.5）  

        //判断是否暴击
        isCrit = UnityEngine.Random.Range(0f, 100f) < totalCritChance;


        float finalDamage = isCrit ? totalBaseDamage * totalCritPower : totalBaseDamage;
        return finalDamage * scaleFactor;
    }


    //护甲越高减伤效果越差，避免护甲无限堆叠，使攻击无效
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue() * 1f; //每一点vitality增加1点护甲
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Math.Clamp(1f - armorReduction, 0f, 1f); //计算护甲减伤的乘数
        float effectiveArmor = totalArmor * reductionMultiplier; //计算有效护甲值


        float mitigation = effectiveArmor / (effectiveArmor + 100f); //护甲减伤公式
        float mitigationCap = .85f; //减伤上限为85%

        float finalMitigation = Mathf.Clamp(mitigation, 0f, mitigationCap);
        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100;  //这也是一个乘数，eg： 20 / 100 = 0.2，表示可以无视20%的护甲

        return finalReduction;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f;  //每一点敏捷增加0.5%的闪避率

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 75f; //闪避率上限为75%

        float finalEvasion = Mathf.Clamp(totalEvasion, 0f, evasionCap);

        return finalEvasion;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = resources.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritPower: return offense.critPower;
            case StatType.CritChance: return offense.critChance;
            case StatType.ArmorReduction: return offense.armorReduction;


            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.FireResistance: return defense.fireRes;
            case StatType.IceResistance: return defense.iceRes;
            case StatType.LightningResistance: return defense.lightningRes;

            default:
                Debug.LogWarning($"StatType{type} not implemented yet");
                return null;
        }
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if(defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned!");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
    }

}
