using UnityEngine;

public class IceShard : StatusEffect
{
    private float debuffMultiplier = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        triggerTime = EffectTriggerTime.OnTurnEnd; // „ÀÈπ—∫µÕπ®∫‡∑‘√Ïπ
    }
    public override void ApplyEffect(Character target)
    {
        target.atk -= Mathf.CeilToInt(target.baseatk * debuffMultiplier);
        target.Int -= Mathf.CeilToInt(target.baseInt * debuffMultiplier);
        Debug.Log($"{target.characterName} is Ice Shard! -20% to ATK/INT");
    }

    public override void RemoveEffect(Character target)
    {
        target.atk += Mathf.CeilToInt(target.baseatk * debuffMultiplier);
        target.Int += Mathf.CeilToInt(target.baseInt * debuffMultiplier);
        Debug.Log($"{target.characterName}'s Ice Shard is lifted.");
    }

}
