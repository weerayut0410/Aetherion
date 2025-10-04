using UnityEngine;

public class Blessing : StatusEffect
{
    private float atkMultiplier = 0.2f;
    private float intMultiplier = 0.2f;
    private int luckBonus = 10;
    private void Awake()
    {
        triggerTime = EffectTriggerTime.OnTurnEnd; // „ÀÈπ—∫µÕπ®∫‡∑‘√Ïπ
    }
    public override void ApplyEffect(Character target)
    {
        target.atk += Mathf.CeilToInt(target.baseatk * atkMultiplier);
        target.Int += Mathf.CeilToInt(target.baseInt * intMultiplier);
        target.luck += luckBonus;
        Debug.Log($"{target.characterName} receives Blessing! +20% ATK/INT, +10 LUCK");
    }

    public override void RemoveEffect(Character target)
    {
        target.atk -= Mathf.CeilToInt(target.baseatk * atkMultiplier);
        target.Int -= Mathf.CeilToInt(target.baseInt * intMultiplier);
        target.luck -= luckBonus;
        Debug.Log($"{target.characterName}'s Blessing wore off.");
    }
}