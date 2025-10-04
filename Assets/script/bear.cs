using System.Collections.Generic;
using UnityEngine;

public class Bear : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool waitingForIdle = false;
    private CameraSwitcher cameraSwitcher;
    public string elementname;
    public string weaknessesname;
    public bool iceBear = false;
    private void Start()
    {

        cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene!");
        }

        fullhealth = 200;
        health = fullhealth;
        fullmagicpoint = 10;
        magicpoint = 0;
        atk = 5;
        baseatk = atk;
        baseInt = Int;
        def = 11;
        basedef = def;
        res = 12;
        baseres = res;
        basespeed = speed;
        luck = 10;
        weaknesses = new List<string> { "weaknessesname" };
    }

    protected override void Update()
    {
        base.Update();
        hpbar.fillAmount = (float)health / fullhealth;
        mpbar.fillAmount = (float)magicpoint / fullmagicpoint;

        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Die") && state.normalizedTime >= 0.9f)
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
            if (magicpoint >= 10)
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
        magicpoint -= 10;
        if (target != null)
        {

            Target = target;
            pendingDamage = Int * 1.5f;
            isPhysicalAttack = false;
            element = elementname;
            animatorname = "attack";
            if (iceBear)
            {
                target.weaknesses = new List<string> { "Fire" };
            }
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
                else if (c.weaknesses.Contains(elementname))
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
