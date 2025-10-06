using System.Collections;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;
using static Unity.Cinemachine.CinemachineTargetGroup;

public class Cleric : Character
{
    TurnManager turnManager;
    private int sanctuaryTurnCount = 0;

    private enum SelectedAction { None, Attack, Heal, Sanctuary, Blessing, Purify, Curse }
    private SelectedAction currentAction = SelectedAction.None;

    public bool isAIControlled = false;
    public bool useWeightScoreAI = false;
    private CameraSwitcher cameraSwitcher;

    public AudioClip soundattack1;
    public AudioClip soundSanctuary;
    public AudioClip soundCurse;

    CharacterStats clericStats;
    protected override void Awake()
    {
        base.Awake();

        clericStats = PlayerDataManager.GetCharacterStats("Cleric");

        Click = GetComponent<ClickSelector>();
        turnManager = FindFirstObjectByType<TurnManager>();
        cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene!");
        }
        originalPosition = transform.position;
        health = clericStats.currentHealth;
        if (health <= 0)
        {
            health = Mathf.CeilToInt((float)clericStats.baseHealth * 0.10f);
        }
        fullhealth = clericStats.baseHealth;
        magicpoint = clericStats.currentMagicPoint;
        fullmagicpoint = clericStats.baseMagicPoint;
        atk = clericStats.currentAttack;
        baseatk = clericStats.baseAttack;
        Int = clericStats.currentIntelligence;
        baseInt = clericStats.baseIntelligence;
        def = clericStats.currentDefense;
        basedef = clericStats.baseDefense;
        res = clericStats.currentResistance;
        baseres = clericStats.baseResistance;
        speed = clericStats.currentSpeed;
        basespeed = clericStats.baseSpeed;
        luck = clericStats.currentLuck;
        isAIControlled = clericStats.rulebase;
        useWeightScoreAI = clericStats.weightscore;

