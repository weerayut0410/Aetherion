using UnityEngine;

public class Curse : StatusEffect
{
    private float debuffMultiplier = 0.2f;

    private void Awake()
    {
        triggerTime = EffectTriggerTime.OnTurnEnd; // „ÀÈπ—∫µÕπ®∫‡∑‘√Ïπ
    }
    public override void ApplyEffect(Character target)
    {
        target.atk -= Mathf.CeilToInt(target.baseatk * debuffMultiplier);
        target.Int -= Mathf.CeilToInt(target.baseInt * debuffMultiplier);
        target.def -= Mathf.CeilToInt(target.basedef * debuffMultiplier);
        target.res -= Mathf.CeilToInt(target.baseres * debuffMultiplier);
        Debug.Log($"{target.characterName} is cursed! -20% to ATK/INT/DEF/RES");
    }

    public override void RemoveEffect(Character target)
    {
        target.atk += Mathf.CeilToInt(target.baseatk * debuffMultiplier);
        target.Int += Mathf.CeilToInt(target.baseInt * debuffMultiplier);
        target.def += Mathf.CeilToInt(target.basedef * debuffMultiplier);
        target.res += Mathf.CeilToInt(target.baseres * debuffMultiplier);
        Debug.Log($"{target.characterName}'s Curse is lifted.");
    }

}