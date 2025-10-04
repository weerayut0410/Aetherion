using Unity.VisualScripting;
using UnityEngine;

public class Golem : Character
{
    public int intdef;
    public int intres;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShadowBeast.spawnedGolems.Add(this.gameObject);
        TurnManager.characters = FindObjectsByType<Character>(FindObjectsSortMode.None);
        fullhealth = 5;
        health = fullhealth;
        def = intdef;
        basedef = def;
        res = intres;
        baseres = res;
    }

    public override void TakeTurn()
    {
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
}
