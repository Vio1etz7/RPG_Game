using UnityEngine;
using System.Collections;

[System.Serializable]
public class buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{

    private SpriteRenderer sr;
    private Entity_Stats statsToModify;

    [Header("Buff connectionDetails")]
    [SerializeField] private buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4f;
    [SerializeField] private bool canBeUsed = true;

    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeUsed == false)
            return;
        //启动一个coroutine来处理buff的持续时间
        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCoroutine(buffDuration));
    }

    private IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;
        ApplyBuff(true);

        
        yield return new WaitForSeconds(duration);
        
        ApplyBuff(false);
        Destroy(gameObject);
    }

    public void ApplyBuff(bool apply)
    {
        foreach(var buff in buffs)
        {
            if(apply)
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
    }

}
