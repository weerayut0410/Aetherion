using UnityEngine;
using System.Collections.Generic;
using System.Linq; // For FirstOrDefault

public static class PlayerDataManager
{
    // Key ����Ѻ�红����ż������ PlayerPrefs
    private const string PLAYER_DATA_KEY = "PlayerSaveDataJson";
    // �ͺਡ�����Ƿ����红����ż����蹷������˹��¤����Ӫ��Ǥ���
    private static PlayerSaveData currentSaveData;

    // Static constructor: �ж١���¡�����á������ա����Ҷ֧���� PlayerDataManager
    // ������Ŵ�����ż���������ش�ҡ PlayerPrefs �ѹ��
    static PlayerDataManager()
    {
        LoadPlayerDataFromPlayerPrefs();
    }

    // --- ���ʹ����Ѻ�Ѵ��â������͹�������١�ӨѴ (���� Prefab ��ҹ��) ---

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
            SavePlayerDataToPlayerPrefs(); // �ѹ�֡�ѹ��������ա������¹�ŧ
        }
    }

    // ��Ǩ�ͺ����͹�������� Prefab ����¶١�ӨѴ������������
    public static bool IsEnemyDefeated(string enemyPrefabName)
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default (false).");
            return false;
        }
        return currentSaveData.defeatedEnemies.Contains(enemyPrefabName);
    }

    // ���ʹ����Ѻ "�����������" ������ҧ�������ѵ�ٷ��١�ӨѴ������
    public static void ClearAllDefeatedEnemies()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Cannot clear defeated enemies.");
            return;
        }
        currentSaveData.defeatedEnemies.Clear();
        Debug.Log("All defeated enemies cleared.");
        SavePlayerDataToPlayerPrefs(); // �ѹ�֡�ѹ��������ա������¹�ŧ
    }

    // --- ���ʹ����Ѻ�Ѵ��ê����͹����� (�ҡ������ͧ�س) ---
    public static void setnamemon(string name)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot set monster name."); return; }
        currentSaveData.monstername = name;
        SavePlayerDataToPlayerPrefs(); // �ѹ�֡�ѹ��������ա������¹�ŧ
    }

    public static string getnamemon()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default monster name.");
            return string.Empty; // �׹�����ҧ�������ͤ��������鹷���������
        }
        return currentSaveData.monstername;
    }

    public static void setaiaction(string aiaction)
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Cannot set monster name."); return; }
        if (currentSaveData.numaiaction >= 3)
        {
            // ��Ҷ֧ 2 ���� ���ź��÷Ѵ�á�͡
            // ���ҵ��˹觢ͧ��÷Ѵ�����á
            int firstNewlineIndex = currentSaveData.aiaction.IndexOf('\n');

            if (firstNewlineIndex != -1) // ��Ǩ�ͺ����պ�÷Ѵ�����������
            {
                // �Ѵ��ͤ�����ǹ���������ѧ��÷Ѵ�����á�͡�
                currentSaveData.aiaction = currentSaveData.aiaction.Substring(firstNewlineIndex + 1);
            }
            else
            {
                // �������պ�÷Ѵ���� (�Ҩ�������÷Ѵ����) �����ҧ��ҷ������
                currentSaveData.aiaction = "";
            }

            // ���� action ��������
            currentSaveData.aiaction += "\n" + aiaction;
            // ���絵�ǹѺ action
            currentSaveData.numaiaction = 3;
        }
        else if (currentSaveData.numaiaction == 0)
        {
            currentSaveData.numaiaction++;
            currentSaveData.aiaction +=  aiaction;
        }
        else
        {
            // ����ѧ���֧ 2 ������ action �������������ǹѺ
            currentSaveData.numaiaction++;
            currentSaveData.aiaction += "\n" + aiaction;
        }
        SavePlayerDataToPlayerPrefs(); // �ѹ�֡�ѹ��������ա������¹�ŧ
    }

    public static string getaiaction()
    {
        if (currentSaveData == null)
        {
            Debug.LogError("PlayerDataManager is not initialized! Returning default monster name.");
            return string.Empty; // �׹�����ҧ�������ͤ��������鹷���������
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
        SavePlayerDataToPlayerPrefs(); // �ѹ�֡�ѹ��������ա������¹�ŧ
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
        SavePlayerDataToPlayerPrefs(); // �ѹ�֡�ѹ��������ա������¹�ŧ
    }

    // �֧�����ŵ���Ф÷�����
    public static List<CharacterStats> GetAllCharacterStats()
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Returning empty character list."); return new List<CharacterStats>(); }
        return currentSaveData.characters;
    }

    // �֧�����ŵ���Фõ������
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

    // �ѻവ������ʶԵԢͧ����Ф�
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

    // --- ���ʹ����Ѻ�Ѵ��â��������� ---

    // �֧�����Ť�ѧ����������
    public static InventoryData GetInventoryData()
    {
        if (currentSaveData == null) { Debug.LogError("PlayerDataManager is not initialized! Returning empty inventory data."); return new InventoryData(); }
        return currentSaveData.inventory;
    }

    // �����ӹǹ����
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

    // ��/Ŵ�ӹǹ����
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

    // �֧�ӹǹ����������������
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

    // --- ���ʹ����Ѻ�ѹ�֡�����Ŵ��������ѡ�ͧ PlayerData ---

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
            currentSaveData = new PlayerSaveData(); // ���ҧ������������� currentSaveData �����辺���૿
            Debug.LogWarning("No player data found in PlayerPrefs. Starting with new save data.");
        }
        // ��Ǩ�ͺ����ش����: ��� currentSaveData �ѧ�� null ���� �ʴ�����ջѭ���˭�
        if (currentSaveData == null)
        {
            Debug.LogError("FATAL ERROR: currentSaveData is still null after LoadPlayerDataFromPlayerPrefs! This should not happen.");
            // �ʶҹ��ó��ԧ �س�Ҩ���¹ exception ���ͺѧ�Ѻ�Դ�������
        }
    }

    public static void ClearAllPlayerData()
    {
        PlayerPrefs.DeleteKey(PLAYER_DATA_KEY);
        currentSaveData = new PlayerSaveData(); // ���ҧ������������� currentSaveData ��ѧ�ҡ��ҧ
        PlayerPrefs.Save();
        Debug.Log("All player data cleared from PlayerPrefs.");
    }
}