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
            // ��Ǩ�ͺ mage (੾�ж�� warrior ���ç���͹�)
            else if (!mageStats.rulebase && !mageStats.weightscore)
            {
                SetmageColor();
            }
            // ��Ǩ�ͺ cleric (੾�ж�� warrior ��� mage ���ç���͹�)
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
            // ��Ǩ�ͺ mage (੾�ж�� warrior ���ç���͹�)
            else if (!mageStats.rulebase && !mageStats.weightscore)
            {
                SetmageColor();
            }
            // ��Ǩ�ͺ cleric (੾�ж�� warrior ��� mage ���ç���͹�)
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
        textAi.text = "AI �еѴ�Թ㨨ҡ������˹������ǧ˹��Ẻ��µ�� �� �������͹�Ҵ�� �����ʡ����Ŕ ���� �����ѵ�����ʹ���� ������Ք\r\n�ĵԡ����ͧ AI �Ф�����������ö�Ҵ������� ���������Ѻ�����蹷���ͧ��ä�����͹";
        if (!warriorStats.rulebase && !warriorStats.weightscore)
        {
            SetwarriorColor();
        }
        // ��Ǩ�ͺ mage (੾�ж�� warrior ���ç���͹�)
        else if (!mageStats.rulebase && !mageStats.weightscore)
        {
            SetmageColor();
        }
        // ��Ǩ�ͺ cleric (੾�ж�� warrior ��� mage ���ç���͹�)
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
        textAi.text = "AI �л����Թʶҹ��ó��ͺ�����������ṹ�Ѻ�ء��á�з� �� ������� ��ͧ�ѹ �������\r\n�ҡ��鹨����͡��á�зӷ�����ṹ�٧�ش �����٩�Ҵ����״�����ҡ��� ���������Ѻ�����蹷���ͧ������ AI ��Ѻ��ǵ��ʶҹ��ó�";
        if (!warriorStats.rulebase && !warriorStats.weightscore)
        {
            SetwarriorColor();
        }
        // ��Ǩ�ͺ mage (੾�ж�� warrior ���ç���͹�)
        else if (!mageStats.rulebase && !mageStats.weightscore)
        {
            SetmageColor();
        }
        // ��Ǩ�ͺ cleric (੾�ж�� warrior ��� mage ���ç���͹�)
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
