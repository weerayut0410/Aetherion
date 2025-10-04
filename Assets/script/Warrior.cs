using System.Collections;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;

public class Warrior : Character
{
    TurnManager turnManager;

    private int tauntTurnCount = 0;
    private int IronGuardTurnCount = 0;

    private enum SelectedAction { None, Attack, PowerSlash, Taunt, IronGuard, Whirlwind }
    private SelectedAction currentAction = SelectedAction.None;

    public bool isAIControlled = false;
    public bool useWeightScoreAI = false;
    private CameraSwitcher cameraSwitcher;

    public AudioClip soundPowerSlash;
    public AudioClip soundWhirlwind;
    public AudioClip soundTaunt;

    CharacterStats warriorStats;

    protected override void Awake()
    {
        base.Awake();

        warriorStats = PlayerDataManager.GetCharacterStats("Warrior");

        Click = GetComponent<ClickSelector>();
        turnManager = FindFirstObjectByType<TurnManager>();
        cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene!");
        }
        originalPosition = transform.position;
        health = warriorStats.currentHealth;
        if (health <= 0)
        {
            health = Mathf.CeilToInt((float)warriorStats.baseHealth * 0.10f);
        }
        fullhealth = warriorStats.baseHealth;
        magicpoint = warriorStats.currentMagicPoint;
        fullmagicpoint = warriorStats.baseMagicPoint;
        atk = warriorStats.currentAttack;
        baseatk = warriorStats.baseAttack;
        Int = warriorStats.currentIntelligence;
        baseInt = warriorStats.baseIntelligence;
        def = warriorStats.currentDefense;
        basedef = warriorStats.baseDefense;
        res = warriorStats.currentResistance;
        baseres = warriorStats.baseResistance;
        speed = warriorStats.currentSpeed;
        basespeed = warriorStats.baseSpeed;
        luck = warriorStats.currentLuck;
        isAIControlled = warriorStats.rulebase;
        useWeightScoreAI = warriorStats.weightscore;

