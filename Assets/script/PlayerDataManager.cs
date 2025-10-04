using UnityEngine;
using System.Collections.Generic;
using System.Linq; // For FirstOrDefault

public static class PlayerDataManager
{
    // Key สำหรับเก็บข้อมูลผู้เล่นใน PlayerPrefs
    private const string PLAYER_DATA_KEY = "PlayerSaveDataJson";
    // ออบเจกต์เดียวที่จะเก็บข้อมูลผู้เล่นทั้งหมดในหน่วยความจำชั่วคราว
    private static PlayerSaveData currentSaveData;

    // Static constructor: จะถูกเรียกครั้งแรกเมื่อมีการเข้าถึงคลาส PlayerDataManager
    // เพื่อโหลดข้อมูลผู้เล่นล่าสุดจาก PlayerPrefs ทันที
    static PlayerDataManager()
    {
        LoadPlayerDataFromPlayerPrefs();
    }

    // --- เมธอดสำหรับจัดการข้อมูลมอนสเตอร์ที่ถูกกำจัด (ชื่อ Prefab เท่านั้น) ---

    public static void AddDefeatedEnemy(string enemyPrefabName)
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Cannot add defeated enemy.");
            return;
        }

        if (!currentSaveData.defeatedEnemies.Contains(enemyPrefabName))
        {
            currentSaveData.defeatedEnemies.Add(enemyPrefabName);
            Debug.Log($"Added defeated enemy globally: {enemyPrefabName}");
            SavePlayerDataToPlayerPrefs(); // บันทึกทันทีเมื่อมีการเปลี่ยนแปลง
        }
    }

    // ตรวจสอบว่ามอนสเตอร์ชื่อ Prefab นี้เคยถูกกำจัดไปแล้วหรือไม่
    public static bool IsEnemyDefeated(string enemyPrefabName)
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default (false).");
            return false;
        }
        return currentSaveData.defeatedEnemies.Contains(enemyPrefabName);
    }

    // เมธอดสำหรับ "เริ่มเกมใหม่" เพื่อล้างข้อมูลศัตรูที่ถูกกำจัดทั้งหมด
    public static void ClearAllDefeatedEnemies()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Cannot clear defeated enemies.");
            return;
        }
        currentSaveData.defeatedEnemies.Clear();
        Debug.Log("All defeated enemies cleared.");
        SavePlayerDataToPlayerPrefs(); // บันทึกทันทีเมื่อมีการเปลี่ยนแปลง
    }

    // --- เมธอดสำหรับจัดการชื่อมอนสเตอร์ (จากโค้ดเดิมของคุณ) ---
    public static void setnamemon(string name)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot set monster name."); return; }
        currentSaveData.monstername = name;
        SavePlayerDataToPlayerPrefs(); // บันทึกทันทีเมื่อมีการเปลี่ยนแปลง
    }

    public static string getnamemon()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default monster name.");
            return string.Empty; // คืนค่าว่างเปล่าหรือค่าเริ่มต้นที่เหมาะสม
        }
        return currentSaveData.monstername;
    }

    public static void setaiaction(string aiaction)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot set monster name."); return; }
        if (currentSaveData.numaiaction >= 3)
        {
            // ถ้าถึง 2 แล้ว ให้ลบบรรทัดแรกออก
            // ค้นหาตำแหน่งของบรรทัดใหม่แรก
            int firstNewlineIndex = currentSaveData.aiaction.IndexOf('\n');

            if (firstNewlineIndex != -1) // ตรวจสอบว่ามีบรรทัดใหม่หรือไม่
            {
                // ตัดข้อความส่วนที่อยู่หลังบรรทัดใหม่แรกออกไป
                currentSaveData.aiaction = currentSaveData.aiaction.Substring(firstNewlineIndex + 1);
            }
            else
            {
                // ถ้าไม่มีบรรทัดใหม่ (อาจจะมีแค่บรรทัดเดียว) ให้ล้างค่าทิ้งไปเลย
                currentSaveData.aiaction = "";
            }

            // เพิ่ม action ใหม่เข้าไป
            currentSaveData.aiaction += "\n" + aiaction;
            // รีเซ็ตตัวนับ action
            currentSaveData.numaiaction = 3;
        }
        else if (currentSaveData.numaiaction == 0)
        {
            currentSaveData.numaiaction++;
            currentSaveData.aiaction +=  aiaction;
        }
        else
        {
            // ถ้ายังไม่ถึง 2 ก็เพิ่ม action ใหม่และเพิ่มตัวนับ
            currentSaveData.numaiaction++;
            currentSaveData.aiaction += "\n" + aiaction;
        }
        SavePlayerDataToPlayerPrefs(); // บันทึกทันทีเมื่อมีการเปลี่ยนแปลง
    }

    public static string getaiaction()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default monster name.");
            return string.Empty; // คืนค่าว่างเปล่าหรือค่าเริ่มต้นที่เหมาะสม
        }
        return currentSaveData.aiaction;
    }
    public static void Clearaiaction()
    {
        currentSaveData.aiaction = "";
        currentSaveData.numaiaction = 0;
    }
    
    public static void finishquest(int num)
    {
        if (currentSaveData.numquest < num) 
        {
            currentSaveData.numquest = num;
        }
    }

    public static int getnumquest()
    {
        return currentSaveData.numquest;
    }

    public static Vector3 GetplayerStartPos()
    {
        return currentSaveData.playerStartPos;
    }

    public static void SetplayerplayPos(Vector3 Pos)
    {
        currentSaveData.playerplayPos = Pos;
    }

    public static Vector3 GetplayerplayPos()
    {
        return currentSaveData.playerplayPos;
    }

    public static bool GetWinStatus()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default win status (false).");
            return false;
        }
        return currentSaveData.win;
    }
    public static void setmusic(float name)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot set music volume."); return; }
        currentSaveData.musicnum = name; 
        SavePlayerDataToPlayerPrefs();
    }
    public static float getmusic()
    {
        return currentSaveData.musicnum;
    }

    public static void SetWinStatus(bool status)
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Cannot set win status.");
            return;
        }
        currentSaveData.win = status;
        SavePlayerDataToPlayerPrefs(); // บันทึกทันทีเมื่อมีการเปลี่ยนแปลง
    }
    public static bool Getcontinue()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default win status (false).");
            return false;
        }
        return currentSaveData.Continue;
    }
    public static void Setcontinue(bool status)
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Cannot set win status.");
            return;
        }
        currentSaveData.Continue = status;
        SavePlayerDataToPlayerPrefs(); // บันทึกทันทีเมื่อมีการเปลี่ยนแปลง
    }

    // ดึงข้อมูลตัวละครทั้งหมด
    public static List<CharacterStats> GetAllCharacterStats()
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Returning empty character list."); return new List<CharacterStats>(); }
        return currentSaveData.characters;
    }

    // ดึงข้อมูลตัวละครตามชื่อ
    public static CharacterStats GetCharacterStats(string characterName)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot get character stats."); return null; }
        CharacterStats stats = currentSaveData.characters.FirstOrDefault(c => c.characterName == characterName);
        if (stats == null)
        {
            Debug.LogWarning($"Character '{characterName}' not found in PlayerDataManager.");
        }
        return stats;
    }

    // อัปเดตข้อมูลสถิติของตัวละคร
    public static void UpdateCharacterStats(CharacterStats updatedStats)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot update character stats."); return; }

        CharacterStats existingStats = currentSaveData.characters.FirstOrDefault(c => c.characterName == updatedStats.characterName);
        if (existingStats != null)
        {
            currentSaveData.characters.Remove(existingStats);
        }
        currentSaveData.characters.Add(updatedStats);
        SavePlayerDataToPlayerPrefs();
        Debug.Log($"Character '{updatedStats.characterName}' stats updated.");
    }

    // --- เมธอดสำหรับจัดการข้อมูลไอเทม ---

    // ดึงข้อมูลคลังไอเทมทั้งหมด
    public static InventoryData GetInventoryData()
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Returning empty inventory data."); return new InventoryData(); }
        return currentSaveData.inventory;
    }

    // เพิ่มจำนวนไอเทม
    public static void AddItem(string itemName, int amount)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot add item."); return; }
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative amount of items. Use UseItem instead.");
            return;
        }

        switch (itemName.ToLower())
        {
            case "hppotion": currentSaveData.inventory.hpPotion += amount; break;
            case "fullhppotion": currentSaveData.inventory.fullHpPotion += amount; break;
            case "manapotion": currentSaveData.inventory.manaPotion += amount; break;
            case "fullmanapotion": currentSaveData.inventory.fullManaPotion += amount; break;
            case "phoenixfeather": currentSaveData.inventory.phoenixFeather += amount; break;
            default:
                Debug.LogWarning($"Item '{itemName}' not recognized by PlayerDataManager.");
                return;
        }
        SavePlayerDataToPlayerPrefs();
        Debug.Log($"Added {amount} of {itemName}. Current count: {GetItemCount(itemName)}");
    }

    // ใช้/ลดจำนวนไอเทม
    public static bool UseItem(string itemName, int amount = 1)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot use item."); return false; }
        if (amount <= 0)
        {
            Debug.LogWarning("Cannot use zero or negative amount of items.");
            return false;
        }

        bool canUse = false;
        switch (itemName.ToLower())
        {
            case "hppotion":
                if (currentSaveData.inventory.hpPotion >= amount) { currentSaveData.inventory.hpPotion -= amount; canUse = true; }
                break;
            case "fullhppotion":
                if (currentSaveData.inventory.fullHpPotion >= amount) { currentSaveData.inventory.fullHpPotion -= amount; canUse = true; }
                break;
            case "manapotion":
                if (currentSaveData.inventory.manaPotion >= amount) { currentSaveData.inventory.manaPotion -= amount; canUse = true; }
                break;
            case "fullmanapotion":
                if (currentSaveData.inventory.fullManaPotion >= amount) { currentSaveData.inventory.fullManaPotion -= amount; canUse = true; }
                break;
            case "phoenixfeather":
                if (currentSaveData.inventory.phoenixFeather >= amount) { currentSaveData.inventory.phoenixFeather -= amount; canUse = true; }
                break;
            default:
                Debug.LogWarning($"Item '{itemName}' not recognized by PlayerDataManager.");
                return false;
        }

        if (canUse)
        {
            SavePlayerDataToPlayerPrefs();
            Debug.Log($"Used {amount} of {itemName}. Remaining: {GetItemCount(itemName)}");
        }
        else
        {
            Debug.LogWarning($"Not enough {itemName} to use {amount}. Current: {GetItemCount(itemName)}");
        }
        return canUse;
    }

    // ดึงจำนวนไอเทมที่เหลืออยู่
    public static int GetItemCount(string itemName)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Returning 0 for item count."); return 0; }
        switch (itemName.ToLower())
        {
            case "hppotion": return currentSaveData.inventory.hpPotion;
            case "fullhppotion": return currentSaveData.inventory.fullHpPotion;
            case "manapotion": return currentSaveData.inventory.manaPotion;
            case "fullmanapotion": return currentSaveData.inventory.fullManaPotion;
            case "phoenixfeather": return currentSaveData.inventory.phoenixFeather;
            default:
                Debug.LogWarning($"Item '{itemName}' not recognized by PlayerDataManager. Returning 0.");
                return 0;
        }
    }
    public static void ClearAllItems()
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot clear items."); return; }
        currentSaveData.inventory = new InventoryData();
        SavePlayerDataToPlayerPrefs();
        Debug.Log("All items cleared.");
    }

    // --- เมธอดสำหรับบันทึกและโหลดข้อมูลหลักของ PlayerData ---

    public static void SavePlayerDataToPlayerPrefs()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("Cannot save data: currentSaveData is null!");
            return;
        }
        string json = JsonUtility.ToJson(currentSaveData);
        PlayerPrefs.SetString(PLAYER_DATA_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("Player data saved to PlayerPrefs.");
    }

    private static void LoadPlayerDataFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(PLAYER_DATA_KEY))
        {
            string json = PlayerPrefs.GetString(PLAYER_DATA_KEY);
            currentSaveData = JsonUtility.FromJson<PlayerSaveData>(json);
            Debug.Log("Player data loaded from PlayerPrefs.");
        }
        else
        {
            currentSaveData = new PlayerSaveData(); // สร้างข้อมูลใหม่ให้ currentSaveData ถ้าไม่พบไฟล์เซฟ
            Debug.LogWarning("No player data found in PlayerPrefs. Starting with new save data.");
        }
        // ตรวจสอบขั้นสุดท้าย: ถ้า currentSaveData ยังเป็น null อยู่ แสดงว่ามีปัญหาใหญ่
        if (currentSaveData == null)
        {
            Debug.LogError("FATAL ERROR: currentSaveData is still null after LoadPlayerDataFromPlayerPrefs! This should not happen.");
            // ในสถานการณ์จริง คุณอาจจะโยน exception หรือบังคับปิดเกมที่นี่
        }
    }

    public static void ClearAllPlayerData()
    {
        PlayerPrefs.DeleteKey(PLAYER_DATA_KEY);
        currentSaveData = new PlayerSaveData(); // สร้างข้อมูลใหม่ให้ currentSaveData หลังจากล้าง
        PlayerPrefs.Save();
        Debug.Log("All player data cleared from PlayerPrefs.");
    }
}