using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        fullmagicpoint = 30;
        magicpoint = 0;
        atk = 5;
        baseatk = atk;
        Int = 0;
        baseInt = Int;
        def = 8;
        basedef = def;
        res = 8;
        baseres = res;
        speed = 11;
        basespeed = speed;
        luck = 10;
        weaknesses = new List<string> { "Fire" };
    }

    protected override void Update()
    {
        base.Update();
        hpbar.fillAmount = (float)health / fullhealth;
        mpbar.fillAmount = (float)magicpoint / fullmagicpoint;

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
            if (magicpoint >= 30)
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
            target.posion += 1;
            animatorname = "attack";
            StartMoveToTarget(Target);
        }

    }
    private void Skill()
    {

        Character target = FindRandomPlayer();
        magicpoint -= 30;
        if (target != null)
        {

            Target = target;
            isPhysicalAttack = false;
            element = "";
            animatorname = "attack";
            if (target.posion != 0) 
            {
                pendingDamage = Int * 0.5f * target.posion;
                target.posion = 0;
                effect = "dark";
                animator.SetTrigger(animatorname);
                animatoinplay = 1;
                isAnimation = true;
            }
            else { pendingDamage = Int * 0.5f; StartMoveToTarget(Target); }
            
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
