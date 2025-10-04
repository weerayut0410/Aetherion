using UnityEngine;

public enum EffectTriggerTime
{
    OnTurnStart,
    OnTurnEnd
}

public class StatusEffect : MonoBehaviour
{
    public int duration;
    public Character source;
    public EffectTriggerTime triggerTime = EffectTriggerTime.OnTurnEnd;

    public virtual void Initialize(Character sourceCharacter)
    {
        source = sourceCharacter;
    }

    public virtual void ApplyEffect(Character target) { }
    public virtual void RemoveEffect(Character target) { }
    public virtual void OnTurnStart(Character target) { }
    public virtual void OnTurnEnd(Character target) { }
}
