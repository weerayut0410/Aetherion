using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static TurnManager;

public class Spawn : MonoBehaviour
{
    string prefabName; // ���� prefab � Resources (����ͧ�չ��ʡ��)
    int numberToSpawn;
    bool firstplayerplay;
    public float spacing = 3f;

    private Vector3 startPosition;
    private GameObject prefabToSpawn;

    void Awake()
    {
        startPosition = transform.position;
        SpawnCharactersFromSavedData();
        // ��Ŵ prefab �ҡ Resources
        prefabToSpawn = Resources.Load<GameObject>(prefabName);
        if (prefabToSpawn == null)
        {
            Debug.LogError("��辺 Prefab ���� " + prefabName + " � Resources folder");
            return;
        }


        SpawnCharacters();
    }

    void SpawnCharactersFromSavedData() // ���� SpawnEnemiesBasedOnData()
    {
        // �֧�������ѵ�ٷ��������١�ѹ�֡���ҡ GameDataManager
        List<EnemySpawnData> dataToSpawn = GameDataManager.GetEnemySpawnData();

        if (dataToSpawn.Count == 0)
        {
            Debug.Log("����բ������ѵ��� JSON ���� Spawn.");
            return;
        }

        Debug.Log("--- ��ª����ѵ�ٷ����Ŵ�ҡ JSON ---");
        foreach (EnemySpawnData enemyData in dataToSpawn)
        {
            // �ç����͡�� print ������Шӹǹ�����Ŵ�Ҥ�Ѻ
            Debug.Log($"����: {enemyData.enemyPrefabName}, �ӹǹ: {enemyData.count}");
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
                    spawnPos -= direction * offsetAmount; // ����
                else
                    spawnPos += direction * offsetAmount; // ���
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
