using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static TurnManager;

public class Spawn : MonoBehaviour
{
    string prefabName; // ชื่อ prefab ใน Resources (ไม่ต้องมีนามสกุล)
    int numberToSpawn;
    bool firstplayerplay;
    public float spacing = 3f;

    private Vector3 startPosition;
    private GameObject prefabToSpawn;

    void Awake()
    {
        startPosition = transform.position;
        SpawnCharactersFromSavedData();
        // โหลด prefab จาก Resources
        prefabToSpawn = Resources.Load<GameObject>(prefabName);
        if (prefabToSpawn == null)
        {
            Debug.LogError("ไม่พบ Prefab ชื่อ " + prefabName + " ใน Resources folder");
            return;
        }


        SpawnCharacters();
    }

    void SpawnCharactersFromSavedData() // หรือ SpawnEnemiesBasedOnData()
    {
        // ดึงข้อมูลศัตรูทั้งหมดที่ถูกบันทึกไว้จาก GameDataManager
        List<EnemySpawnData> dataToSpawn = GameDataManager.GetEnemySpawnData();

        if (dataToSpawn.Count == 0)
        {
            Debug.Log("ไม่มีข้อมูลศัตรูใน JSON ที่จะ Spawn.");
            return;
        }

        Debug.Log("--- รายชื่อศัตรูที่โหลดจาก JSON ---");
        foreach (EnemySpawnData enemyData in dataToSpawn)
        {
            // ตรงนี้คือการ print ชื่อและจำนวนที่โหลดมาครับ
            Debug.Log($"ชื่อ: {enemyData.enemyPrefabName}, จำนวน: {enemyData.count}");
            prefabName = enemyData.enemyPrefabName;
            numberToSpawn = enemyData.count;
            firstplayerplay = enemyData.firstplayerplay;

        }
        Debug.Log("-----------------------------------");
    }

    void SpawnCharacters()
    {
        Vector3 direction = Vector3.right;

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 spawnPos = startPosition;

            if (i > 0)
            {
                int offsetIndex = (i + 1) / 2;
                float offsetAmount = spacing * offsetIndex;

                if (i % 2 == 1)
                    spawnPos -= direction * offsetAmount; // ซ้าย
                else
                    spawnPos += direction * offsetAmount; // ขวา
            }

            Quaternion rotation = Quaternion.Euler(0, 180f, 0);
            Instantiate(prefabToSpawn, spawnPos, rotation);
        }

        StartCoroutine(StartBattleNextFrame());
    }

    IEnumerator StartBattleNextFrame()
    {
        yield return null;
        TurnManager.Instance.spawn = true;
        if (firstplayerplay)
        {
            TurnManager.Instance.startMode = StartMode.PlayerFirst;
        }
        else { TurnManager.Instance.startMode = StartMode.EnemyFirst; }
        TurnManager.Instance.StartCoroutine(TurnManager.Instance.InitializeTurnQueue());
    }
}
