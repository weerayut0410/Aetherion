using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Golem : Character
{
    public int intdef;
    public int intres;
    public string weaknessesname;
    private CameraSwitcher cameraSwitcher;
    TurnManager turnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShadowBeast.spawnedGolems.Add(this.gameObject);
        TurnManager.characters = FindObjectsByType<Character>(FindObjectsSortMode.None);
        turnManager = FindFirstObjectByType<TurnManager>();
        turnManager.enemyteam.Add(this);
        cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene!");
        }
        fullhealth = 5;
        health = fullhealth;
        def = intdef;
        basedef = def;
        res = intres;
        baseres = res;
        speed = 5;
        basespeed = speed;
        weaknesses = new List<string> { weaknessesname };
    }

    public override void TakeTurn()
    {
        showplayer(0);
        if (cameraSwitcher != null)
        {
            cameraSwitcher.SwitchCamera(cameraSwitcher.enemyVcam); // เรียกฟังก์ชัน SwitchCamera ของ CameraSwitcher
        }
        StartCoroutine(slow1f());
        EndTurn();
    }
    protected override void Update()
    {
        if (health <= 0)
        {
            ShadowBeast.spawnedGolems.Remove(this.gameObject);
            Destroy(gameObject);
        }
    }
    IEnumerator slow1f()
    {
        yield return new WaitForSeconds(1);
    }
}
