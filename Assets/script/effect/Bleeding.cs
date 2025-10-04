using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Bleeding : StatusEffect
{
    private int damagePerTurn;

    public override void Initialize(Character sourceCharacter)
    {
        base.Initialize(sourceCharacter);
        triggerTime = EffectTriggerTime.OnTurnStart;
    }

    public override void ApplyEffect(Character target)
    {
        if (source == null)
        {
            Debug.LogWarning("Bleeding: source is null");
            return;
        }

        // คำนวณดาเมจ: 50% ของ ATK ผู้ร่าย
        damagePerTurn = Mathf.CeilToInt(source.atk * 0.5f);
        Debug.Log($"{target.characterName} is bleeding! Will take {damagePerTurn} damage per turn.");
    }

    public override void OnTurnStart(Character target)
    {

        if (damagePerTurn > 0 && target.IsAlive())
        {
            target.health -= damagePerTurn;
            Debug.Log($"{target.characterName} suffers {damagePerTurn} bleeding damage.");
        }
        else
        {
            Debug.Log($"❗️No bleeding damage applied. damagePerTurn = {damagePerTurn}, IsAlive = {target.IsAlive()}");
        }
    }


    public override void RemoveEffect(Character target)
    {
        Debug.Log($"{target.characterName}'s bleeding stopped.");
    }
    
}
