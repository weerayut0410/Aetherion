using UnityEngine;
using UnityEngine.SceneManagement;
public class menu : MonoBehaviour
{
    int selectcharacter;
    public GameObject canvaselectplay;
    public GameObject canvasmenu;
    public GameObject canvasoption;


    private void Start()
    {
        backtomenu();
    }
    public void newgame()
    {
        canvaselectplay.SetActive(true);
        canvasmenu.SetActive(false);
    }
    public void Continue()
    {
        if (PlayerDataManager.Getcontinue() == true)
        {
            Cursor.visible = false; // ซ่อนเมาส์
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.LoadScene("Environment_Free");
        }
        
    }
    public void option()
    {
        canvasoption.SetActive(true);
        canvasmenu.SetActive(false);
    }
    public void quit()
    {
        Application.Quit();
    }
    public void backtomenu()
    {
        canvasmenu.SetActive(true);
        canvasoption.SetActive(false);
        canvaselectplay.SetActive(false);
    }
    public void warrior()
    {
        selectcharacter = 1;
    }
    public void mage()
    {
        selectcharacter = 2;
    }
    public void cleric()
    {
        selectcharacter = 3;
    }
    public void play()
    {

        if (selectcharacter == 1) 
        {
            isplay();
            CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
            warriorStats.rulebase = false;
            PlayerDataManager.UpdateCharacterStats(warriorStats);
            CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
            mageStats.rulebase = true;
            PlayerDataManager.UpdateCharacterStats(mageStats);
            CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
            clericStats.rulebase = true;
            PlayerDataManager.UpdateCharacterStats(clericStats);
            SceneManager.LoadScene("Environment_Free");
        }
        else if (selectcharacter == 2) 
        {
            isplay();
            CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
            warriorStats.rulebase = true;
            PlayerDataManager.UpdateCharacterStats(warriorStats);
            CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
            mageStats.rulebase = false;
            PlayerDataManager.UpdateCharacterStats(mageStats);
            CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
            clericStats.rulebase = true;
            PlayerDataManager.UpdateCharacterStats(clericStats);
            SceneManager.LoadScene("Environment_Free");
        }
        else if (selectcharacter == 3) 
        {
            isplay();
            CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
            warriorStats.rulebase = true;
            PlayerDataManager.UpdateCharacterStats(warriorStats);
            CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
            mageStats.rulebase = true;
            PlayerDataManager.UpdateCharacterStats(mageStats);
            CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
            clericStats.rulebase = false;
            PlayerDataManager.UpdateCharacterStats(clericStats);
            SceneManager.LoadScene("Environment_Free");
        }
    }
    public void SetMusicVolume(float volumeValue)
    {
        print(volumeValue);
        PlayerDataManager.setmusic(volumeValue);
    }
    void isplay()
    {
        PlayerDataManager.ClearAllPlayerData();
        PlayerDataManager.ClearAllItems();
        PlayerDataManager.ClearAllDefeatedEnemies();
        PlayerDataManager.AddItem("hpPotion", 1);
        PlayerDataManager.AddItem("fullHpPotion", 1);
        PlayerDataManager.AddItem("manaPotion", 1);
        PlayerDataManager.AddItem("fullManaPotion", 1);
        PlayerDataManager.AddItem("phoenixFeather", 1);

        PlayerDataManager.Setcontinue(true);
        Cursor.visible = false; // ซ่อนเมาส์
        Cursor.lockState = CursorLockMode.Locked;
    }
    }
