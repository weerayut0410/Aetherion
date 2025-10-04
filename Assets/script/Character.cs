using TMPro;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
    public string characterName;
    public int fullhealth;
    public int health;
    public int shieldpoint;
    public int speed;
    public int basespeed;
    public int magicpoint;
    public int fullmagicpoint;
    public int atk;
    public int baseatk;
    public int Int;
    public int baseInt;
    public int def;
    public int basedef;
    public int res;
    public int baseres;
    public int luck;
    public bool useitem = false;
    public bool isuseitem = false;
    public bool isaction = false;
    public bool isEnemy = false;
    public bool taunt = false;
    public bool shield = false;
    public bool moveto = false;
    public bool sanctuary = false;
    protected TextMeshProUGUI text;
    public bool isdie = false;

    private bool MovingToTarget = false;
    private Vector3 targetPosition;
    private Character moveTarget;
    protected bool isAnimation = false;
    protected bool isAttacking = false;
    private bool hasDealtDamage = false;
    protected float pendingDamage;
    protected bool isPhysicalAttack;
    protected int animatoinplay = 0;
    protected Character Target = null;

    public float distance = 1f;
    public float moveSpeed = 5f;
    protected Vector3 originalPosition;
    protected bool isReturning = false;
    protected Quaternion originalRotation;

    protected Animator animator;
    protected string animatorname = "";


    public StatusEffectManager statusEffectManager;

    protected int HPPotion;
    protected int FullHPPotion;
    protected int ManaPotion;
    protected int FullManaPotion;
    protected int phoenixfeather;

    public TextMeshProUGUI textnumHPPotion;
    public TextMeshProUGUI textnumFullHPPotion;
    public TextMeshProUGUI textnumManaPotion;
    public TextMeshProUGUI textnumFullManaPotion;
    public TextMeshProUGUI textnumphoenixfeather;
    public TextMeshProUGUI textai;

    public ClickSelector Click;

    public GameObject playerW;
    public GameObject playerM;
    public GameObject playerC;

    public GameObject textW;
    public GameObject textM;
    public GameObject textC;

    private Vector3 originalPosW;
    private Vector3 originalPosM;
    private Vector3 originalPosC;

    public float positionThreshold = 0.05f; // ค่าเผื่อสำหรับตำแหน่ง (อาจจะ 0.01f - 0.1f)
    public float rotationThreshold = 1.0f; // ค่าเผื่อสำหรับมุม (อาจจะ 0.5f - 2.0f องศา)

    public GameObject mainCanvas;
    public GameObject mainaction;
    public GameObject Skillcanvas;
    public GameObject itemcanvas;

    public Image hpbar;
    public Image mpbar;
    public TextMeshProUGUI textnumHP;
    public TextMeshProUGUI textnumMP;

    public Sprite characterPortrait;

    public PlayerSaveData playerSaveData;
    private InventoryData inventory;

    protected enum SelectedItem { None, HPPo, FullHPPo, ManaPo, FullManaPo, phoenix }
    protected SelectedItem currentItem = SelectedItem.None;

    public AudioClip soundattack;
    public AudioClip sounduse;
    public AudioClip soundphoenix;
    GameObject sound;
    AudioSource audiosound;

    public TextMeshProUGUI textdamage;
    public TextMeshProUGUI textheal;

    public List<string> weaknesses = new List<string>();
    public string element = "";
    public int posion;
    public int crabstack;

    public TextMeshProUGUI textposion;
    public TextMeshProUGUI textcrabstack;

    public string effect;

    protected virtual void Awake()
    {
        posion = 0;
        crabstack = 0;
        if (text == null)
            text = GameObject.Find("Damagescore")?.GetComponent<TextMeshProUGUI>();
        statusEffectManager = GetComponent<StatusEffectManager>();
        animator = GetComponent<Animator>();

        playerW = GameObject.Find("Warrior");
        playerM = GameObject.Find("Mage");
        playerC = GameObject.Find("Cleric");

        textW = GameObject.Find("cw");
        textM = GameObject.Find("cm");
        textC = GameObject.Find("cc");

        originalPosW = playerW.transform.position;
        originalPosM = playerM.transform.position;
        originalPosC = playerC.transform.position;
        inventory = PlayerDataManager.GetInventoryData();

        inventory = PlayerDataManager.GetInventoryData();
        RefreshInventoryValues();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        sound = GameObject.Find("sound");
        audiosound = sound.GetComponent<AudioSource>();
        if (textdamage != null)
        {
            textdamage.gameObject.SetActive(false);
        }
        if (textheal != null)
        {
            textheal.gameObject.SetActive(false);
        }
    }

    public void showplayer(int num)
    {
        if (num == 1)
        {
            playerW.transform.position = originalPosW;
            playerM.transform.position = originalPosM + Vector3.left * 1f;
            playerC.transform.position = originalPosC + Vector3.left * 1f;
            textW.transform.position = originalPosW;
            textM.transform.position = originalPosM + Vector3.right * 1f;
            textC.transform.position = originalPosC + Vector3.left * 1f;
        }
        else if (num == 2)
        {
            playerW.transform.position = originalPosW + Vector3.right * 1f;
            playerM.transform.position = originalPosM;
            playerC.transform.position = originalPosC + Vector3.left * 1f;
            textW.transform.position = originalPosW + Vector3.right * 1f;
            textM.transform.position = originalPosM;
            textC.transform.position = originalPosC + Vector3.left * 1f;
        }
        else if (num == 3)
        {
            playerW.transform.position = originalPosW + Vector3.right * 1f;
            playerM.transform.position = originalPosM + Vector3.right * 1f;
            playerC.transform.position = originalPosC;
            textW.transform.position = originalPosW + Vector3.right * 1f;
            textM.transform.position = originalPosM + Vector3.right * 1f;
            textC.transform.position = originalPosC;
        }
        else
        {
            playerW.transform.position = originalPosW;
            playerM.transform.position = originalPosM;
            playerC.transform.position = originalPosC;
            textW.transform.position = originalPosW;
            textM.transform.position = originalPosM;
            textC.transform.position = originalPosC;
        }
    }

    public virtual IEnumerator HPPo(Character target)
    {
        PlayerDataManager.UseItem("hpPotion");
        RefreshInventoryValues();
        if (target.health < target.fullhealth && target.IsAlive())
        {
            target.TakeHeal(40);
            audiosound.clip = sounduse;
            audiosound.Play();
        }
        Target = null;
        yield return new WaitForSeconds(2);
        useitem = false;

    }
    public virtual IEnumerator FullHPPo(Character target)
    {
        PlayerDataManager.UseItem("fullHpPotion");
        RefreshInventoryValues();
        if (target.health < target.fullhealth && target.IsAlive())
        {
            target.TakeHeal(80);
            audiosound.clip = sounduse;
            audiosound.Play();
        }
        Target = null;
        yield return new WaitForSeconds(2);
        useitem = false;
    }
    public virtual IEnumerator ManaPo(Character target)
    {
        PlayerDataManager.UseItem("manaPotion");
        RefreshInventoryValues();
        if (target.magicpoint < target.fullmagicpoint && target.IsAlive())
        {
            target.magicpoint += 20;
            if (target.magicpoint > target.fullmagicpoint)
                target.magicpoint = target.fullmagicpoint;
            audiosound.clip = sounduse;
            audiosound.Play();
        }

        Target = null;
        yield return new WaitForSeconds(2);
        useitem = false;
    }
    public virtual IEnumerator phoenix(Character target)
    {
        PlayerDataManager.UseItem("phoenixFeather");
        RefreshInventoryValues();
        target.health = Mathf.CeilToInt(target.fullhealth * 0.5f); // ฟื้น HP 50%
        if (!TurnManager.Instance.turnQueue.Contains(target))
        {
            target.animator.SetBool("die", false);
            TurnManager.Instance.turnQueue.Enqueue(target);
            audiosound.clip = soundphoenix;
            audiosound.Play();
        }
        TurnManager.Instance.statusEffectManager.RemoveAllDebuffs(target);
        target.isdie = false;
        Target = null;
        yield return new WaitForSeconds(2);
        useitem = false;
    }
    public virtual IEnumerator FullManaPo(Character target)
    {
        PlayerDataManager.UseItem("fullManaPotion");
        RefreshInventoryValues();
        if (target.magicpoint < target.fullmagicpoint && target.IsAlive())
        {
            target.magicpoint += 40;
            if (target.magicpoint > target.fullmagicpoint)
                target.magicpoint = target.fullmagicpoint;
            audiosound.clip = sounduse;
            audiosound.Play();
        }
        Target = null;
        yield return new WaitForSeconds(2);
        useitem = false;
    }

    public virtual void TakeTurn()
    {
        text.text = "";
        useitem = true;
        
    }

    public void EndTurn()
    {
        Target = null;
        audiosound.Stop();
        TurnManager.Instance.statusEffectManager.OnTurnEnd(this);
        TurnManager.Instance.NextTurn();
    }

    public bool IsAlive()
    {
        return health > 0;
    }
    public IEnumerator ShowTextForSeconds(int damage, TextMeshProUGUI text, bool heal, TextMeshProUGUI damagetext = null)
    {
        if (text != null) 
        {
            // ตั้งค่าข้อความและทำให้มองเห็นได้
            text.text = damage.ToString();
            text.gameObject.SetActive(true);

        }
        if (damagetext != null && heal == false) 
        {
            damagetext.text = damage.ToString();
            damagetext.gameObject.SetActive(true);
        }

        // รอตามระยะเวลาที่กำหนด
        yield return new WaitForSeconds(2f);

        // ซ่อนข้อความ
        if (text != null)
        {
            text.gameObject.SetActive(false);
        }
        if (damagetext != null)
        {
            damagetext.gameObject.SetActive(false);
        }
        
    }
    public void TakeDamage(int amount, bool atk,string element = null)
    {
        if (animator != null) 
        {
            animator.SetTrigger("Hit");
        }
        
        int damageLeft = 0;
        if (atk)
        {
            amount -= Mathf.CeilToInt(def * 0.5f);
            if (amount > 0)
            {
                damageLeft = amount;
            }
            else if (amount <= 0)
            {
                damageLeft = 1;
            }
        }
        else
        {
            amount -= Mathf.CeilToInt(res * 0.5f);
            if (amount > 0)
            {
                damageLeft = amount;
            }
            else if (amount <= 0)
            {
                damageLeft = 1;
            }
        }

        if (shield)
        {
            if (damageLeft <= shieldpoint)
            {
                shieldpoint -= damageLeft;
                damageLeft = 0;
                StartCoroutine(ShowTextForSeconds(damageLeft,textdamage, false,text));
            }
            else
            {
                damageLeft -= shieldpoint;
                shieldpoint = 0;
            }

            if (shieldpoint == 0)
            {
                shield = false;
            }
        }

        if (damageLeft > 0)
        {
            if (sanctuary)
            {
                damageLeft = Mathf.CeilToInt(damageLeft * 0.5f);
            }
            else if (element != null)
            {
                if (element != null && weaknesses.Contains(element))
                {
                    damageLeft = Mathf.CeilToInt(damageLeft * 1.5f);
                    print("weaknesses"+element);
                }
            }
            health -= damageLeft;
            StartCoroutine(ShowTextForSeconds(damageLeft, textdamage, false, text));
        }

    }

    public void TakeHeal(int amount)
    {
        health += amount;
        if (health > fullhealth)
            health = fullhealth;
        StartCoroutine(ShowTextForSeconds(amount, textheal,true));
    }

    public void Die()
    {
        isdie = true;

        health = 0;
        taunt = false;
        shield = false;
        //ตาย animator.SetTrigger("die");
        animator.SetBool("die", true);
        if (TurnManager.Instance.currentCharacter == this)
        {
            EndTurn();
        }
    }

    public int GetHealthLevel10()
    {
        if (fullhealth <= 0) return 1;
        float percent = (float)health / fullhealth * 100f;
        int level = Mathf.FloorToInt(percent / 10f) + 1;
        return Mathf.Clamp(level, 1, 10);
    }

    public virtual IEnumerator TryUseHealingItem(float chance = 0.5f)
    {
        if (Random.value > chance)
        {
            yield break; // ไม่ใช้ไอเทม โอกาสไม่ถึง
        }
        RefreshInventoryValues();
        var turnManager = TurnManager.Instance;
        if (turnManager == null)
        {
            Debug.LogError("TurnManager.Instance is null!");
            yield break;
        }

        if (turnManager.playerteam == null)
        {
            Debug.LogError("playerteam is null in TurnManager!");
            yield break;
        }

        var allies = turnManager.playerteam.Where(a => a != null && a.IsAlive()).ToList();

        if (this.IsAlive() && !allies.Contains(this))
            allies.Add(this);

        var lowHP = allies
            .Where(a => (float)a.health / a.fullhealth < 0.5f)
            .OrderBy(a => a.health)
            .FirstOrDefault();

        if (lowHP != null)
        {
            if (FullHPPotion > 0)
            {
                currentItem = SelectedItem.FullHPPo;

                if (Click == null)
                {
                    Debug.LogError("Click is null!");
                    yield break;
                }

                Click.target = lowHP.gameObject;
                Character name = lowHP.gameObject.GetComponentInParent<Character>();
                PlayerDataManager.setaiaction($"{characterName}AI Use FullHPPotion to {name.characterName}");
                textai.text = PlayerDataManager.getaiaction();
                yield break;
            }
            else if (HPPotion > 0)
            {
                currentItem = SelectedItem.HPPo;
                if (Click == null)
                {
                    Debug.LogError("Click is null!");
                    yield break;
                }
                Click.target = lowHP.gameObject;
                Character name = lowHP.gameObject.GetComponentInParent<Character>();
                PlayerDataManager.setaiaction($"{characterName}AI Use HPPotion to {name.characterName}");
                textai.text = PlayerDataManager.getaiaction();
                yield break;
            }
        }

        var lowMP = allies
            .Where(a => (float)a.magicpoint / a.fullmagicpoint < 0.3f)
            .OrderBy(a => a.magicpoint)
            .FirstOrDefault();

        if (lowMP != null)
        {
            if (FullManaPotion > 0)
            {
                currentItem = SelectedItem.FullManaPo;
                if (Click == null)
                {
                    Debug.LogError("Click is null!");
                    yield break;
                }
                Click.target = lowMP.gameObject;
                Character name = lowMP.gameObject.GetComponentInParent<Character>();
                PlayerDataManager.setaiaction($"{characterName}AI Use FullManaPotion to {name.characterName}");
                textai.text = PlayerDataManager.getaiaction();
                yield break;
            }
            else if (ManaPotion > 0)
            {
                currentItem = SelectedItem.ManaPo;
                if (Click == null)
                {
                    Debug.LogError("Click is null!");
                    yield break;
                }
                Click.target = lowMP.gameObject;
                Character name = lowMP.gameObject.GetComponentInParent<Character>();
                PlayerDataManager.setaiaction($"{characterName}AI Use ManaPotion to {name.characterName}");
                textai.text = PlayerDataManager.getaiaction();
                yield break;
            }
        }
    }


    public void StartMoveToTarget(Character target)
    {
        if (target == null) return;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        targetPosition = target.transform.position - direction * 1.2f;
        targetPosition.y = transform.position.y;

        // หันไปทางศัตรู
        Vector3 lookDirection = direction;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        isaction = true;
        hasDealtDamage = false;
        MovingToTarget = true;
    }
    public void savestat(CharacterStats character)
    {
        character.currentHealth = health;
        character.currentMagicPoint = magicpoint;
        character.currentAttack = atk;
        character.currentIntelligence = Int;
        character.currentDefense = def;
        character.currentResistance = res;
    }
    private void RefreshInventoryValues()
    {
        HPPotion = inventory.hpPotion;
        FullHPPotion = inventory.fullHpPotion;
        ManaPotion = inventory.manaPotion;
        FullManaPotion = inventory.fullManaPotion;
        phoenixfeather = inventory.phoenixFeather;
    }
    public void StartReturnToOriginal()
    {
        isReturning = true;
    }
    protected virtual void Update()
    {

        RefreshInventoryValues();
        if (!isEnemy)
        {
            textnumHPPotion.text = $"{HPPotion}";
            textnumFullHPPotion.text = $"{FullHPPotion}";
            textnumManaPotion.text = $"{ManaPotion}";
            textnumFullManaPotion.text = $"{FullManaPotion}";
            textnumphoenixfeather.text = $"{phoenixfeather}";
        }
        if (!isdie)
        {
            if (health <= 0)
            {
                Die();
            }
        }

        if(textposion != null && posion != 0)
    {
            textposion.text = posion.ToString();
        }
        if(textcrabstack != null && crabstack != 0)
    {
            textcrabstack.text = crabstack.ToString();
        }

        if (MovingToTarget)
        {
            animator.SetBool("MovingToTarget", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < distance)
            {
                MovingToTarget = false;
                animator.SetBool("MovingToTarget", false);
                isAttacking = true;
                animator.SetTrigger(animatorname);
            }
            return;
        }

        if (isAnimation)
        {
            isaction = true;
            var state = animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName(animatorname))
            {
                if (state.normalizedTime >= 0.4f && !hasDealtDamage)
                {
                    float clipLen = state.length;
                    float currentTime = state.normalizedTime * clipLen;

                    // อยากให้ถึงเร็วขึ้น/ทันแน่ ๆ: ใช้ earlyFactor < 1f ก็ได้
                    // float earlyFactor = 0.9f;
                    // float remainingTime = Mathf.Max(0.6f, (clipLen - currentTime) * earlyFactor);
                    float remainingTime = Mathf.Max(0.6f, clipLen - currentTime);

                    if (animatoinplay == 1 && !hasDealtDamage && state.normalizedTime >= 0.2f)
                    {
                        // ยิงไฟบอล
                        GameObject fb = Instantiate(Resources.Load<GameObject>(effect), transform.position, Quaternion.identity);

                        // ระยะบนระนาบ XZ (ไม่สน Y เพราะเราจะล็อก Y)
                        Vector3 from = transform.position;
                        Vector3 to = Target.transform.position;

                        Vector2 from2 = new Vector2(from.x, from.z);
                        Vector2 to2 = new Vector2(to.x, to.z);
                        float planarDist = Vector2.Distance(from2, to2);

                        // ความเร็ว = ระยะ / เวลา
                        float speed = planarDist / remainingTime;

                        // เร่งให้ถึงเร็วขึ้นอีกเท่าตัวตามที่คุณตั้งใจ (ปรับตามฟีลได้)
                        speed *= 2f;

                        // (ทางเลือก) กันเร็ว/ช้าเกินไป
                        speed = Mathf.Clamp(speed, 2f, 100f);

                        // ล็อก Y ที่ตำแหน่งผู้ร่าย
                        float fixedYValue = from.y + 1f;

                        if (fb.TryGetComponent<TimedMover>(out var mover))
                        {
                            mover.Init(
                                Target.transform,
                                speed,
                                () => {
                                    DoDamage(Target, pendingDamage, isPhysicalAttack);
                                },
                                lockY: true,
                                fixedY: fixedYValue
                            );
                        }
                        else
                        {
                            Debug.LogError("TimedMover ไม่ได้ติดอยู่ใน prefab 'fire'");
                            Destroy(fb);
                            
                        }

                        audiosound.clip = soundattack;
                        audiosound.Play();
                        hasDealtDamage = true;
                    }
                    else if (animatoinplay == 2)
                    {
                        audiosound.clip = soundattack;
                        audiosound.Play();
                        DoAOE(pendingDamage, isPhysicalAttack);
                        hasDealtDamage = true;
                    }
                    else
                    {
                        audiosound.clip = soundattack;
                        audiosound.Play();
                        hasDealtDamage = true;
                    }
                }
                if (state.normalizedTime >= 0.9f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * 5f);
                }

                if (state.normalizedTime >= 1f)
                {
                    transform.rotation = originalRotation;
                    isAnimation = false;
                    hasDealtDamage = false;
                    isaction = false;
                    effect = "";
                    EndTurn();
                }
            }
            return;
        }
        if (isAttacking)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(animatorname))
            {
                if (state.normalizedTime >= 0.4f && !hasDealtDamage)
                {
                    audiosound.clip = soundattack;
                    audiosound.Play();
                    DoDamage(Target, pendingDamage, isPhysicalAttack);
                    hasDealtDamage = true;
                }

                if (state.normalizedTime >= 1f)
                {
                    isAttacking = false;
                    isReturning = true;
                }
            }
            return;
        }

        if (isReturning)
        {
            // เดินกลับ
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * 3f * Time.deltaTime);

            // หมุนกลับทิศเดิม
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * 5f);

            // ตรวจสอบว่าถึงตำแหน่งและหมุนกลับทิศเดิมแล้ว โดยใช้ค่า Threshold
            // Vector3.Distance(A, B) < threshold
            // Quaternion.Angle(A, B) < threshold

            bool reachedPosition = Vector3.Distance(transform.position, originalPosition) < positionThreshold;
            bool reachedRotation = Quaternion.Angle(transform.rotation, originalRotation) < rotationThreshold;

            if (reachedPosition && reachedRotation)
            {
                // อาจจะบังคับให้ตำแหน่งและ rotation เป๊ะๆ เป็นครั้งสุดท้ายก่อนจบ
                transform.position = originalPosition;
                transform.rotation = originalRotation;

                isReturning = false;
                isaction = false; // ถ้า isaction คือ flag สำหรับว่ากำลังทำ action อยู่
                EndTurn();
            }
        }
    }

    // ✅ ยิงดาเมจตัวเดียว (เคลียร์ element ทันทีหลังยิง)
    protected virtual void DoDamage(Character target, float damage, bool atk)
    {
        if (target == null) return;

        // จำค่า element ปัจจุบันไว้ แล้วเคลียร์หลังยิงเสร็จ
        string appliedElement = element;

        DealDamageInternal(target, damage, atk, appliedElement);

        // เคลียร์หลังโจมตีเป้าหมายเดี่ยวเสร็จ
        element = "";
    }

    // ✅ ยิงดาเมจแบบ AOE (เคลียร์ element หลังวนครบทุกเป้าหมาย)
    protected virtual void DoAOE(float damage, bool isPhysical)
    {
        // เก็บ element ปัจจุบันไว้ใช้ทั้งลูป แล้วค่อยเคลียร์ตอนท้าย
        string appliedElement = element;

        foreach (Character c in TurnManager.characters)
        {
            if (c.isEnemy && c.IsAlive())
            {
                DealDamageInternal(c, damage, isPhysical, appliedElement);
            }
        }

        // เคลียร์หลังจากครบทุกตัว
        element = "";
    }

    // ▶️ เมธอดภายในที่ทำงานจริง: คิดคริติคอลและส่งต่อให้ TakeDamage พร้อม element ที่กำหนด
    private void DealDamageInternal(Character target, float damage, bool atk, string appliedElement)
    {
        // คิดคริติคอลต่อเป้าหมาย (roll แยกต่อหนึ่งตัว)
        float critChance = Mathf.Clamp01(luck / 40f);
        bool isCrit = Random.value < critChance;
        float finalDamage = isCrit ? damage * 2f : damage;

        target.TakeDamage(Mathf.CeilToInt(finalDamage), atk, appliedElement);
    }

}