        weaknesses = new List<string> { "none" };
    }


    public override void TakeTurn()
    {
        base.TakeTurn();
        endaction();
        showplayer(1);
        cameraSwitcher.SwitchCamera(cameraSwitcher.warriorVcam);

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

        TurnManager tm = FindFirstObjectByType<TurnManager>();
        tm.currentCharacter = this;

        currentAction = SelectedAction.None;
        Click.target = null;

        if (tauntTurnCount > 0)
        {
            tauntTurnCount--;
            if (tauntTurnCount == 0) taunt = false;
        }

        if (IronGuardTurnCount > 0)
        {
            IronGuardTurnCount--;
            if (IronGuardTurnCount == 0)
            {
                sanctuary = false;
            }
        }
        if (isAIControlled)
        {
            StartCoroutine(AutoDecideAction());
        }
        else if(useWeightScoreAI)
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
        currentAction = SelectedAction.Attack;
        endaction();
        Click.showOutline = true;
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

    public void PowerSlash()
    {
        if (magicpoint >= 15)
        {
            currentAction = SelectedAction.PowerSlash;
            endaction();
            Click.showOutline = true;
        }

    }

    public void Taunt()
    {
        if (magicpoint >= 20)
        {
            currentAction = SelectedAction.Taunt;
            endaction();
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            Click.showOutline = true;
        }
    }

    public void IronGuard()
    {
        if (magicpoint >= 25)
        {
            currentAction = SelectedAction.IronGuard;
            endaction();
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            Click.showOutline = true;
        }
    }

    public void Whirlwind()
    {
        if (magicpoint >= 30)
        {
            currentAction = SelectedAction.Whirlwind;
            endaction();
            Click.showOutline = true;
        }
    }

    public void CancelSkill()
    {
        cameraSwitcher.SwitchCamera(cameraSwitcher.warriorVcam);
        showplayer(1);
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
        savestat(warriorStats);
        base.Update();
        textnumHP.text = health.ToString();
        textnumMP.text = magicpoint.ToString();
        hpbar.fillAmount = (float)health / fullhealth;
        mpbar.fillAmount = (float)magicpoint / fullmagicpoint;

        if (currentAction != SelectedAction.None)
        {
            switch (currentAction)
            {
                case SelectedAction.Attack:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint = Mathf.Min(fullmagicpoint, magicpoint + 9);
                        Target = Click.target.GetComponentInParent<Character>();
                        pendingDamage = atk * 0.7f;
                        isPhysicalAttack = true;
                        element = "Physical";
                        soundattack = soundPowerSlash;
                        animatorname = "attack";
                        StartMoveToTarget(Target);
                        currentAction = SelectedAction.None;

                    }
                    break;
                case SelectedAction.PowerSlash:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 15;
                        Target = Click.target.GetComponentInParent<Character>();
                        pendingDamage = atk * 1.5f;
                        isPhysicalAttack = true;
                        element = "Physical";
                        soundattack = soundPowerSlash;
                        animatorname = "PowerSlash";
                        StartMoveToTarget(Target);
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.Taunt:
                    if (Click.target != null && (Click.target.name == "Warrior"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 20;
                        taunt = true;
                        tauntTurnCount = 2;
                        animatorname = "Taunt";
                        soundattack = soundTaunt;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.IronGuard:
                    if (Click.target != null && (Click.target.name == "Warrior"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 25;
                        sanctuary = true;
                        IronGuardTurnCount = 2;
                        animatorname = "attack";
                        soundattack = soundTaunt;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.Whirlwind:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 30;
                        Target = Click.target.GetComponentInParent<Character>();
                        pendingDamage = atk * 0.8f;
                        isPhysicalAttack = true;
                        element = "Physical";
                        animatorname = "Whirlwind";
                        soundattack = soundWhirlwind;
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
        showplayer(1);
        isuseitem = false;
        Click.target = null;
        cameraSwitcher.SwitchCamera(cameraSwitcher.warriorVcam);
        currentItem = SelectedItem.None;
        if (!isAIControlled)
        {
            mainaction.SetActive(true);
        }
    }

    private IEnumerator AutoDecideAction()
    {
        yield return new WaitForSeconds(1f); // หน่วงเวลาเล็กน้อยให้ดูเป็นธรรมชาติ

        var enemies = turnManager.enemyteam.Where(e => e.IsAlive()).ToList();
        var allies = turnManager.playerteam.Where(a => a.IsAlive()).ToList();
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
        // ✅ Rule 0.1:ใช้ item สุ่มผ่าน 50%
        yield return StartCoroutine(TryUseHealingItem(0.5f));

        while (isuseitem)
        {
            yield return null;
        }

        // ✅ Rule 1: ใช้ Taunt ถ้ามีเพื่อนเลือด < 60%, ยังไม่ Taunt, และสุ่ม 70%
        if (magicpoint >= 20 &&
            !taunt &&
            allies.Any(a => a != this && a.health < a.fullhealth * 0.7f) &&
            Random.value < 0.7f)
        {
            Click.target = this.gameObject;
            Taunt();
            
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Taunt");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }
        // ✅ Rule 2: ใช้ IronGuard ถ้าเลือด < 50% และยังไม่บัฟ
        if (magicpoint >= 25 && health < fullhealth * 0.5f && IronGuardTurnCount == 0)
        {
            Click.target = this.gameObject;
            IronGuard();
            
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use IronGuard");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }
        // ✅ Rule 3: ใช้ Whirlwind ถ้า MP พอ และศัตรู ≥ 2 หรือสุ่ม 30%
        if (magicpoint >= 30 && (enemies.Count >= 2 || Random.value < 0.3f))
        {
            var target = enemies.OrderBy(e => e.GetHealthLevel10()).First();
            Click.target = target.gameObject;
            Whirlwind();
            
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Whirlwind");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }
        // ✅ Rule 4: ใช้ PowerSlash ถ้า MP พอ และโจมตีศัตรู HP ต่ำสุด สุ่มผ่าน 80%
        if (magicpoint >= 15 && Random.value < 0.8f || magicpoint >= 40)
        {
            var target = enemies.OrderBy(e => e.GetHealthLevel10()).First();
            Click.target = target.gameObject;
            PowerSlash();
            
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use PowerSlash");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }
        // ✅ Rule 5: ใช้ Basic Attack โจมตีศัตรู HP ต่ำสุด
        {
            var target = enemies.OrderBy(e => e.GetHealthLevel10()).First();
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

    private float GetTauntScore(List<Character> allies)
    {
        int lowHPCount = allies.Count(a => a != this && a.health < a.fullhealth * 0.6f);
        float score = (lowHPCount * 2.0f);
        if (!taunt) score += 1.0f;
        if (magicpoint > 20) score += 0.5f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 20 || taunt) score *= -1;
        return score;
    }

    private float GetIronGuardScore()
    {
        float score = GetInverseHP(health, fullhealth) * 2.5f;
        if (IronGuardTurnCount == 0) score += 1.0f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 25 || IronGuardTurnCount !=0) score *= -1;
        return score;
    }

    private float GetWhirlwindScore(List<Character> enemies)
    {
        float score = 0f;
        if (enemies.Count >= 2) score += 1.5f;
        if (magicpoint > 30) score += 0.5f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 30) score *= -1;
        return score;
    }

    private float GetPowerSlashScore(List<Character> enemies)
    {
        int minLevel = enemies.Min(e => e.GetHealthLevel10()); // ศัตรูเลือดน้อยที่สุด (1-10)
        float inverseLevel = (11 - minLevel) / 10f; // แปลงเป็นค่ากลับแบบ 0.1–1.0

        float score = inverseLevel * 2.0f;
        if (magicpoint > 15) score += 0.5f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 15) score *= -1;
        return score;
    }

    private float GetBasicAttackScore(List<Character> enemies)
    {
        int minLevel = enemies.Min(e => e.GetHealthLevel10());
        float inverseLevel = (11 - minLevel) / 10f;

        return inverseLevel * 1.0f + 0.2f;
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
        { "Taunt", GetTauntScore(allies) },
        { "IronGuard", GetIronGuardScore() },
        { "Whirlwind", GetWhirlwindScore(enemies) },
        { "PowerSlash", GetPowerSlashScore(enemies) },
        { "Attack", GetBasicAttackScore(enemies) }
    };

        var bestSkill = skillScores.OrderByDescending(kv => kv.Value).First().Key;

        Character lowestEnemy = enemies.OrderBy(e => e.GetHealthLevel10()).First();

        switch (bestSkill)
        {
            case "Taunt":
                Click.target = this.gameObject;
                Taunt(); 
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Taunt");
                textai.text = PlayerDataManager.getaiaction();
                break;
            case "IronGuard":
                Click.target = this.gameObject;
                IronGuard(); 
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use IronGuard");
                textai.text = PlayerDataManager.getaiaction();
                break;
            case "Whirlwind":
                Click.target = lowestEnemy.gameObject;
                Whirlwind(); 
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Whirlwind");
                textai.text = PlayerDataManager.getaiaction();
                break;
            case "PowerSlash":
                Click.target = lowestEnemy.gameObject;
                PowerSlash(); 
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use PowerSlash");
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

