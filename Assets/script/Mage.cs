using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using static Unity.Cinemachine.CinemachineTargetGroup;
using UnityEngine.TextCore.Text;
using UnityEngine.SocialPlatforms.Impl;

public class Mage : Character
{
    TurnManager turnManager;
    private int ManaShieldTurnCount = 0;

    private enum SelectedAction { None, Attack, Fireball, IceShard, ManaShield, ArcaneBurst }
    private SelectedAction currentAction = SelectedAction.None;

    public bool isAIControlled = false;
    public bool useWeightScoreAI = false;
    private CameraSwitcher cameraSwitcher;

    public AudioClip soundFireball;
    public AudioClip soundIceShard;

    public Image Shieldbar;
    CharacterStats mageStats;

    protected override void Awake()
    {
        base.Awake();

        mageStats = PlayerDataManager.GetCharacterStats("Mage");

        Click = GetComponent<ClickSelector>();
        turnManager = FindFirstObjectByType<TurnManager>();
        cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene!");
        }
        // เซ็ตค่าพลัง
        health = mageStats.currentHealth;
        if (health <= 0) 
        {
            health = Mathf.CeilToInt((float)mageStats.baseHealth * 0.10f);
        }
        fullhealth = mageStats.baseHealth;
        magicpoint = mageStats.currentMagicPoint;
        fullmagicpoint = mageStats.baseMagicPoint;
        atk = mageStats.currentAttack;
        baseatk = mageStats.baseAttack;
        Int = mageStats.currentIntelligence;
        baseInt = mageStats.baseIntelligence;
        def = mageStats.currentDefense;
        basedef = mageStats.baseDefense;
        res = mageStats.currentResistance;
        baseres = mageStats.baseResistance;
        speed = mageStats.currentSpeed;
        basespeed = mageStats.baseSpeed;
        luck = mageStats.currentLuck;
        isAIControlled = mageStats.rulebase;
        useWeightScoreAI = mageStats.weightscore;

