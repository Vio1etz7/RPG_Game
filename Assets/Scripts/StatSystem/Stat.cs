using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat 
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();
    
    private bool needToRecalculate = true;
    private float finalValue;
    
    public float GetValue()
    {
        if (needToRecalculate)
        {
            finalValue = GetFinalValue();
            needToRecalculate = false;
        }

        return finalValue;
    }

    //所有的计算在这里做   
    //buff/items会影响这个basevalue
    //eg：药水，buff，debuff，装备等

    public void AddModifier(float value,string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        modifiers.Add(modToAdd);
        needToRecalculate = true;
    }

    public void RemoveModifier(string source) 
    {
        modifiers.RemoveAll(modifiers => modifiers.source == source);
        needToRecalculate = true;
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach(var modifier in modifiers)
        {
            finalValue += modifier.value;
        }
        return finalValue;
    }

    public void SetBaseValue(float value) => baseValue = value;

}

[Serializable]
public class StatModifier
{
    public float value; // 4/5
    public string source; //buff / items

    public StatModifier(float value,string source)
    {
        this.value = value;
        this.source = source;
    }

}
