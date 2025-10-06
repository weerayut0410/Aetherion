using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerstat : MonoBehaviour
{
    public TextMeshProUGUI textstat;
    public Sprite imagewarriorstat;
    public Sprite imagemagestat;
    public Sprite imageclericstat;
    public Sprite imagewarriorstatinfo;
    public Sprite imagemagestatinfo;
    public Sprite imageclericstatinfo;
    public Sprite imagewarriorstatskill;
    public Sprite imagemagestatskill;
    public Sprite imageclericstatskill;
    public Image image;
    public Image imageinfo;
    public Image imageskill;

    public TextMeshProUGUI leveltext;

    private void Start()
    {
        warriorstat();
        CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
        CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
        CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
        if (warriorStats.currentHealth ==0  && mageStats.currentHealth == 0 && clericStats.currentHealth == 0)
        {

            // ลดค่าพลังชีวิตและมานาลงครึ่งหนึ่ง
            // ใช้ Mathf.CeilToInt เพื่อปัดเศษขึ้น เพื่อให้มีค่าอย่างน้อย 1
            if (warriorStats != null)
            {
                warriorStats.currentHealth = Mathf.CeilToInt((float)warriorStats.baseHealth / 2f);
                warriorStats.currentMagicPoint = Mathf.CeilToInt((float)warriorStats.baseMagicPoint / 2f);
                PlayerDataManager.UpdateCharacterStats(warriorStats); // บันทึกการเปลี่ยนแปลง
            }
            if (mageStats != null)
            {
                mageStats.currentHealth = Mathf.CeilToInt((float)mageStats.baseHealth / 2f);
                mageStats.currentMagicPoint = Mathf.CeilToInt((float)mageStats.baseMagicPoint / 2f);
                PlayerDataManager.UpdateCharacterStats(mageStats);
            }
            if (clericStats != null)
            {
                clericStats.currentHealth = Mathf.CeilToInt((float)clericStats.baseHealth / 2f);
                clericStats.currentMagicPoint = Mathf.CeilToInt((float)clericStats.baseMagicPoint / 2f);
                PlayerDataManager.UpdateCharacterStats(clericStats);
            }
        }
    }
    public void warriorstat()
    {
        image.sprite = imagewarriorstat;
        imageinfo.sprite = imagewarriorstatinfo;
        imageskill.sprite = imagewarriorstatskill;
        CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
        textstat.text = $"HP = {warriorStats.currentHealth}\r\nMP = {warriorStats.currentMagicPoint}\r\nATK = {warriorStats.currentAttack}\r\nINT = {warriorStats.currentIntelligence}\r\nDEF = {warriorStats.currentDefense}\r\nRES = {warriorStats.currentResistance}\r\nLUCK = {warriorStats.currentLuck}";
    }
    public void magestat()
    {
        image.sprite = imagemagestat;
        imageinfo.sprite = imagemagestatinfo;
        imageskill.sprite = imagemagestatskill;
        CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
        textstat.text = $"HP = {mageStats.currentHealth}\r\nMP = {mageStats.currentMagicPoint}\r\nATK = {mageStats.currentAttack}\r\nINT = {mageStats.currentIntelligence}\r\nDEF = {mageStats.currentDefense}\r\nRES = {mageStats.currentResistance}\r\nLUCK = {mageStats.currentLuck}";
    }
    public void clericstat()
    {
        image.sprite = imageclericstat;
        imageinfo.sprite = imageclericstatinfo;
        imageskill.sprite= imageclericstatskill;
        CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
        textstat.text = $"HP = {clericStats.currentHealth}\r\nMP = {clericStats.currentMagicPoint}\r\nATK = {clericStats.currentAttack}\r\nINT = {clericStats.currentIntelligence}\r\nDEF = {clericStats.currentDefense}\r\nRES = {clericStats.currentResistance}\r\nLUCK = {clericStats.currentLuck}";
    }
    public void home()
    {
        SceneManager.LoadScene("menu");
    }
    private void Update()
    {
        leveltext.text = $"Level { PlayerDataManager.Getlevel().ToString()}";
    }
}
