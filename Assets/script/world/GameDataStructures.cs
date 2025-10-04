using System.Collections.Generic;
using UnityEngine; // จำเป็นสำหรับ [System.Serializable]

// โครงสร้างข้อมูลสำหรับศัตรูแต่ละประเภทที่ถูกตีโดน
[System.Serializable]
public class EnemySpawnData
{
    public string enemyPrefabName; // ชื่อ Prefab ของศัตรู
    public int count;              // จำนวนที่ถูกตีโดน/ต้องการ Spawn
    public bool firstplayerplay;

    public EnemySpawnData(string name, int num, bool firstplayerplay)
    {
        enemyPrefabName = name;
        count = num;
        this.firstplayerplay = firstplayerplay;
    }
}

// โครงสร้างข้อมูลสำหรับเก็บลิสต์ของ EnemySpawnData
[System.Serializable]
public class EnemySpawnList
{
    public List<EnemySpawnData> enemies = new List<EnemySpawnData>();
}


// --- โครงสร้างข้อมูลสำหรับตัวละคร (ใหม่) ---
[System.Serializable]
public class CharacterStats
{
    public string characterName;

    public int baseHealth;
    public int baseMagicPoint;
    public int baseAttack;
    public int baseIntelligence;
    public int baseDefense;
    public int baseResistance;
    public int baseSpeed;
    public int baseLuck;

    public int currentHealth;
    public int currentMagicPoint;
    public int currentAttack;
    public int currentIntelligence;
    public int currentDefense;
    public int currentResistance;
    public int currentSpeed;
    public int currentLuck;

    public bool rulebase;
    public bool weightscore;

    public CharacterStats(string name)
    {
        characterName = name;
        baseHealth = 100; currentHealth = 100;
        baseMagicPoint = 50; currentMagicPoint = 50;
        baseAttack = 10; currentAttack = 10;
        baseIntelligence = 8; currentIntelligence = 8;
        baseDefense = 5; currentDefense = 5;
        baseResistance = 5; currentResistance = 5;
        baseSpeed = 7; currentSpeed = 7;
        baseLuck = 3; currentLuck = 3;
        rulebase = false;
        weightscore = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(baseHealth, currentHealth + amount);
    }
}

// --- โครงสร้างข้อมูลสำหรับไอเทมในคลัง (ใหม่) ---
[System.Serializable]
public class InventoryData
{
    public int hpPotion;
    public int fullHpPotion;
    public int manaPotion;
    public int fullManaPotion;
    public int phoenixFeather;

    public InventoryData()
    {
        hpPotion = 0;
        fullHpPotion = 0;
        manaPotion = 0;
        fullManaPotion = 0;
        phoenixFeather = 0;
    }
}

// --- โครงสร้างข้อมูลรวมสำหรับผู้เล่น (ใหม่) ---
[System.Serializable]
public class PlayerSaveData
{
    public List<CharacterStats> characters = new List<CharacterStats>();
    public InventoryData inventory = new InventoryData();

    public bool win;
    public bool Continue;
    public string monstername;
    public float musicnum = 0.5f;
    public string aiaction;
    public int numaiaction = 0;
    public Vector3 playerStartPos = new Vector3(53.20f, 3.00f, 63.09f);
    public Vector3 playerplayPos = Vector3.zero;
    public int numquest;

    // เก็บชื่อ Prefab ของมอนสเตอร์ที่ถูกกำจัด "ทั้งหมด" (ไม่สนใจ Scene)
    public List<string> defeatedEnemies = new List<string>();

    public PlayerSaveData()
    {
        // --- กำหนดค่าเริ่มต้นสำหรับ Warrior ---
        CharacterStats Warrior = new CharacterStats("Warrior");
        Warrior.baseHealth = 150; // พลังชีวิตพื้นฐานของ Warrior
        Warrior.currentHealth = 150; // พลังชีวิตปัจจุบันเริ่มต้นเท่ากับพื้นฐาน
        Warrior.baseMagicPoint = 50; // ค่าเวทมนตร์พื้นฐาน
        Warrior.currentMagicPoint = 50;
        Warrior.baseAttack = 20; // พลังโจมตี
        Warrior.currentAttack = 20;
        Warrior.baseIntelligence = 5; // สติปัญญา
        Warrior.currentIntelligence = 5;
        Warrior.baseDefense = 15; // ป้องกัน
        Warrior.currentDefense = 15;
        Warrior.baseResistance = 8; // ต้านทานเวท
        Warrior.currentResistance = 8;
        Warrior.baseSpeed = 10; // ความเร็ว
        Warrior.currentSpeed = 10;
        Warrior.baseLuck = 10; // โชค
        Warrior.currentLuck = 10;
        Warrior.rulebase = false;
        Warrior.weightscore = false;
        characters.Add(Warrior); // เพิ่ม Warrior เข้าไปในลิสต์

        // --- กำหนดค่าเริ่มต้นสำหรับ Mage ---
        CharacterStats Mage = new CharacterStats("Mage");
        Mage.baseHealth = 80;
        Mage.currentHealth = 80;
        Mage.baseMagicPoint = 80; // Mage อาจเป็นสายเวท
        Mage.currentMagicPoint = 80;
        Mage.baseAttack = 5;
        Mage.currentAttack = 5;
        Mage.baseIntelligence = 25;
        Mage.currentIntelligence = 25;
        Mage.baseDefense = 5;
        Mage.currentDefense = 5;
        Mage.baseResistance = 15;
        Mage.currentResistance = 15;
        Mage.baseSpeed = 12;
        Mage.currentSpeed = 12;
        Mage.baseLuck = 12;
        Mage.currentLuck = 12;
        Mage.rulebase = false;
        Mage.weightscore = false;
        characters.Add(Mage); // เพิ่ม Mage เข้าไปในลิสต์

        // --- กำหนดค่าเริ่มต้นสำหรับ Cleric ---
        CharacterStats Cleric = new CharacterStats("Cleric");
        Cleric.baseHealth = 100; // Cleric อาจเป็นสาย Tank
        Cleric.currentHealth = 100;
        Cleric.baseMagicPoint = 60;
        Cleric.currentMagicPoint = 60;
        Cleric.baseAttack = 8;
        Cleric.currentAttack = 8;
        Cleric.baseIntelligence = 20;
        Cleric.currentIntelligence = 20;
        Cleric.baseDefense = 10;
        Cleric.currentDefense = 10;
        Cleric.baseResistance = 18;
        Cleric.currentResistance = 18;
        Cleric.baseSpeed = 11;
        Cleric.currentSpeed = 11;
        Cleric.baseLuck = 14;
        Cleric.currentLuck = 14;
        Cleric.rulebase = false;
        Cleric.weightscore = false;
        characters.Add(Cleric); // เพิ่ม Cleric เข้าไปในลิสต์

        win = false;
        monstername = "";
        musicnum = 0.5f;
        aiaction = "";
        numaiaction = 0;
        Continue = false;
        numquest = 1;
}
}