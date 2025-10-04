using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

public class ToggleManager : MonoBehaviour
{
    public TMP_Dropdown Dropdownplayer;
    public TMP_Dropdown dropdown;
    private string currentPlayerCharacterName = "none";
    private int currentPlayerCharacterIndex = 0;
    public TextMeshProUGUI text;
    public TextMeshProUGUI labeltext;

    public bool ai = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
        CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
        CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");

        if(!warriorStats.rulebase && !warriorStats.weightscore && !mageStats.rulebase && !mageStats.weightscore && !clericStats.rulebase && !clericStats.weightscore)
        {
            currentPlayerCharacterName = "-";
            currentPlayerCharacterIndex = 0;
        }
        else if(!warriorStats.rulebase && !warriorStats.weightscore)
        {
            currentPlayerCharacterName = "warrior";
            currentPlayerCharacterIndex = 1;
        }
        // ตรวจสอบ mage (เฉพาะถ้า warrior ไม่ตรงเงื่อนไข)
        else if (!mageStats.rulebase && !mageStats.weightscore)
        {
            currentPlayerCharacterName = "mage";
            currentPlayerCharacterIndex = 2;
        }
        // ตรวจสอบ cleric (เฉพาะถ้า warrior และ mage ไม่ตรงเงื่อนไข)
        else if (!clericStats.rulebase && !clericStats.weightscore)
        {
            currentPlayerCharacterName = "cleric";
            currentPlayerCharacterIndex = 3;
        }

        if (!ai)
        {
            labeltext.text = currentPlayerCharacterName;
            Dropdownplayer.value = currentPlayerCharacterIndex;
            Dropdownplayer.RefreshShownValue();
        }
        else 
        {
            if (clericStats.rulebase == true || mageStats.rulebase == true || warriorStats.rulebase == true)
            {
                print(0);
                Dropdownplayer.value = 0;
            }
            else if (clericStats.weightscore == true || mageStats.weightscore == true || warriorStats.weightscore == true)
            {
                print(01);
                Dropdownplayer.value = 1;
            }
            Dropdownplayer.RefreshShownValue();
        }


        if (text != null)
        {
            text.text = currentPlayerCharacterName;
        }
        Dropdownplayer.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnDropdownValueChanged(int selectedIndex)
    {
        // selectedIndex คือ index ของตัวเลือกที่ถูกเลือก (เริ่มต้นที่ 0)
        string selectedText = Dropdownplayer.options[selectedIndex].text;
        Debug.Log("Dropdown value changed to index: " + selectedIndex);
        Debug.Log("Selected option text: " + selectedText);
        CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
        CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
        CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
        // คุณสามารถใช้ if/else if หรือ switch case เพื่อทำสิ่งต่างๆ ตามตัวเลือกที่ถูกเลือก
        switch (selectedText)
        {
            case "Rule Base":
                if (dropdown.value != 0)
                {
                    if (dropdown.value == 1)
                    {
                        warriorStats.rulebase = false;
                        mageStats.rulebase = true;
                        clericStats.rulebase = true;
                        warriorStats.weightscore = false;
                        mageStats.weightscore = false;
                        clericStats.weightscore = false;
                    }
                    else if (dropdown.value == 2)
                    {
                        warriorStats.rulebase = true;
                        mageStats.rulebase = false;
                        clericStats.rulebase = true;
                        warriorStats.weightscore = false;
                        mageStats.weightscore = false;
                        clericStats.weightscore = false;
                    }
                    else if (dropdown.value == 3)
                    {
                        warriorStats.rulebase = true;
                        mageStats.rulebase = true;
                        clericStats.rulebase = false;
                        warriorStats.weightscore = false;
                        mageStats.weightscore = false;
                        clericStats.weightscore = false;
                    }
                        
                    PlayerDataManager.UpdateCharacterStats(warriorStats);
                    PlayerDataManager.UpdateCharacterStats(mageStats);
                    PlayerDataManager.UpdateCharacterStats(clericStats);
                    Debug.Log("Weight Score");
                }
                else
                {
                    Dropdownplayer.value = 2;
                    Debug.Log("None");
                }
                break;
            case "Weight Score":
                if(dropdown.value != 0)
                {
                    if (dropdown.value == 1)
                    {
                        warriorStats.weightscore = false;
                        mageStats.weightscore = true;
                        clericStats.weightscore = true;
                        warriorStats.rulebase = false;
                        mageStats.rulebase = false;
                        clericStats.rulebase = false;
                    }
                    else if (dropdown.value == 2)
                    {
                        warriorStats.weightscore = true;
                        mageStats.weightscore = false;
                        clericStats.weightscore = true;
                        warriorStats.rulebase = false;
                        mageStats.rulebase = false;
                        clericStats.rulebase = false;
                    }
                    else if (dropdown.value == 3)
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
                    Debug.Log("Weight Score");
                }
                else
                {
                    Dropdownplayer.value = 2;
                    Debug.Log("None");
                }
                break;
            case "None":
                List<string> options = new List<string> { "-", "warrior", "mage", "cleric" };
                text.text = "All";
                labeltext.text = "-";
                dropdown.value = options.IndexOf("-");
                dropdown.RefreshShownValue();
                warriorStats.weightscore = false;
                mageStats.weightscore = false;
                clericStats.weightscore = false;
                warriorStats.rulebase = false;
                mageStats.rulebase = false;
                clericStats.rulebase = false;
                PlayerDataManager.UpdateCharacterStats(warriorStats);
                PlayerDataManager.UpdateCharacterStats(mageStats);
                PlayerDataManager.UpdateCharacterStats(clericStats);
                dropdown.value = 0;
                Debug.Log("None");
                break;
            case "warrior":
                if (text != null)
                {
                    text.text = "warrior";
                }
                warriorStats.rulebase = false;
                warriorStats.weightscore = false;
                mageStats.rulebase = true;
                mageStats.weightscore = false;
                clericStats.rulebase = true;
                clericStats.weightscore = false;
                PlayerDataManager.UpdateCharacterStats(warriorStats);
                PlayerDataManager.UpdateCharacterStats(mageStats);
                PlayerDataManager.UpdateCharacterStats(clericStats);
                dropdown.value = 0;
                Debug.Log("warrior");
                break;
            case "mage":
                if (text != null)
                {
                    text.text = "mage";
                }
                warriorStats.rulebase = true;
                warriorStats.weightscore = false;
                mageStats.rulebase = false;
                mageStats.weightscore = false;
                clericStats.rulebase = true;
                clericStats.weightscore = false;
                PlayerDataManager.UpdateCharacterStats(warriorStats);
                PlayerDataManager.UpdateCharacterStats(mageStats);
                PlayerDataManager.UpdateCharacterStats(clericStats);
                dropdown.value = 0;
                Debug.Log("mage");
                break;
            case "cleric":
                if (text != null)
                {
                    text.text = "cleric";
                }
                warriorStats.rulebase = true;
                warriorStats.weightscore = false;
                mageStats.rulebase = true;
                mageStats.weightscore = false;
                clericStats.rulebase = false;
                clericStats.weightscore = false;
                PlayerDataManager.UpdateCharacterStats(warriorStats);
                PlayerDataManager.UpdateCharacterStats(mageStats);
                PlayerDataManager.UpdateCharacterStats(clericStats);
                dropdown.value = 0;
                Debug.Log("cleric");
                break;

            default:
                Debug.Log("Unknown option selected.");
                break;

        }

    }

}