using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class talk : MonoBehaviour
{
    public GameObject canvastalkmenu;
    public GameObject canvastalk;
    bool cantalk = false;
    public TextMeshProUGUI text;
    int textnum = 0;
    public GameObject boss;
    public GameObject wolf;
    public GameObject boar1;
    public GameObject boar2;
    public GameObject mushroom;
    public GameObject bear;
    public GameObject crab;
    int numquest;

    int quest1 = 0, quest2 = 0;
    int exp = 0;
    int level;
    int upexp;
    int money;
    string itemname;

    public GameObject canvasgif;
    public TextMeshProUGUI textgif;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = "";
        numquest = PlayerDataManager.getnumquest();
    }
    private void Update()
    {
        level = PlayerDataManager.Getlevel();
        upexp = PlayerDataManager.Getexp();

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

        switch (numquest)
        {
            case 2:
                if (boar1 == null && boar2 == null)
                    PlayerDataManager.finishquest(3);   // ����ٻ�� -> 仴�ҹ��� (3)
                break;

            case 3:
                if (mushroom == null)
                    PlayerDataManager.finishquest(4);   // ����� -> 仴�ҹ��һ�� (4)
                break;

            case 4:
                if (wolf == null)
                    PlayerDataManager.finishquest(5);   // ����һ�� -> ��Ѻ�Ҿ����2 (5)
                break;

            case 5:
                // �ç����� ���¾����2� ���������繤��ѻ�� 6 (�蹵͹�������)
                break;

            case 6:
                if (bear == null)
                    PlayerDataManager.finishquest(7);   // ����� -> 仴�ҹ�� (7)
                break;

            case 7:
                if (crab == null)                       // ��������������� carb ��������ç�����
                    PlayerDataManager.finishquest(8);   // ���� -> ��Ѻ�Ҿ����3 (8)
                break;

            case 8:
                // ���¾����3� ���������ѻ�� 9
                break;

            case 9:
                if (boss == null)
                    PlayerDataManager.finishquest(10);  // ����� -> �Դ�Ԫ��� (10)
                break;
        }


        if (Input.GetKeyDown(KeyCode.F) && cantalk)
        {
            // ��� canvastalk ���ѧ�ʴ����� ���Դ
            if (canvastalk.activeSelf && textnum == 10)
            {
                print("�Դ canvastalk");
                canvastalk.SetActive(false);

            }
            // ��� canvastalk ������ʴ����� ����Դ
            else if (!canvastalk.activeSelf && textnum == 0 && boss != null & wolf != null) 
            {
                canvastalk.SetActive(true);
                text.text = "�Թᴹ Aetherion ���������ͧ...����з�����״��׹�Թ�ء���";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss != null & wolf != null) 
            {
                text.text = "�ʧ���ҧ�С�Ѻ�׹�����������ͻ�Һ Umbra ������ҹ��";
                textnum = 2;
            }
            else if (canvastalk.activeSelf && textnum == 2 && boss != null & wolf != null)
            {
                text.text = "��è���� Umbra ���� �е�ͧ�Ǻ��������ǹ��� 2 ���";
                textnum = 3;
            }
            else if (canvastalk.activeSelf && textnum == 3 && boss != null & wolf != null)
            {
                text.text = "����á����Ѻ�ѵ�������㹻���֡ ��仹Ӫ����ǹ�á��";
                PlayerDataManager.finishquest(2);
                print(PlayerDataManager.getnumquest());
                textnum = 10;
                
            }
            else if (!canvastalk.activeSelf && textnum == 0 && boss != null & wolf == null & crab != null)
            {
                canvastalk.SetActive(true);
                text.text = "������ǹ�á�������Թ�";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss != null & wolf == null & crab != null)
            {
                text.text = "���Ǣͧ�����ǹ����ͧ ��������觹�� \n�Թ�����¹�ӷҧ��� �������բ�ҧ�ҧ����";
                textnum = 2;
            }

            else if (canvastalk.activeSelf && textnum == 2 && boss != null & wolf == null & crab != null)
            {
                text.text = "��仹Ӫ����ǹ����ͧ��";
                PlayerDataManager.finishquest(6);
                textnum = 10;
                //gif
                quest1 += 1;
                if (quest1 == 1) 
                {
                    itemname = "ALL ITEM +1";
                    PlayerDataManager.Upexp(exp);
                    PlayerDataManager.AddItem("hpPotion", 1);
                    PlayerDataManager.AddItem("fullHpPotion", 1);
                    PlayerDataManager.AddItem("manaPotion", 1);
                    PlayerDataManager.AddItem("fullManaPotion", 1);
                    PlayerDataManager.AddItem("phoenixFeather", 1);
                    PlayerDataManager.addcash(money);
                    StartCoroutine(showgif());
                }

            }
            else if (!canvastalk.activeSelf && textnum == 0 && boss != null & crab == null)
            {
                canvastalk.SetActive(true);
                text.text = "������ǹ����ͧ�������Թ�";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss != null & crab == null)
            {
                text.text = "任�Һ Umbra ������";
                PlayerDataManager.finishquest(9);
                textnum = 10;
                //gif
                quest2 += 1;
                if (quest2 == 1)
                {
                    itemname = "ALL ITEM +1";
                    money = 500;
                    PlayerDataManager.Upexp(exp);
                    PlayerDataManager.AddItem("hpPotion", 1);
                    PlayerDataManager.AddItem("fullHpPotion", 1);
                    PlayerDataManager.AddItem("manaPotion", 1);
                    PlayerDataManager.AddItem("fullManaPotion", 1);
                    PlayerDataManager.AddItem("phoenixFeather", 1);
                    PlayerDataManager.addcash(money);
                    StartCoroutine(showgif());
                }
            }
            else if (!canvastalk.activeSelf && textnum == 0 && boss == null & crab == null)
            {
                canvastalk.SetActive(true);
                text.text = "��Һ Umbra ����������Թ�����Ҽ�����";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss == null & crab == null)
            {
                canvastalk.SetActive(true);
                text.text = "�ʧ���ҧ�ͧ�Թᴹ Aetherion ��Ѻ�׹������";
                textnum = 10;
            }
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvastalkmenu.SetActive(true);
            cantalk = true;
            textnum = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canvastalkmenu.SetActive(false);
        canvastalk.SetActive(false);
        cantalk = false;
        textnum = 0;
    }
    private IEnumerator showgif()
    {
        canvasgif.SetActive(true);
        textgif.text = $"EPX + {exp}  CASH + {money}\n{itemname}";
        yield return new WaitForSeconds(2f);
        canvasgif.SetActive(false);
    }

    public void chest() 
    {
        int numbers = Random.Range(0, 5);
        int numbersitem = Random.Range(1, 3);
        if (numbers == 0)
        {
            PlayerDataManager.AddItem("hpPotion", numbersitem);
            itemname = $"Item hpPotion + {numbersitem}";
        }
        else if (numbers == 1)
        {
            PlayerDataManager.AddItem("fullHpPotion", numbersitem);
            itemname = $"Item fullHpPotion + {numbersitem}";
        }
        else if (numbers == 2)
        {
            PlayerDataManager.AddItem("manaPotion", numbersitem);
            itemname = $"Item manaPotion + {numbersitem}";
        }
        else if (numbers == 3)
        {
            PlayerDataManager.AddItem("fullManaPotion", numbersitem);
            itemname = $"Item fullManaPotion + {numbersitem}";
        }
        else if (numbers == 4)
        {
            PlayerDataManager.AddItem("phoenixFeather", 1);
            itemname = "Item phoenixFeather +1";
        }
        PlayerDataManager.addcash(money);
        PlayerDataManager.Upexp(exp);
        StartCoroutine(showgif());
    }
}
