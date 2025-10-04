using UnityEngine;
using System.Collections.Generic;
using System.Linq; // สำหรับ .FirstOrDefault()

public static class GameDataManager
{
    private const string ENEMY_DATA_KEY = "EnemySpawnDataJson"; // Key สำหรับเก็บใน PlayerPrefs

    // ข้อมูลศัตรูที่ถูกตีโดนในปัจจุบัน (จะถูกเก็บในหน่วยความจำชั่วคราว)
    private static EnemySpawnList currentEnemySpawnList = new EnemySpawnList();

    // เมธอดสำหรับเพิ่มหรืออัปเดตจำนวนศัตรูที่ถูกตีโดน
    public static void AddOrUpdateEnemyHit(string enemyPrefabName, int hitCount,bool firstplayerplay)
    {
        // โหลดข้อมูลล่าสุดจาก PlayerPrefs ทุกครั้งที่เรียกใช้ เพื่อให้แน่ใจว่าข้อมูลเป็นปัจจุบัน
        LoadEnemyDataFromPlayerPrefs();

        EnemySpawnData existingData = currentEnemySpawnList.enemies
            .FirstOrDefault(e => e.enemyPrefabName == enemyPrefabName);

        if (existingData != null)
        {
            existingData.count += hitCount; // ถ้ามีอยู่แล้วก็เพิ่มจำนวน
            Debug.Log($"Updated: {enemyPrefabName} count to {existingData.count}");
        }
        else
        {
            currentEnemySpawnList.enemies.Add(new EnemySpawnData(enemyPrefabName, hitCount, firstplayerplay)); // ถ้ายังไม่มีก็เพิ่มใหม่
            Debug.Log($"Added new enemy: {enemyPrefabName} with count {hitCount}");
        }

        // บันทึกข้อมูลที่อัปเดตลง PlayerPrefs ทันที
        SaveEnemyDataToPlayerPrefs();
    }

    // เมธอดสำหรับดึงข้อมูลศัตรูทั้งหมดที่ถูกตีโดน
    public static List<EnemySpawnData> GetEnemySpawnData()
    {
        LoadEnemyDataFromPlayerPrefs(); // โหลดข้อมูลล่าสุดก่อนส่งคืน
        return currentEnemySpawnList.enemies;
    }

    // เมธอดสำหรับบันทึกข้อมูล EnemySpawnList ลง PlayerPrefs
    private static void SaveEnemyDataToPlayerPrefs()
    {
        string json = JsonUtility.ToJson(currentEnemySpawnList);
        PlayerPrefs.SetString(ENEMY_DATA_KEY, json);
        PlayerPrefs.Save(); // บันทึกการเปลี่ยนแปลงทันที (บางครั้งไม่จำเป็นต้องเรียกถ้า Unity กำลังจะปิด)
        Debug.Log("Enemy data saved to PlayerPrefs.");
    }

    // เมธอดสำหรับโหลดข้อมูล EnemySpawnList จาก PlayerPrefs
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
            currentEnemySpawnList = new EnemySpawnList(); // ถ้าไฟล์ไม่มี ให้สร้าง PlayerList ใหม่
            Debug.LogWarning("No enemy data found in PlayerPrefs. Starting with empty list.");
        }
    }

    // เมธอดสำหรับล้างข้อมูลทั้งหมด (มีประโยชน์สำหรับการทดสอบหรือเริ่มเกมใหม่)
    public static void ClearEnemyData()
    {
        PlayerPrefs.DeleteKey(ENEMY_DATA_KEY);
        currentEnemySpawnList = new EnemySpawnList();
        PlayerPrefs.Save();
        Debug.Log("Enemy data cleared from PlayerPrefs.");
    }
}