using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerstat : MonoBehaviour
{
    public TextMeshProUGUI textstat;
    public TextMeshProUGUI textskill;
    public Sprite imagewarriorstat;
    public Sprite imagemagestat;
    public Sprite imageclericstat;
    public Image image;

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
        CharacterStats warriorStats = PlayerDataManager.GetCharacterStats("Warrior");
        textstat.text = $"HP = {warriorStats.currentHealth} MP = {warriorStats.currentMagicPoint} ATK = {warriorStats.currentAttack} \nINT = {warriorStats.currentIntelligence} DEF = {warriorStats.currentDefense} RES = {warriorStats.currentResistance} LUCK = {warriorStats.currentLuck}";
        textskill.text = "Attack โจมตี0.7 MP+10\r\nPowerSlash โจมตี1.5 ใช้ 15 MP\r\nTaunt ดึงความสนใจศัตรูทั้งหมด 2เทิร์น ใช้ 20 MP\r\nIronGuard ลดความเสียหายที่ได้รับ 50% 1เทิร์น \nใช้ 25 MP\r\nWhirlwind โจมตีทุกตัว0.8 ใช้ 30 MP";
    }
    public void magestat()
    {
        image.sprite = imagemagestat;
        CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
        textstat.text = $"HP = {mageStats.currentHealth} MP = {mageStats.currentMagicPoint} ATK = {mageStats.currentAttack}  \nINT = {mageStats.currentIntelligence} DEF = {mageStats.currentDefense} RES = {mageStats.currentResistance} LUCK = {mageStats.currentLuck}";
        textskill.text = "Attack โจมตี0.7 MP+10\r\nFireball โจมตี1.8 ใช้ 25 MP\r\nIceShard โจมตี1.2 ลด ATK/INT 20% 2เทิร์น \nใช้ 20 MP\r\nArcaneBurst โจมตีทุกตัว1.0 ใช้ 35 MP\r\nManaShield สร้างโล่ 50% 2เทิร์น ใช้ 30 MP";
    }
    public void clericstat()
    {
        image.sprite = imageclericstat;
        CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
        textstat.text = $"HP = {clericStats.currentHealth} MP = {clericStats.currentMagicPoint} ATK = {clericStats.currentAttack} \nINT = {clericStats.currentIntelligence} DEF = {clericStats.currentDefense} RES = {clericStats.currentResistance} LUCK = {clericStats.currentLuck}";
        textskill.text = "Attack โจมตี0.7 MP+10\r\nHeal ฟื้นฟู HP ตามค่า IN ใช้ 20 MP\r\nSanctuary ลดความเสียหายที่ได้รับ 40% 1เทิร์น \nใช้ 25 MP\r\nBlessing เพิ่ม ATK,INT 20% และ LUCK 10% 2เทิร์น ใช้ 20 MP\r\nPurify ล้างสถานะผิดปกติทั้งหมด ใช้ 15 MP\r\nCurse ลด ATK/INT/DEF/RES 20% ทุกตัว 2เทิร์น ใช้ 45 MP";
    }
    public void home()
    {
        SceneManager.LoadScene("menu");
    }
}
