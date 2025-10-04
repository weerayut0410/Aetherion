using System.Collections.Generic;
using UnityEngine;

public class Smallmushroom : Character
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
        fullhealth = 120;
        health = fullhealth;
        atk = 5;
        baseatk = atk;
        Int = 0;
        baseInt = Int;
        def = 7;
        basedef = def;
        res = 5;
        baseres = res;
        speed = 13;
        basespeed = speed;
        luck = 10;
        animator = GetComponentInChildren<Animator>();
        weaknesses = new List<string> { "Fire" };
    }
    protected override void Update()
    {
        base.Update();
        distance = 1.5f;
        hpbar.fillAmount = (float)health / fullhealth;

        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (!IsAlive())
        {
            Destroy(gameObject);
            return;
        }

        if (waitingForIdle)
        {
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
            EnemyRoutine();
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
            pendingDamage = atk;
            isPhysicalAttack = true;
            animatorname = "attack";
            target.posion += 1;
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