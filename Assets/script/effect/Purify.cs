using UnityEngine;

public class Purify : StatusEffect
{
    public override void ApplyEffect(Character target)
    {
        TurnManager.Instance.statusEffectManager.RemoveAllDebuffs(target);
        Debug.Log($"{target.characterName} has been purified!");
    }

    public override void RemoveEffect(Character target) { }
}