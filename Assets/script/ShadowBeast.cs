using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ShadowBeast : Character
{
    private bool waitingForIdle = false;
    private int DarkHowlTurnCount = 0;
    TurnManager turnManager;
    public GameObject objattack1;
    public GameObject objattack2;
    private CameraSwitcher cameraSwitcher;

    string prefabName;
    private GameObject prefabToSpawn;

    bool golem = false;
    public static List<GameObject> spawnedGolems = new List<GameObject>();
    public GameObject weakimage;

    protected override void Awake()
    {
        base.Awake();
        turnManager = FindFirstObjectByType<TurnManager>();
        cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene!");
        }
        ShadowBeast.spawnedGolems.Clear();
        weakimage.SetActive(false);
    }

    private void Start()
    {
        fullhealth = 700;
        health = fullhealth;
        fullmagicpoint = 20;
        magicpoint = 0;
        atk = 30;
        baseatk = atk;
        Int = 25;
        baseInt = Int;
        def = 15;
        basedef = def;
        res = 15;
        baseres = res;
        speed = 14;
        basespeed = speed;
        luck = 10;
        DarkHowlTurnCount = 0;
        objattack1.SetActive(false);
        objattack2.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();

        hpbar.fillAmount = (float)health / fullhealth;
        mpbar.fillAmount = (float)magicpoint / fullmagicpoint;

        if (!IsAlive())
        {
            Destroy(gameObject);
            return;
        }

        if (spawnedGolems.Count == 0 && golem == true)
        {
            bossdown();
        }

        if (waitingForIdle)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Idle"))
            {
                waitingForIdle = false;
                TakeTurn(); // เริ่มเทิร์นหลัง Idle
            }
        }
    }
    public override void TakeTurn()
    {
        base.TakeTurn();
        showplayer(0);
        if (cameraSwitcher != null)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.enemyVcam); // เรียกฟังก์ชัน SwitchCamera ของ CameraSwitcher
        }


        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Idle"))
        {
            DarkHowlTurnCount += 1;

            if (DarkHowlTurnCount > 2 && spawnedGolems.Count == 0)
            {
                DarkHowlTurnCount = 0;
                objattack1.SetActive(false);
                objattack2.SetActive(false);
                EndTurn();
                return;
            }
            if(DarkHowlTurnCount == 1) 
            {
                weakimage.SetActive(false);
                weaknesses = new List<string> { "" };
                golem = false;
            }
            else if (DarkHowlTurnCount == 2)
            {
                objattack1.SetActive(true);
                spawngolem();
            }
            else if (DarkHowlTurnCount == 3) 
            { 
                objattack2.SetActive(true);
            }

            if (magicpoint >= 20 && DarkHowlTurnCount != 4)
            {
                Skill();
            }
            else if (DarkHowlTurnCount >= 4)
            {
                objattack1.SetActive(false);
                objattack2.SetActive(false);
                DestroyAllGolems();
                DarkHowl();
            }

            else
            {
                EnemyRoutine();
            }
        }
        else
        {
            waitingForIdle = true; // ถ้ายังไม่ Idle ให้รอใน Update
        }
    }

    private void EnemyRoutine()
    {

        Character target = FindRandomPlayer();

        if (target != null)
        {
            Target = target;
            magicpoint += 10;
            pendingDamage = atk;
            isPhysicalAttack = true;
            animatorname = "attack1";
            StartMoveToTarget(Target);
        }

    }
    private void DarkHowl()
    {

        Character target = FindRandomPlayer();
        DarkHowlTurnCount = 0;
        if (target != null)
        {
            pendingDamage = Int * 1.5f;
            isPhysicalAttack = false;
            var DarkHowlEffectPrefab = Resources.Load<DarkHowl>("StatusEffects/DarkHowl");
            foreach (Character c in TurnManager.characters)
            {
                if (!c.isEnemy && c.IsAlive())
                {
                    TurnManager.Instance.statusEffectManager.AddEffect(c, DarkHowlEffectPrefab);
                    DoDamage(c, pendingDamage, isPhysicalAttack);

                }
            }
            animatorname = "attack2";
            animator.SetTrigger(animatorname);
            animatoinplay = 0;
            isAnimation = true;

            
        }
    }

    private void Skill()
    {

        Character target = FindRandomPlayer();
        magicpoint -= 20;
        if (target != null)
        {
            var crushEffectPrefab = Resources.Load<crush>("StatusEffects/crush");

            TurnManager.Instance.statusEffectManager.AddEffect(target, crushEffectPrefab);
            Target = target;
            pendingDamage = atk * 2f;
            isPhysicalAttack = true;
            animatorname = "attack2";
            effect = "dark";
            animator.SetTrigger(animatorname);
            animatoinplay = 1;
            isAnimation = true;
        }
    }

    private Character FindRandomPlayer()
    {
        Character[] all = FindObjectsByType<Character>(FindObjectsSortMode.None);
        List<Character> players = new List<Character>();

        foreach (Character c in all)
        {
            if (!c.isEnemy && c.IsAlive())
            {
                if (c.taunt)
                {
                    return c;
                }
                players.Add(c);
            }
        }

        if (players.Count > 0)
            return players[Random.Range(0, players.Count)];
        else
            return null;
    }

    private void bossdown()
    {
        weakimage.SetActive(true);
        weaknesses = new List<string> { "Fire", "Ice", "Physical" };
        golem = false;
        print(1);
        
    }

    private void spawngolem()
    {
        Vector3 direction = Vector3.right;

        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPos = transform.position;

            float offsetAmount = 2.5f; // ระยะห่างจากบอส
            if (i == 0) spawnPos -= direction * offsetAmount; // ซ้าย
            else spawnPos += direction * offsetAmount;        // ขวา
            int randomInt = Random.Range(0, 2);
            if (randomInt == 0)
            {
                prefabName = "golematk";
            }
            else { prefabName = "golemint"; }

            prefabToSpawn = Resources.Load<GameObject>(prefabName);
            Quaternion rotation = Quaternion.Euler(0, 180f, 0);

            Instantiate(prefabToSpawn, spawnPos, rotation);

            golem = true;
        }
    }

    private void DestroyAllGolems()
    {
        foreach (GameObject g in spawnedGolems)
        {
            if (g != null)
                Destroy(g);
        }

        // ล้าง list เพื่อไม่ให้เก็บ reference เก่าๆ
        spawnedGolems.Clear();
        golem = false;
    }

}