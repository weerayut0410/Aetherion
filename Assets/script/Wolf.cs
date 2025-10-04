using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.Collections.AllocatorManager;

public class Wolf : Character
{
    private bool waitingForIdle = false;
    private CameraSwitcher cameraSwitcher;
    private void Start()
    {
        cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene!");
        }
        
        fullhealth = 150;
        health = fullhealth;
        fullmagicpoint = 20;
        magicpoint = 0;
        atk = 26;
        baseatk = atk;
        Int = 0;
        baseInt = Int;
        def = 9;
        basedef = def;
        res = 10;
        baseres = res;
        speed = 19;
        basespeed = speed;
        luck = 10;
        weaknesses = new List<string> { "Fire" };
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
            if (magicpoint >= 20)
            {
                Skill();
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
            animatorname = "attack";
            StartMoveToTarget(Target);
        }

    }
    private void Skill()
    {

        Character target = FindRandomPlayer();
        magicpoint -= 20;
        if (target != null)
        {
            var bleedPrefab = Resources.Load<Bleeding>("StatusEffects/Bleeding");
            

            TurnManager.Instance.statusEffectManager.AddEffect(target, bleedPrefab, this);
            Target = target;
            pendingDamage = atk * 2;
            isPhysicalAttack = true;
            animatorname = "attack";
            StartMoveToTarget(Target);
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
}
