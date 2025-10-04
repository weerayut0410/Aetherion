using UnityEngine;

public class DarkHowl : StatusEffect
{
    private float debuffMultiplier = 0.25f;

    private void Awake()
    {
        triggerTime = EffectTriggerTime.OnTurnEnd; // „ÀÈπ—∫µÕπ®∫‡∑‘√Ïπ
    }
    public override void ApplyEffect(Character target)
    {
        target.atk -= Mathf.CeilToInt(target.baseatk * debuffMultiplier);
        target.Int -= Mathf.CeilToInt(target.baseInt * debuffMultiplier);
        Debug.Log($"{target.characterName} is DarkHowl! -25% to ATK/INT");
    }

    public override void RemoveEffect(Character target)
    {
        target.atk += Mathf.CeilToInt(target.baseatk * debuffMultiplier);
        target.Int += Mathf.CeilToInt(target.baseInt * debuffMultiplier);
        Debug.Log($"{target.characterName}'s DarkHowl is lifted.");
    }
}
