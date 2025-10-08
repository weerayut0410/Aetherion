using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // จำเป็นสำหรับ Image และ UI components

public class TurnManager : MonoBehaviour
{
    public GameObject EndCanva;
    public TextMeshProUGUI endtext;
    public GameObject status;
    public TextMeshProUGUI itemtext;

    public Queue<Character> turnQueue = new Queue<Character>();
    public static Character[] characters;
    public Character currentCharacter;
    public bool spawn = false;

    public List<Character> playerteam = new List<Character>();
    public List<Character> enemyteam = new List<Character>();

    public static TurnManager Instance { get; private set; }

    public StatusEffectManager statusEffectManager;

    public int winner = 0;
    bool stopgame = false;
    public GameObject stopcanvas;

    public Image bar;
    public TextMeshProUGUI levelext;
    public TextMeshProUGUI exptext;

    public GameObject canvastip;
    public GameObject tipbutton;
    public enum StartMode
    {
        PlayerFirst,
        EnemyFirst
    }

    public StartMode startMode = StartMode.PlayerFirst;

    // --- ตัวแปร UI ใหม่ ---
    public Transform turnOrderUIContainer; // กำหนด Panel ที่มี Vertical Layout Group ที่นี่
    public GameObject turnPortraitUIPrefab; // กำหนด TurnPortraitUI Prefab ของคุณที่นี่
    private List<GameObject> activeTurnPortraits = new List<GameObject>(); // เพื่อติดตามรูปภาพที่ถูกสร้างขึ้น

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (statusEffectManager == null)
        {
            statusEffectManager = GetComponent<StatusEffectManager>();
            if (statusEffectManager == null)
            {
                Debug.LogWarning("StatusEffectManager not found on TurnManager!");
            }
        }
    }

    void Start()
    {
        tipbutton.SetActive(false);
        EndCanva.SetActive(false);
        status.SetActive(false);
        if (!spawn)
        {
            StartCoroutine(InitializeTurnQueue());
        }
        endtext = EndCanva.GetComponentInChildren<TextMeshProUGUI>();
        winner = 0;
    }
    public void tip()
    {
        tipbutton.SetActive(true);
        Time.timeScale = 0.0f;
        canvastip.SetActive(true);
        PlayerDataManager.notnewbie();
    }
    public void closetip()
    {
        Time.timeScale = 1.0f;
        canvastip.SetActive(false);
    }

    public IEnumerator InitializeTurnQueue()
    {
        yield return null;

        characters = FindObjectsByType<Character>(FindObjectsSortMode.None);

        List<Character> tempList = new List<Character>();
        playerteam.Clear();
        enemyteam.Clear();
        PlayerDataManager.Clearaiaction();

        foreach (Character c in characters)
        {
            if (c.IsAlive())
            {
                tempList.Add(c);
                if (c.isEnemy)
                    enemyteam.Add(c);
                else
                    playerteam.Add(c);
            }
        }

        // เรียง speed และฝ่ายเหมือนเดิม
        tempList = tempList.OrderByDescending(c => c.speed).ToList();

        if (startMode == StartMode.PlayerFirst)
            tempList = tempList.OrderByDescending(c => !c.isEnemy).ToList();
        else
            tempList = tempList.OrderByDescending(c => c.isEnemy).ToList();

        foreach (Character c in tempList)
        {
            turnQueue.Enqueue(c);
        }

        UpdateTurnOrderUI(); // เรียกใช้เพื่อเติมข้อมูล UI ตั้งแต่เริ่มต้น
        StartTurn();
    }


    void Update()
    {

        if (PlayerDataManager.newbie())
        {
            tip();
        }


        if (!IsTeamAlive(false))
        {
            EndBattle(false);
        }

        if (!IsTeamAlive(true))
        {
            winner +=1;
            EndBattle(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!stopgame)
            {
                Time.timeScale = 0.0f;
                stopgame = !stopgame;
                stopcanvas.SetActive(true);
            }
            else 
            {
                Time.timeScale = 1.0f;
                stopgame = !stopgame;
                stopcanvas.SetActive(false);
            }

        }
        if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1))
        {
            if (currentCharacter != null)
            {
                if (!currentCharacter.isuseitem && !currentCharacter.isaction)
                {
                    if (currentCharacter is Mage mage)
                        mage.CancelSkill();
                    else if (currentCharacter is Warrior warrior)
                        warrior.CancelSkill();
                    else if (currentCharacter is Cleric cleric)
                        cleric.CancelSkill();
                }
            }
        }
    }

    public void StartTurn()
    {
        if (turnQueue.Count == 0) return;

        currentCharacter = turnQueue.Peek(); // ตัวหน้าสุด
        Debug.Log("Turn Queue: " + string.Join(" → ", turnQueue.Select(c => c.characterName)));

        // ไฮไลต์ตัวละครปัจจุบันใน UI
        UpdateTurnOrderUI();

        currentCharacter.TakeTurn();
    }

    public void NextTurn()
    {
        if (turnQueue.Count == 0)
        {
            Debug.Log("Queue is empty.");
            return;
        }

        Character finishedCharacter = turnQueue.Dequeue();

        RemoveDead(); // ลบตัวที่ตายทั้งหมด

        if (finishedCharacter.IsAlive())
        {
            turnQueue.Enqueue(finishedCharacter); // หมุนกลับไปท้ายคิว
        }

        UpdateTurnOrderUI(); // อัปเดต UI หลังจากเปลี่ยนเทิร์นและลบตัวละคร
        StartTurn();
    }

    public void RemoveDead()
    {
        Queue<Character> newQueue = new Queue<Character>();

        foreach (var c in turnQueue)
        {
            if (c.IsAlive())
                newQueue.Enqueue(c);
        }

        turnQueue = newQueue;
        UpdateTurnOrderUI(); // อัปเดต UI หลังจากลบตัวละครที่ตายแล้ว
    }

    private bool IsTeamAlive(bool isEnemyTeam)
    {
        Character[] characters = FindObjectsByType<Character>(FindObjectsSortMode.None);
        foreach (Character c in characters)
        {
            if (c.isEnemy == isEnemyTeam && c.IsAlive())
                return true;
        }
        return false;
    }
    void additem1()
    {
        if (winner == 1)
        {
            int numbers = Random.Range(0,5);
            int numbersitem = Random.Range(1, 3);
            if (numbers == 0)
            {
                PlayerDataManager.AddItem("hpPotion", numbersitem);
                itemtext.text = $"Item hpPotion + {numbersitem}";
            }
            else if (numbers == 1)
            {
                PlayerDataManager.AddItem("fullHpPotion", numbersitem);
                itemtext.text = $"Item fullHpPotion + {numbersitem}";
            }
            else if (numbers == 2)
            {
                PlayerDataManager.AddItem("manaPotion", numbersitem);
                itemtext.text = $"Item manaPotion + {numbersitem}";
            }
            else if (numbers == 3)
            {
                PlayerDataManager.AddItem("fullManaPotion", numbersitem);
                itemtext.text = $"Item fullManaPotion + {numbersitem}";
            }
            else if (numbers == 4)
            {
                PlayerDataManager.AddItem("phoenixFeather", 1);
                itemtext.text = "Item phoenixFeather +1";
            }
        }

    }
    private void EndBattle(bool Win)
    {
       
        Time.timeScale = 0f;
        endtext.text = Win ? "Win" : "Lose";
        
        PlayerDataManager.SetWinStatus(Win);
        if (Win) 
        {
            status.SetActive(true);
            PlayerDataManager.AddDefeatedEnemy(PlayerDataManager.getnamemon());

            int level = PlayerDataManager.Getlevel();
            int upexp = PlayerDataManager.Getexp();
            int exp;
            int money;
            bar.fillAmount = (float)upexp / 40;
            levelext.text = $"Level {level}";
            
            if (level == 1)
            {
                exp = 40;
                money = 100;
            }
            else if (level == 2)
            {
                exp = 20;
                money = 200;
            }
            else { exp = 15; money = 300; }

            exptext.text = $"EXP +{exp}";

            if (winner == 1)
            {
                PlayerDataManager.Upexp(exp);
                PlayerDataManager.addcash(money);
            }
            additem1();

        }
        EndCanva.SetActive(true);
    }
    public void backworld()
    {
        Cursor.visible = false; // ซ่อนเมาส์
        Cursor.lockState = CursorLockMode.Locked;
        string name =PlayerDataManager.getnamemon();
        if (name == "ghost" && winner >= 1) 
        {
            SceneManager.LoadScene("end");
        }
        else { SceneManager.LoadScene("Environment_Free"); }
    }
    // --- เมธอด UI ใหม่ ---
    public void UpdateTurnOrderUI()
    {
        // ลบรูปภาพที่มีอยู่เดิม
        foreach (GameObject portrait in activeTurnPortraits)
        {
            Destroy(portrait);
        }
        activeTurnPortraits.Clear();

        // เติมข้อมูลด้วยคิวเทิร์นปัจจุบัน
        foreach (Character c in turnQueue)
        {
            GameObject portraitGO = Instantiate(turnPortraitUIPrefab, turnOrderUIContainer);
            Image characterImage = portraitGO.GetComponent<Image>();

            // สมมติว่าสคริปต์ Character ของคุณมีฟิลด์ Sprite 'characterPortrait'
            if (c.characterPortrait != null)
            {
                characterImage.sprite = c.characterPortrait;
            }
            else
            {
                // ตัวเลือกสำรองหากไม่มีรูปภาพถูกกำหนด
                characterImage.color = c.isEnemy ? Color.red : Color.blue;
                Debug.LogWarning($"Character {c.characterName} has no portrait sprite assigned!");
            }

            // ทางเลือก: เพิ่มตัวบ่งชี้ภาพสำหรับตัวละครปัจจุบัน
            if (c == currentCharacter)
            {
                characterImage.color = Color.white; // ตรวจสอบให้แน่ใจว่าไม่มีการย้อมสีหากคุณใช้ตัวเลือกสำรอง
                // เพิ่มขอบหากคุณมี (เช่น คอมโพเนนต์ Image อื่นเป็น child)
            }
            else
            {
                characterImage.color = Color.gray; // ทำให้ตัวละครที่ไม่ใช่ปัจจุบันจางลง
            }

            activeTurnPortraits.Add(portraitGO);
        }
    }
}