        weaknesses = new List<string> { "none" };
    }


    public override void TakeTurn()
    {
        base.TakeTurn();
        endaction();
        showplayer(3);
        cameraSwitcher.SwitchCamera(cameraSwitcher.clericVcam);

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

        turnManager.currentCharacter = this;
        Click.target = null;

        currentAction = SelectedAction.None;
        Click.target = null;
        if (sanctuaryTurnCount > 0)
        {
            sanctuaryTurnCount--;
            if (sanctuaryTurnCount == 0) taunt = false;
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

    public void Heal()
    {
        if (magicpoint >= 20)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            currentAction = SelectedAction.Heal;
            endaction();
            Click.showOutline = true;
        }
    }

    public void Sanctuary()
    {
        if (magicpoint >= 25)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            currentAction = SelectedAction.Sanctuary;
            endaction();
            Click.showOutline = true;
        }
    }

    public void Blessing()
    {
        if (magicpoint >= 20)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            currentAction = SelectedAction.Blessing;
            endaction();
            Click.showOutline = true;
        }
    }

    public void Purify()
    {
        if (magicpoint >= 15)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.playerVcam);
            showplayer(0);
            currentAction = SelectedAction.Purify;
            endaction();
            Click.showOutline = true;
        }
    }

    public void Curse()
    {
        if (magicpoint >= 45)
        {
            currentAction = SelectedAction.Curse;
            endaction();
            Click.showOutline = true;
        }
    }

    public void CancelSkill()
    {
        cameraSwitcher.SwitchCamera(cameraSwitcher.clericVcam);
        showplayer(3);
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
        savestat(clericStats);
        base.Update();
        textnumHP.text = health.ToString();
        textnumMP.text = magicpoint.ToString();
        hpbar.fillAmount = (float)health / fullhealth;
        mpbar.fillAmount = (float)magicpoint / fullmagicpoint;

        if (currentAction != SelectedAction.None)
            switch (currentAction)
            {
                case SelectedAction.Attack:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint = Mathf.Min(fullmagicpoint, magicpoint + 10);
                        Target = Click.target.GetComponentInParent<Character>();
                        pendingDamage = atk * 0.7f;
                        isPhysicalAttack = true;
                        element = "Physical";
                        soundattack = soundattack1;
                        animatorname = "attack";
                        StartMoveToTarget(Target);
                        currentAction = SelectedAction.None;
                    }
                    break;
                case SelectedAction.Heal:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 20;
                        Target = Click.target.GetComponentInParent<Character>();
                        Target.TakeHeal(Mathf.CeilToInt(Int * 1f));
                        animatorname = "attack";
                        soundattack = soundSanctuary;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.Sanctuary:
                    if (Click.target != null && (Click.target.name == "Cleric"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 25;
                        sanctuary = true;
                        sanctuaryTurnCount = 2;
                        animatorname = "attack";
                        soundattack = soundSanctuary;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.Blessing:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 20;
                        Target = Click.target.GetComponentInParent<Character>();
                        var blessingEffectPrefab = Resources.Load<Blessing>("StatusEffects/Blessing");
                        TurnManager.Instance.statusEffectManager.AddEffect(Target, blessingEffectPrefab);
                        animatorname = "attack";
                        soundattack = soundSanctuary;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.Purify:
                    if (Click.target != null && Click.target.CompareTag("Player"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 15;
                        Target = Click.target.GetComponentInParent<Character>();
                        TurnManager.Instance.statusEffectManager.RemoveAllDebuffs(Target);
                        Target.posion = 0;
                        Target.crabstack = 0;
                        animatorname = "attack";
                        soundattack = soundSanctuary;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;

                case SelectedAction.Curse:
                    if (Click.target != null && Click.target.CompareTag("enemy"))
                    {
                        Click.showOutline = false;
                        magicpoint -= 45;
                        var curseEffectPrefab = Resources.Load<Curse>("StatusEffects/Curse");
                        foreach (Character c in TurnManager.characters)
                        {
                            if (c.isEnemy && c.IsAlive())
                            {
                                TurnManager.Instance.statusEffectManager.AddEffect(c, curseEffectPrefab);
                            }
                        }
                        animatorname = "attack";
                        soundattack = soundCurse;
                        animator.SetTrigger(animatorname);
                        animatoinplay = 0;
                        isAnimation = true;
                        currentAction = SelectedAction.None;
                    }
                    break;
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
        showplayer(3);
        isuseitem = false;
        Click.target = null;
        cameraSwitcher.SwitchCamera(cameraSwitcher.clericVcam);
        currentItem = SelectedItem.None;
        if (!isAIControlled)
        {
            mainaction.SetActive(true);
        }
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
    private bool findteamstack()
    {
        Character[] all = FindObjectsByType<Character>(FindObjectsSortMode.None);
        List<Character> players = new List<Character>();

        foreach (Character c in all)
        {
            if (!c.isEnemy && c.IsAlive())
            {
                if (c.posion >=4||c.crabstack >=6)
                {
                    return true;
                }
            }
        }
        return false; 
    }
    private Character findteamstackplayer()
    {
        Character[] all = FindObjectsByType<Character>(FindObjectsSortMode.None);
        List<Character> players = new List<Character>();

        foreach (Character c in all)
        {
            if (!c.isEnemy && c.IsAlive())
            {
                if (c.posion >= 4 || c.crabstack >= 6)
                {
                    return c;
                }
            }
        }
        return null;
    }

    private IEnumerator AutoDecideAction()
    {
        yield return new WaitForSeconds(1f); // หน่วงเวลาเล็กน้อยให้ดูเป็นธรรมชาติ

        var allies = turnManager.playerteam.Where(a => a != null).ToList();
        var aliveAllies = allies.Where(a => a.IsAlive()).ToList();
        var enemies = turnManager.enemyteam.Where(e => e.IsAlive()).ToList();
        var fallenAlly = allies.FirstOrDefault(a => !a.IsAlive());
        // ✅ Rule 0: ชุบเพื่อนถ้าตาย

        if (phoenixfeather > 0 && fallenAlly != null)
        {
            Click.target = fallenAlly.gameObject;
            UsePhoenixfeather();

            aliveAllies = allies.Where(a => a.IsAlive()).ToList();
            Character name = fallenAlly.gameObject.GetComponentInParent<Character>();
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Phoenixfeather to {name.characterName}");
            textai.text = PlayerDataManager.getaiaction();
        }
        // ✅ Rule 0.1:ใช้ item สุ่ม 50%
        yield return StartCoroutine(TryUseHealingItem(0.5f));

        while (isuseitem)
        {
            yield return null;
        }

        // ✅ Rule 1: ใช้ Heal ถ้าเพื่อนมี HP < 60% และสุ่ม 90%
        aliveAllies = allies.Where(a => a.IsAlive()).ToList();
        var lowHPAlly = aliveAllies
            .Where(a => a.health < a.fullhealth * 0.6f)
            .OrderBy(a => a.health)
            .FirstOrDefault();

        if (magicpoint >= 20 && lowHPAlly != null && Random.value < 0.9f)
        {
            Click.target = lowHPAlly.gameObject;
            Heal();

            Character name = lowHPAlly.gameObject.GetComponentInParent<Character>();
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Heal to {name.characterName}");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ✅ Rule 2: ใช้ Purify ถ้ามีเพื่อนติดดีบัฟ สุ่ม 90%
        var debuffedAlly = aliveAllies.FirstOrDefault(a =>
            TurnManager.Instance.statusEffectManager.HasDebuff(a));
        if (magicpoint >= 15 && debuffedAlly != null && Random.value < 0.9f|| magicpoint >= 15 && findteamstack())
        {
            if (findteamstack())
            {
                Click.target = findteamstackplayer().gameObject;
            }
            else
            {
                Click.target = debuffedAlly.gameObject;
            }
            Purify();

            Character name = Click.target.GetComponentInParent<Character>();
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Purify to {name.characterName}");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        var unblessedAlly = aliveAllies.FirstOrDefault(a =>
            !TurnManager.Instance.statusEffectManager.HasBuff(a));

        // มีเพื่อนที่ไม่มีบัฟ (ไม่รวมตัวเอง) หรือไม่
        var unblessedOtherAlly = aliveAllies.FirstOrDefault(a =>
            a != this && !TurnManager.Instance.statusEffectManager.HasBuff(a));

        // เลือกเป้าหมายสำหรับ Blessing
        Character targetForBlessing = null;
        if (unblessedOtherAlly != null)
        {
            targetForBlessing = unblessedOtherAlly;
        }
        else if (!TurnManager.Instance.statusEffectManager.HasBuff(this))
        {
            targetForBlessing = this;
        }

        if (magicpoint >= 20 && targetForBlessing != null && (Random.value < 0.9f || magicpoint >= 40))
        {
            Click.target = targetForBlessing.gameObject;
            Blessing();

            Character name = targetForBlessing.gameObject.GetComponentInParent<Character>();
            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Blessing to {name.characterName}");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ✅ Rule 4: ใช้ Sanctuary ถ้าเลือดตัวเองต่ำ และยังไม่มี Sanctuary
        if (magicpoint >= 25 && health < fullhealth * 0.5f && !sanctuary)
        {
            Click.target = this.gameObject;
            Sanctuary();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Sanctuary");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ✅ Rule 5: ใช้ Curse ถ้ามีศัตรูหลายตัว และสุ่มผ่าน 60%
        var debuffedenemies = enemies.FirstOrDefault(a =>
            TurnManager.Instance.statusEffectManager.HasDebuff(a));

        Character target = null;
        if (enemies != null && enemies.Count > 0)
            target = findenemy("Physical");

        if (magicpoint >= 45 && enemies.Count >= 2 && debuffedenemies == null && Random.value < 0.6f && target != null)
        {
            Click.target = target.gameObject;
            Curse();

            PlayerDataManager.setaiaction($"{characterName}AI Rulebase Use Curse to Enemy");
            textai.text = PlayerDataManager.getaiaction();
            yield break;
        }

        // ✅ Rule 6: ใช้ Attack โจมตีศัตรู HP ต่ำสุด
        if (target != null)
        {
            Click.target = findenemy("Physical").gameObject;
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

    private float GetHealScore(List<Character> allies)
    {
        int lowHPCount = allies.Count(a => a.health < a.fullhealth * 0.6f && a != this);
        float score = lowHPCount * 2.5f;
        if (magicpoint > 20) score += 0.5f;
        score += Random.Range(0f, 0.4f);
        if (magicpoint < 20) score *= -1;
        return score;
    }

    private float GetPurifyScore(List<Character> allies)
    {
        bool hasDebuffedAlly = allies.Any(a => TurnManager.Instance.statusEffectManager.HasDebuff(a));
        float score = hasDebuffedAlly ? 1.5f : 0f;
        if (findteamstack()) score += 1.5f;
        if (magicpoint > 15) score += 0.3f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 15) score *= -1;
        return score;
    }

    private float GetBlessingScore(List<Character> allies)
    {
        bool hasUnbuffedAlly = allies.Any(a => !TurnManager.Instance.statusEffectManager.HasBuff(a));
        float score = hasUnbuffedAlly ? 1.3f : 0f;
        score += Random.Range(0f, 0.3f);
        if (magicpoint < 20) score *= -1;
        return score;
    }

    private float GetSanctuaryScore()
    {
        float score = GetInverseHP(health, fullhealth) * 2.0f;
        if (!sanctuary) score += 1.0f;
        if (magicpoint < 25) score *= -1;
        return score;
    }

    private float GetCurseScore(List<Character> enemies)
    {
        var debuffedenemies = enemies.FirstOrDefault(a =>
            TurnManager.Instance.statusEffectManager.HasDebuff(a));
        float score = enemies.Count >= 3 ? 1.5f : 0f;
        if (magicpoint > 45) score += 0.5f;
        score += Random.Range(0f, 0.4f);
        if (magicpoint < 45 || debuffedenemies != null) score *= -1;
        return score;
    }

    private float GetBasicAttackScore(List<Character> enemies)
    {
        int minLevel = enemies.Min(e => e.GetHealthLevel10());
        float inverseLevel = (11 - minLevel) / 10f;
        float score = 0;
        if (findenemy("Physical")) { score += 0.8f; }
        return inverseLevel * 1.0f + 0.2f;
    }
    private IEnumerator AutoDecideActionByWeight()
    {
        yield return new WaitForSeconds(1f);

        var allies = turnManager.playerteam.Where(a => a.IsAlive()).ToList();
        var enemies = turnManager.enemyteam.Where(e => e.IsAlive()).ToList();
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
        // กรณีมีเพื่อนตาย ใช้ Phoenix Feather ชุบ (เหมือนที่คุณมีอยู่)

        Dictionary<string, float> skillScores = new Dictionary<string, float>
    {
        { "Heal", GetHealScore(allies) },
        { "Purify", GetPurifyScore(allies) },
        { "Blessing", GetBlessingScore(allies) },
        { "Sanctuary", GetSanctuaryScore() },
        { "Curse", GetCurseScore(enemies) },
        { "Attack", GetBasicAttackScore(enemies) }
    };

        var bestSkill = skillScores.OrderByDescending(kv => kv.Value).First().Key;

        Character lowestEnemy = findenemy("Physical");
        Character lowestAlly = allies
            .Where(a => a.health < a.fullhealth * 0.6f && a != this)
            .OrderBy(a => a.health)
            .FirstOrDefault();

        Character debuffedAlly = allies.FirstOrDefault(a => TurnManager.Instance.statusEffectManager.HasDebuff(a));
        Character unbuffedAlly = allies.FirstOrDefault(a => !TurnManager.Instance.statusEffectManager.HasBuff(a) && a != this);

        switch (bestSkill)
        {
            case "Heal":
                if (lowestAlly != null)
                {
                    Click.target = lowestAlly.gameObject;
                    Heal();
                    Character name = lowestAlly.gameObject.GetComponentInParent<Character>();
                    PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Heal to {name.characterName}");
                    textai.text = PlayerDataManager.getaiaction();
                }
                break;

            case "Purify":
                if (debuffedAlly != null)
                {
                    Click.target = debuffedAlly.gameObject;
                    Purify();
                    Character name = debuffedAlly.gameObject.GetComponentInParent<Character>();
                    PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Purify to {name.characterName}");
                    textai.text = PlayerDataManager.getaiaction();
                }
                else if (findteamstack())
                {
                    Click.target = findteamstackplayer().gameObject;
                    Purify();
                    Character name = findteamstackplayer().gameObject.GetComponentInParent<Character>();
                    PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Purify to {name.characterName}");
                    textai.text = PlayerDataManager.getaiaction();
                }
                break;

            case "Blessing":
                if (unbuffedAlly != null)
                {
                    Click.target = unbuffedAlly.gameObject;
                    Blessing();
                    Character name = unbuffedAlly.gameObject.GetComponentInParent<Character>();
                    PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Blessing to {name.characterName}");
                    textai.text = PlayerDataManager.getaiaction();
                }
                break;

            case "Sanctuary":
                Click.target = this.gameObject;
                Sanctuary();
                PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Sanctuary");
                textai.text = PlayerDataManager.getaiaction();
                break;

            case "Curse":
                if (enemies.Count > 0)
                {
                    Click.target = enemies[Random.Range(0, enemies.Count)].gameObject;
                    Curse();
                    PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Curse to Enemy");
                    textai.text = PlayerDataManager.getaiaction();
                }
                break;

            case "Attack":
            default:
                if (lowestEnemy != null)
                {
                    Click.target = lowestEnemy.gameObject;
                    Attack();
                    PlayerDataManager.setaiaction($"{characterName}AI Weight Score Use Attack");
                    textai.text = PlayerDataManager.getaiaction();
                }
                break;
        }


    }
}