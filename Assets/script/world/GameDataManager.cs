using UnityEngine;
using System.Collections.Generic;
using System.Linq; // ����Ѻ .FirstOrDefault()

public static class GameDataManager
{
    private const string ENEMY_DATA_KEY = "EnemySpawnDataJson"; // Key ����Ѻ��� PlayerPrefs

    // �������ѵ�ٷ��١��ⴹ㹻Ѩ�غѹ (�ж١���˹��¤����Ӫ��Ǥ���)
    private static EnemySpawnList currentEnemySpawnList = new EnemySpawnList();

    // ���ʹ����Ѻ���������ѻവ�ӹǹ�ѵ�ٷ��١��ⴹ
    public static void AddOrUpdateEnemyHit(string enemyPrefabName, int hitCount,bool firstplayerplay)
    {
        // ��Ŵ����������ش�ҡ PlayerPrefs �ء���駷�����¡�� ������������Ң������繻Ѩ�غѹ
        LoadEnemyDataFromPlayerPrefs();

        EnemySpawnData existingData = currentEnemySpawnList.enemies
            .FirstOrDefault(e => e.enemyPrefabName == enemyPrefabName);

        if (existingData != null)
        {
            existingData.count += hitCount; // ������������ǡ������ӹǹ
            Debug.Log($"Updated: {enemyPrefabName} count to {existingData.count}");
        }
        else
        {
            currentEnemySpawnList.enemies.Add(new EnemySpawnData(enemyPrefabName, hitCount, firstplayerplay)); // ����ѧ����ա���������
            Debug.Log($"Added new enemy: {enemyPrefabName} with count {hitCount}");
        }

        // �ѹ�֡�����ŷ���ѻവŧ PlayerPrefs �ѹ��
        SaveEnemyDataToPlayerPrefs();
    }

    // ���ʹ����Ѻ�֧�������ѵ�ٷ��������١��ⴹ
    public static List<EnemySpawnData> GetEnemySpawnData()
    {
        LoadEnemyDataFromPlayerPrefs(); // ��Ŵ����������ش��͹�觤׹
        return currentEnemySpawnList.enemies;
    }

    // ���ʹ����Ѻ�ѹ�֡������ EnemySpawnList ŧ PlayerPrefs
    private static void SaveEnemyDataToPlayerPrefs()
    {
        string json = JsonUtility.ToJson(currentEnemySpawnList);
        PlayerPrefs.SetString(ENEMY_DATA_KEY, json);
        PlayerPrefs.Save(); // �ѹ�֡�������¹�ŧ�ѹ�� (�ҧ���������繵�ͧ���¡��� Unity ���ѧ�лԴ)
        Debug.Log("Enemy data saved to PlayerPrefs.");
    }

    // ���ʹ����Ѻ��Ŵ������ EnemySpawnList �ҡ PlayerPrefs
    private static void LoadEnemyDataFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(ENEMY_DATA_KEY))
        {
            string json = PlayerPrefs.GetString(ENEMY_DATA_KEY);
            currentEnemySpawnList = JsonUtility.FromJson<EnemySpawnList>(json);
            Debug.Log("Enemy data loaded from PlayerPrefs.");
        }
        else
        {
            currentEnemySpawnList = new EnemySpawnList(); // ����������� ������ҧ PlayerList ����
            Debug.LogWarning("No enemy data found in PlayerPrefs. Starting with empty list.");
        }
    }

    // ���ʹ����Ѻ��ҧ�����ŷ����� (�ջ���ª������Ѻ��÷��ͺ���������������)
    public static void ClearEnemyData()
    {
        PlayerPrefs.DeleteKey(ENEMY_DATA_KEY);
        currentEnemySpawnList = new EnemySpawnList();
        PlayerPrefs.Save();
        Debug.Log("Enemy data cleared from PlayerPrefs.");
    }
}