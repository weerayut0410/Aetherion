using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StatusEffectManager : MonoBehaviour
{
    private Dictionary<Character, List<StatusEffect>> characterEffects = new Dictionary<Character, List<StatusEffect>>();

    public void AddEffect(Character target, StatusEffect effectPrefab, Character source = null)
    {
        if (target == null || effectPrefab == null)
        {
            Debug.LogWarning("target or effectPrefab is null");
            return;
        }

        if (!characterEffects.ContainsKey(target))
            characterEffects[target] = new List<StatusEffect>();

        StatusEffect newEffect = Instantiate(effectPrefab, transform);

        // ✅ Call Initialize หากมี source
        if (source != null)
        {
            newEffect.Initialize(source);  // ต้องทำให้ StatusEffect มี method นี้
        }

        newEffect.ApplyEffect(target);
        characterEffects[target].Add(newEffect);
    }

    public bool ApplyStartTurnEffects(Character target)
    {
        OnTurnStart(target);

        if (!target.IsAlive())
        {
            Debug.Log($"{target.characterName} died from a status effect before their turn.");
            return false; // ตายแล้ว
        }

        return true; // ยังมีชีวิต
    }


    public void OnTurnStart(Character currentCharacter)
    {
        if (currentCharacter == null) return;
        if (!characterEffects.ContainsKey(currentCharacter)) return;

        var effects = characterEffects[currentCharacter];

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            var effect = effects[i];
            if (effect == null)
            {
                effects.RemoveAt(i);
                continue;
            }

            // ✅ เช็กว่าต้องทำงานตอนเริ่มเทิร์นหรือไม่
            if (effect.triggerTime == EffectTriggerTime.OnTurnStart)
            {
                effect.OnTurnStart(currentCharacter);

                effect.duration--;
                if (effect.duration <= 0)
                {
                    effect.RemoveEffect(currentCharacter);
                    effects.RemoveAt(i);
                    Destroy(effect.gameObject);
                }
            }
        }

        if (effects.Count == 0)
            characterEffects.Remove(currentCharacter);
    }

    public void OnTurnEnd(Character currentCharacter)
    {
        if (currentCharacter == null) return;
        if (!characterEffects.ContainsKey(currentCharacter)) return;

        var effects = characterEffects[currentCharacter];

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            var effect = effects[i];
            if (effect == null)
            {
                effects.RemoveAt(i);
                continue;
            }

            // ✅ เช็กว่าต้องทำงานตอนจบเทิร์นหรือไม่
            if (effect.triggerTime == EffectTriggerTime.OnTurnEnd)
            {
                effect.OnTurnEnd(currentCharacter);

                effect.duration--;
                if (effect.duration <= 0)
                {
                    effect.RemoveEffect(currentCharacter);
                    effects.RemoveAt(i);
                    Destroy(effect.gameObject);
                }
            }
        }

        if (effects.Count == 0)
            characterEffects.Remove(currentCharacter);
    }
    public bool HasDebuff(Character target)
    {
        if (target == null || !characterEffects.ContainsKey(target)) return false;

        foreach (var effect in characterEffects[target])
        {
            if (effect == null) continue;

            if (effect is Bleeding || effect is crush || effect is DarkHowl || effect is Curse)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasBuff(Character target)
    {
        if (target == null || !characterEffects.ContainsKey(target)) return false;

        foreach (var effect in characterEffects[target])
        {
            if (effect == null) continue;

            if (effect is Blessing)
            {
                return true;
            }
        }
        return false;
    }


    public void RemoveAllDebuffs(Character target)
    {

        if (target == null)
        {
            return;
        }

        if (!characterEffects.ContainsKey(target))
        {
            return;
        }

        var effects = characterEffects[target];
        var copy = effects.ToList();

        foreach (var effect in copy)
        {
            if (effect == null)
            {
                continue;
            }

            if (effect is Bleeding || effect is crush || effect is DarkHowl)
            {
                effect.RemoveEffect(target);
                effects.Remove(effect);
                Destroy(effect.gameObject);
            }
        }

        if (effects.Count == 0)
        {
            Debug.Log("🧹 Removed all effects from: " + target.characterName);   
            characterEffects.Remove(target);
        }
    }
    public bool HasBleeding(Character target)
    {
        if (target == null || !characterEffects.ContainsKey(target)) return false;

        foreach (var effect in characterEffects[target])
        {
            if (effect == null) continue;

            if (effect is Bleeding) return true;
        }
        return false;
    }
    public bool HasBlessing(Character target)
    {
        if (target == null || !characterEffects.ContainsKey(target)) return false;

        foreach (var effect in characterEffects[target])
        {
            if (effect == null) continue;

            if (effect is Blessing) return true;
        }
        return false;
    }
    public bool Hascrush(Character target)
    {
        if (target == null || !characterEffects.ContainsKey(target)) return false;

        foreach (var effect in characterEffects[target])
        {
            if (effect == null) continue;

            if (effect is crush) return true;
        }
        return false;
    }
    public bool HasDarkHowl(Character target)
    {
        if (target == null || !characterEffects.ContainsKey(target)) return false;

        foreach (var effect in characterEffects[target])
        {
            if (effect == null) continue;

            if (effect is DarkHowl) return true;
        }
        return false;
    }
    public bool HasCurse(Character target)
    {
        if (target == null || !characterEffects.ContainsKey(target)) return false;

        foreach (var effect in characterEffects[target])
        {
            if (effect == null) continue;

            if (effect is Curse) return true;
        }
        return false;
    }

}
