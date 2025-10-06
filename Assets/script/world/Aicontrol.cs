using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Aicontrol : MonoBehaviour
{
    [Header("AI Buttons")]
    public Button ruleBasedButton;
    public Button weightedScoreButton;
    public Button warrior;
    public Button mage;
    public Button cleric;

    bool rulebase = true;


    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.red;


    public TextMeshProUGUI textAi;
    CharacterStats warriorStats;
    CharacterStats mageStats;
    CharacterStats clericStats; 

    private void Start()
    {
        warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
        mageStats = PlayerDataManager.GetCharacterStats("Mage");
        clericStats = PlayerDataManager.GetCharacterStats("Cleric");
        if (!warriorStats.weightscore && !mageStats.weightscore &&!clericStats.weightscore)
        {
            SetRuleBasedColor();
            if (!warriorStats.rulebase && !warriorStats.weightscore)
            {
                SetwarriorColor();
            }
            // ตรวจสอบ mage (เฉพาะถ้า warrior ไม่ตรงเงื่อนไข)
            else if (!mageStats.rulebase && !mageStats.weightscore)
            {
                SetmageColor();
            }
            // ตรวจสอบ cleric (เฉพาะถ้า warrior และ mage ไม่ตรงเงื่อนไข)
            else if (!clericStats.rulebase && !clericStats.weightscore)
            {
                SetclericColor();
            }
        }
        else 
        {
            SetWeightedColor();
            if (!warriorStats.rulebase && !warriorStats.weightscore)
            {
                SetwarriorColor();
            }
            // ตรวจสอบ mage (เฉพาะถ้า warrior ไม่ตรงเงื่อนไข)
            else if (!mageStats.rulebase && !mageStats.weightscore)
            {
                SetmageColor();
            }
            // ตรวจสอบ cleric (เฉพาะถ้า warrior และ mage ไม่ตรงเงื่อนไข)
            else if (!clericStats.rulebase && !clericStats.weightscore)
            {
                SetclericColor();
            }
        }
    }

    public void SetRuleBasedColor()
    {
        rulebase = true;
        SetButtonColor(ruleBasedButton, selectedColor);
        SetButtonColor(weightedScoreButton, normalColor);
        textAi.text = "AI จะตัดสินใจจากกฎที่กำหนดไว้ล่วงหน้าแบบตายตัว เช่น “ถ้าเพื่อนบาดเจ็บ ให้ใช้สกิลฮีล” หรือ “ถ้าศัตรูเลือดน้อย ให้โจมตี”\r\nพฤติกรรมของ AI จะคงที่และสามารถคาดเดาได้ง่าย เหมาะสำหรับผู้เล่นที่ต้องการความแน่นอน";
        if (!warriorStats.rulebase && !warriorStats.weightscore)
        {
            SetwarriorColor();
        }
        // ตรวจสอบ mage (เฉพาะถ้า warrior ไม่ตรงเงื่อนไข)
        else if (!mageStats.rulebase && !mageStats.weightscore)
        {
            SetmageColor();
        }
        // ตรวจสอบ cleric (เฉพาะถ้า warrior และ mage ไม่ตรงเงื่อนไข)
        else if (!clericStats.rulebase && !clericStats.weightscore)
        {
            SetclericColor();
        }
    }

    public void SetWeightedColor()
    {
        rulebase = false;
        SetButtonColor(ruleBasedButton, normalColor);
        SetButtonColor(weightedScoreButton, selectedColor);
        textAi.text = "AI จะประเมินสถานการณ์รอบตัวแล้วให้คะแนนกับทุกการกระทำ เช่น การโจมตี ป้องกัน หรือฮีล\r\nจากนั้นจะเลือกการกระทำที่ได้คะแนนสูงสุด ทำให้ดูฉลาดและยืดหยุ่นมากขึ้น เหมาะสำหรับผู้เล่นที่ต้องการให้ AI ปรับตัวตามสถานการณ์";
        if (!warriorStats.rulebase && !warriorStats.weightscore)
        {
            SetwarriorColor();
        }
        // ตรวจสอบ mage (เฉพาะถ้า warrior ไม่ตรงเงื่อนไข)
        else if (!mageStats.rulebase && !mageStats.weightscore)
        {
            SetmageColor();
        }
        // ตรวจสอบ cleric (เฉพาะถ้า warrior และ mage ไม่ตรงเงื่อนไข)
        else if (!clericStats.rulebase && !clericStats.weightscore)
        {
            SetclericColor();
        }
    }

    public void SetwarriorColor()
    {
        SetButtonColor(mage, normalColor);
        SetButtonColor(cleric, normalColor);
        SetButtonColor(warrior, selectedColor);
        if (rulebase)
        {
            warriorStats.weightscore = false;
            mageStats.weightscore = false;
            clericStats.weightscore = false;
            warriorStats.rulebase = false;
            mageStats.rulebase = true;
            clericStats.rulebase = true;
        }
        else 
        {
            warriorStats.weightscore = false;
            mageStats.weightscore = true;
            clericStats.weightscore = true;
            warriorStats.rulebase = false;
            mageStats.rulebase = false;
            clericStats.rulebase = false;
        }

        PlayerDataManager.UpdateCharacterStats(warriorStats);
        PlayerDataManager.UpdateCharacterStats(mageStats);
        PlayerDataManager.UpdateCharacterStats(clericStats);

    }
    public void SetmageColor()
    {
        SetButtonColor(mage, selectedColor);
        SetButtonColor(cleric, normalColor);
        SetButtonColor(warrior, normalColor);

        if (rulebase)
        {
            warriorStats.weightscore = false;
            mageStats.weightscore = false;
            clericStats.weightscore = false;
            warriorStats.rulebase = true;
            mageStats.rulebase = false;
            clericStats.rulebase = true;
        }
        else
        {
            warriorStats.weightscore = true;
            mageStats.weightscore = false;
            clericStats.weightscore = true;
            warriorStats.rulebase = false;
            mageStats.rulebase = false;
            clericStats.rulebase = false;
        }

        PlayerDataManager.UpdateCharacterStats(warriorStats);
        PlayerDataManager.UpdateCharacterStats(mageStats);
        PlayerDataManager.UpdateCharacterStats(clericStats);
    }
    public void SetclericColor()
    {
        SetButtonColor(mage, normalColor);
        SetButtonColor(cleric, selectedColor);
        SetButtonColor(warrior, normalColor);

        if (rulebase)
        {
            warriorStats.weightscore = false;
            mageStats.weightscore = false;
            clericStats.weightscore = false;
            warriorStats.rulebase = true;
            mageStats.rulebase = true;
            clericStats.rulebase = false;
        }
        else
        {
            warriorStats.weightscore = true;
            mageStats.weightscore = true;
            clericStats.weightscore = false;
            warriorStats.rulebase = false;
            mageStats.rulebase = false;
            clericStats.rulebase = false;
        }

        PlayerDataManager.UpdateCharacterStats(warriorStats);
        PlayerDataManager.UpdateCharacterStats(mageStats);
        PlayerDataManager.UpdateCharacterStats(clericStats);
    }


    private void SetButtonColor(Button btn, Color color)
    {
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = color;
    }
}
