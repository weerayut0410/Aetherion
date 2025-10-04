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

            // Ŵ��Ҿ�ѧ���Ե����ҹ�ŧ����˹��
            // �� Mathf.CeilToInt ���ͻѴ��ɢ�� ��������դ�����ҧ���� 1
            if (warriorStats != null)
            {
                warriorStats.currentHealth = Mathf.CeilToInt((float)warriorStats.baseHealth / 2f);
                warriorStats.currentMagicPoint = Mathf.CeilToInt((float)warriorStats.baseMagicPoint / 2f);
                PlayerDataManager.UpdateCharacterStats(warriorStats); // �ѹ�֡�������¹�ŧ
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
        textskill.text = "Attack ����0.7 MP+10\r\nPowerSlash ����1.5 �� 15 MP\r\nTaunt �֧����ʹ��ѵ�ٷ����� 2���� �� 20 MP\r\nIronGuard Ŵ����������·�����Ѻ 50% 1���� \n�� 25 MP\r\nWhirlwind ���շء���0.8 �� 30 MP";
    }
    public void magestat()
    {
        image.sprite = imagemagestat;
        CharacterStats mageStats = PlayerDataManager.GetCharacterStats("Mage");
        textstat.text = $"HP = {mageStats.currentHealth} MP = {mageStats.currentMagicPoint} ATK = {mageStats.currentAttack}  \nINT = {mageStats.currentIntelligence} DEF = {mageStats.currentDefense} RES = {mageStats.currentResistance} LUCK = {mageStats.currentLuck}";
        textskill.text = "Attack ����0.7 MP+10\r\nFireball ����1.8 �� 25 MP\r\nIceShard ����1.2 Ŵ ATK/INT 20% 2���� \n�� 20 MP\r\nArcaneBurst ���շء���1.0 �� 35 MP\r\nManaShield ���ҧ��� 50% 2���� �� 30 MP";
    }
    public void clericstat()
    {
        image.sprite = imageclericstat;
        CharacterStats clericStats = PlayerDataManager.GetCharacterStats("Cleric");
        textstat.text = $"HP = {clericStats.currentHealth} MP = {clericStats.currentMagicPoint} ATK = {clericStats.currentAttack} \nINT = {clericStats.currentIntelligence} DEF = {clericStats.currentDefense} RES = {clericStats.currentResistance} LUCK = {clericStats.currentLuck}";
        textskill.text = "Attack ����0.7 MP+10\r\nHeal ��鹿� HP ������ IN �� 20 MP\r\nSanctuary Ŵ����������·�����Ѻ 40% 1���� \n�� 25 MP\r\nBlessing ���� ATK,INT 20% ��� LUCK 10% 2���� �� 20 MP\r\nPurify ��ҧʶҹмԴ���Է����� �� 15 MP\r\nCurse Ŵ ATK/INT/DEF/RES 20% �ء��� 2���� �� 45 MP";
    }
    public void home()
    {
        SceneManager.LoadScene("menu");
    }
}
