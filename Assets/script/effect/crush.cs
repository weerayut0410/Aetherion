using UnityEngine;

public class crush : StatusEffect
{
    private float debuffMultiplier = 0.25f;

    private void Awake()
    {
        triggerTime = EffectTriggerTime.OnTurnEnd; // „ÀÈπ—∫µÕπ®∫‡∑‘√Ïπ
    }
    public override void ApplyEffect(Character target)
    {
        target.def -= Mathf.CeilToInt(target.basedef * debuffMultiplier);
        target.res -= Mathf.CeilToInt(target.baseres * debuffMultiplier);
        Debug.Log($"{target.characterName} is crush! -25% to DEF/RES");
    }

    public override void RemoveEffect(Character target)
    {
        target.def += Mathf.CeilToInt(target.basedef * debuffMultiplier);
        target.res += Mathf.CeilToInt(target.baseres * debuffMultiplier);
        Debug.Log($"{target.characterName}'s crush is lifted.");
    }
}