        weaknesses = new List<string> { "none" };
    }


    public override void TakeTurn()
    {
        base.TakeTurn();
        endaction();
        showplayer(2);
        cameraSwitcher.SwitchCamera(cameraSwitcher.mageVcam);

        weaknesses = new List<string> { "none" };

        var status = TurnManager.Instance.statusEffectManager;
        if (status != null)
        {
            bool alive = status.ApplyStartTurnEffects(this);
            if (!alive)
            {
                TurnManager.Instance.NextTurn(); // ข้ามเทิร์นทันที
                return;
            }
        }

        Click.target = null;

        if (turnManager != null)
            turnManager.currentCharacter = this;

        currentAction = SelectedAction.None;
        Click.target = null;

        if (ManaShieldTurnCount > 0)
        {
            ManaShieldTurnCount--;
            if (ManaShieldTurnCount == 0)
            {
                shield = false;
                shieldpoint = 0;
            }
        }
        if (isAIControlled)
        {
            StartCoroutine(AutoDecideAction());
        }
        else if (useWeightScoreAI)
        {
            StartCoroutine(AutoDecideActionByWeight());
        }
        else
        {
            mainCanvas.SetActive(true);
            mainaction.SetActive(true);
        }

    }


    public void Attack()
    {
        currentAction = SelectedAction.Attack; endaction(); Click.showOutline = true;
    }
    public void UseSkill()
    {
        Skillcanvas.SetActive(true);
        mainaction.SetActive(false);
        itemcanvas.SetActive(false);
    }
    public void UseItem()
    {
        if (useitem)
        {
            itemcanvas.SetActive(true);
            mainaction.SetActive(false);
            Skillcanvas.SetActive(false);
        }
    }

    public void UsePhoenixfeather()
    {
        if (phoenixfeather >= 1)
        {
            currentItem = SelectedItem.phoenix;
            endaction();
            Click.showOutline = true;
        }
    }
    public void UseHPPotion()
    {
        if (HPPotion >= 1)
        {
            currentItem = SelectedItem.HPPo;
            endaction();
            Click.showOutline = true;
        }
    }
    public void UseFullHPPotion()
    {
        if (FullHPPotion >= 1)
        {
            currentItem = SelectedItem.FullHPPo;
            endaction();
            Click.showOutline = true;
        }

    }
    public void UseManaPotion()
    {
        if (ManaPotion >= 1)
        {
            currentItem = SelectedItem.ManaPo;
            endaction();
            Click.showOutline = true;
        }
    }
    public void UseFullManaPotion()
    {
        if (FullManaPotion >= 1)
        {
            currentItem = SelectedItem.FullManaPo;
            endaction();
            Click.showOutline = true;
        }
    }
    public void Fireball()
    {
        if (magicpoint >= 25)
        {
            currentAction = SelectedAction.Fireball; endaction(); Click.showOutline = true;
        }
    }
    public void IceShard()
    {
        if (magicpoint >= 20)
        {
            currentAction = SelectedAction.IceShard; endaction(); Click.showOutline = true;
        }
    }
    public void ManaShield()
    {
        if (magicpoint >= 30)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            currentAction = SelectedAction.ManaShield; endaction(); Click.showOutline = true;
        }
    }
    public void ArcaneBurst()
    {
        if (magicpoint >= 35)
        {
            currentAction = SelectedAction.ArcaneBurst; endaction(); Click.showOutline = true;
        }
    }

    public void CancelSkill()
    {
        showplayer(2);
        cameraSwitcher.SwitchCamera(cameraSwitcher.mageVcam);
        Click.showOutline = false;
        currentAction = SelectedAction.None;
        currentItem = SelectedItem.None;
        mainCanvas.SetActive(true);
        mainaction.SetActive(true);
        Skillcanvas.SetActive(false);
        itemcanvas.SetActive(false);
        Click.target = null;
        Target = null;
    }

    protected override void Update()
    {
        savestat(mageStats);
        base.Update();
        textnumHP.text = health.ToString();
        textnumMP.text = magicpoint.ToString();
        hpbar.fillAmount = (float)health / fullhealth;
        mpbar.fillAmount = (float)magicpoint / fullmagicpoint;
        Shieldbar.fillAmount = (float)shieldpoint / fullhealth;

        if (currentAction != SelectedAction.None)
        {
            switch (currentAction)
            {
                case SelectedAction.Attack:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint = Mathf.Min(fullmagicpoint, magicpoint + 12);
                        Target = Click.target.GetComponentInParent<Character>();
                        pendingDamage = Int * 0.7f;
                        isPhysicalAttack = false;
                        effect = "fire";
                        element = "Fire";
                        animatorname = "attack";
                        soundattack = soundFireball;
                        animatoinplay = 1;
                        animator.SetTrigger(animatorname);
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.Fireball:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 25;
                        Target = Click.target.GetComponentInParent<Character>();
                        pendingDamage = Int * 1.8f;
                        isPhysicalAttack = false;
                        effect = "fire";
                        element = "Fire";
                        animatorname = "attack";
                        soundattack = soundFireball;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 1;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.IceShard:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 20;
                        Target = Click.target.GetComponentInParent<Character>();
                        var IceShardEffectPrefab = Resources.Load<IceShard>("StatusEffects/IceShard");
                        TurnManager.Instance.statusEffectManager.AddEffect(Target, IceShardEffectPrefab);
                        pendingDamage = Int * 1.2f;
                        isPhysicalAttack = false;
                        effect = "ice";
                        element = "Ice";
                        animatorname = "IceShard";
                        soundattack = soundIceShard;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 1;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.ManaShield:
                    if (Click.target != null && (Click.target.name == "Mage"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 30;
                        shield = true;
                        shieldpoint = Mathf.CeilToInt(fullhealth * 0.5f);
                        ManaShieldTurnCount = 2;
                        animatorname = "ManaShield";
                        soundattack = soundIceShard;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.ArcaneBurst:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 35;
                        Target = Click.target.GetComponentInParent<Character>();
                        pendingDamage = Int * 1f;
                        isPhysicalAttack = false;
                        element = "Fire";
                        animatorname = "ArcaneBurst";
                        soundattack = soundFireball;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 2;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;
            }
        }

        if (currentItem != SelectedItem.None)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            switch (currentItem)
            {
                case SelectedItem.HPPo:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        if (HPPotion >= 1 && isuseitem != true)
                        {
                            Target = Click.target.GetComponentInParent<Character>();
                            isuseitem = true;
                            Click.showOutline = false;
                            StartCoroutine(HPPo(Target));
                        }
                    }
                    break;
                case SelectedItem.FullHPPo:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        if (FullHPPotion >= 1 && isuseitem != true)
                        {
                            Target = Click.target.GetComponentInParent<Character>();
                            isuseitem = true;
                            Click.showOutline = false;
                            StartCoroutine(FullHPPo(Target));
                        }
                    }
                    break;
                case SelectedItem.ManaPo:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        if (ManaPotion >= 1 && isuseitem != true)
                        {
                            Target = Click.target.GetComponentInParent<Character>();
                            isuseitem = true;
                            Click.showOutline = false;
                            StartCoroutine(ManaPo(Target));
                        }
                    }
                    break;
                case SelectedItem.FullManaPo:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        if (FullManaPotion >= 1 && isuseitem != true)
                        {
                            Target = Click.target.GetComponentInParent<Character>();
                            isuseitem = true;
                            Click.showOutline = false;
                            StartCoroutine(FullManaPo(Target));
                        }
                    }
                    break;
                case SelectedItem.phoenix:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        if (phoenixfeather >= 1 && isuseitem != true)
                        {
                            Target = Click.target.GetComponentInParent<Character>();
                            if (!Target.IsAlive())
                            {
                                isuseitem = true;
                                Click.showOutline = false;
                                StartCoroutine(phoenix(Target));
                            }
                        }
                    }
                    break;
            }
            if (!useitem)
            {
                afteritem();
            }
        }
    }

    private void endaction()
    {
        itemcanvas.SetActive(false);
        Skillcanvas.SetActive(false);
        mainaction.SetActive(false);
    }
    public void afteritem()
    {
        showplayer(2);
        isuseitem = false;
        Click.target = null;
        cameraSwitcher.SwitchCamera(cameraSwitcher.mageVcam);
        currentItem = SelectedItem.None;
        if (!isAIControlled)
        {
            mainaction.SetActive(true);
        }
    }

    private bool findweak(string name)
    {
        Character[] all = FindObjectsByType<Character>(FindObjectsSortMode.None);
        List<Character> players = new List<Character>();

        foreach (Character c in all)
        {
            if (c.isEnemy && c.IsAlive())
            {
                if (c.weaknesses.Contains(name))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Character findenemy(string name)
    {
        var enemies = turnManager.enemyteam.Where(e => e.IsAlive()).ToList();
        Character[] all = FindObjectsByType<Character>(FindObjectsSortMode.None);
        List<Character> players = new List<Character>();

        foreach (Character c in all)
        {
            if (c.isEnemy && c.IsAlive())
            {
                if (c.weaknesses.Contains(name))
                {
                    return c;
                }
            }
        }
        return enemies.OrderBy(e => e.GetHealthLevel10()).First(); ;
    }

    private IEnumerator AutoDecideAction()
    {
        yield return new WaitForSeconds(1f); // หน่วงเวลาเล็กน้อยให้ดูเป็นธรรมชาติ

        var enemies = turnManager.enemyteam.Where(e => e.IsAlive()).ToList();
        var fallenAlly = turnManager.playerteam.FirstOrDefault(a => !a.IsAlive());
        // ✅ Rule 0: ชุบเพื่อนถ้าตาย

        if (phoenixfeather > 0 && fallenAlly != null)
        {
            Click.target = fallenAlly.gameObject;
            UsePhoenixfeather();

            Character name = fallenAlly.gameObject.GetComponentInParent<Character>();
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Phoenixfeather to {name.characterName}");
            textai.text = PlayerDataManager.getaiaction();
        }

        // ✅ Rule 0.1: 
        yield return StartCoroutine(TryUseHealingItem(0.5f));

        while (isuseitem)
        {
            yield return null;
        }

        // ✅ Rule 1: 
        if (magicpoint >= 30 && !shield && health < fullhealth * 0.4f)
        {
            Click.target = this.gameObject;
            ManaShield();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use ManaShield");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ป้องกัน enemies ว่าง
        if (enemies == null || enemies.Count == 0)
        {
            // ไม่มีศัตรูให้โจมตี — จบเทิร์น
            yield break;
        }
        if (findweak("Fire") && magicpoint >= 25)
        {
            var target = findenemy("Fire");
            Click.target = target.gameObject;
            Fireball();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Fireball");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }
        if (findweak("Ice") && magicpoint >= 20) 
        {
            var target = findenemy("Ice");
            Click.target = target.gameObject;
            IceShard();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use IceShard");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }
        // ✅ Rule 2: 
        if (magicpoint >= 35 && enemies.Count >= 2 && Random.value < 0.7f)
        {
            var target = enemies.OrderBy(e => e.GetHealthLevel10()).First();
            Click.target = target.gameObject;
            ArcaneBurst();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use ArcaneBurst");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ✅ Rule 3: 
        if (magicpoint >= 25 && Random.value < 0.7f)
        {
            var target = findenemy("Fire");
            Click.target = target.gameObject;
            Fireball();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Fireball");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ✅ Rule 4: 
        // ใส่วงเล็บเพื่อความถูกต้องของลอจิก
        if ((magicpoint >= 20 && Random.value < 0.8f) || magicpoint >= 60)
        {
            var target = findenemy("Ice");
            Click.target = target.gameObject;
            IceShard();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use IceShard");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ✅ Rule 5: 
        {
            var target = findenemy("Fire");
            Click.target = target.gameObject;
            Attack();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Attack");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }
    }

    private float GetInverseHP(float hp, float maxHP)
    {
        return 1f - (hp / maxHP);
    }

    private float GetManaShieldScore()
    {
        float score = GetInverseHP(health, fullhealth) * 1.3f;
        if (ManaShieldTurnCount == 0) score += 0.8f;
        if (magicpoint >= 30) score += 0.5f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 30|| ManaShieldTurnCount != 0) score *= -1;
        return score;
    }

    private float GetArcaneBurstScore(List<Character> enemies)
    {
        float score = (enemies.Count >= 3 ? 2.0f : 0f);
        if (magicpoint >= 35) score += 0.5f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 35) score *= -1;
        return score;
    }

    private float GetFireballScore(List<Character> enemies)
    {
        float score = (magicpoint > 25 ? 0.5f : 0f);
        if (findweak("Fire")) score += 1.5f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 25) score *= -1;
        return score;
    }

    private float GetIceShardScore()
    {
        float score = (magicpoint >= 20 ? 0.5f : 0f);
        score += Random.Range(0f, 0.3f);
        if (findweak("Ice")) score += 1.5f;
        if (magicpoint < 20) score *= -1;
        return score;
    }

    private float GetBasicAttackScore(List<Character> enemies)
    {
        int minLevel = enemies.Min(e => e.GetHealthLevel10());
        float inverseLevel = (11 - minLevel) / 10f;
        return inverseLevel + 0.2f ;
    }

    private IEnumerator AutoDecideActionByWeight()
    {
        yield return new WaitForSeconds(1f);

        var enemies = turnManager.enemyteam.Where(e => e.IsAlive()).ToList();
        var allies = turnManager.playerteam.Where(a => a.IsAlive()).ToList();
        var fallenAlly = turnManager.playerteam.FirstOrDefault(a => !a.IsAlive());

        if (phoenixfeather > 0 && fallenAlly != null)
        {
            Click.target = fallenAlly.gameObject;
            UsePhoenixfeather();

            Character name = fallenAlly.gameObject.GetComponentInParent<Character>();
            PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Phoenixfeather to {name.characterName}");
            textai.text = PlayerDataManager.getaiaction();
        }

        yield return StartCoroutine(TryUseHealingItem(0.5f));
        while (isuseitem) yield return null;

        Dictionary<string, float> skillScores = new Dictionary<string, float>
{
    { "ManaShield",  GetManaShieldScore() },
    { "ArcaneBurst", GetArcaneBurstScore(enemies) },
    { "Fireball",    GetFireballScore(enemies) },
    { "IceShard",    GetIceShardScore() },
    { "Attack",      GetBasicAttackScore(enemies) }
};

        var bestSkill = skillScores.OrderByDescending(kv => kv.Value).First().Key;
        Character lowestEnemy = findenemy("Fire");

        switch (bestSkill)
        {
            case "ManaShield":
                Click.target = this.gameObject;
                ManaShield();
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use ManaShield");
                textai.text = PlayerDataManager.getaiaction();
                break;

            case "ArcaneBurst":
                Click.target = lowestEnemy.gameObject;
                ArcaneBurst();
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use ArcaneBurst");
                textai.text = PlayerDataManager.getaiaction();
                break;

            case "Fireball":
                Click.target = lowestEnemy.gameObject;
                Fireball();
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Fireball");
                textai.text = PlayerDataManager.getaiaction();
                break;

            case "IceShard":
                Click.target = findenemy("Ice").gameObject;
                IceShard();
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use IceShard");
                textai.text = PlayerDataManager.getaiaction();
                break;

            case "Attack":
            default:
                Click.target = lowestEnemy.gameObject;
                Attack();
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Attack");
                textai.text = PlayerDataManager.getaiaction();
                break;
        }

    }
}
