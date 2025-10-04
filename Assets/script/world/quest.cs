using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject mission;
    int numquest;
    public GameObject canvasnpc;
    public GameObject canvasboar1;
    public GameObject canvasboar2;
    public GameObject canvasmushroom;
    public GameObject canvaswolf;
    public GameObject canvasbear;
    public GameObject canvascrab;
    public GameObject canvasboss;
    public GameObject boar1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = "";
        canvasnpc.SetActive(false);
        canvasboar1.SetActive(false);
        canvasboar2.SetActive(false);
        canvasmushroom.SetActive(false);
        canvaswolf.SetActive(false);
        canvasbear.SetActive(false);
        canvascrab.SetActive(false);
        canvasboss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (boar1 == null)
        {
            if (canvasboar2 != null)
            {
                canvasboar2.SetActive(true);
            }
        }
        numquest = PlayerDataManager.getnumquest();
        if (numquest == 1)
        {
            text.text = "��� �����1";
            canvasnpc.SetActive(true);
        }
        else if (numquest == 2)
        {
            text.text = "�ӨѴ ��ٻ�� \n2 ���";
            canvasnpc.SetActive(false);
            if (canvasboar1 != null)
            {
                canvasboar1.SetActive(true);
            }
            
        }
        else if (numquest == 3)
        {
            text.text = "�ӨѴ ��� \n1 ���";
            canvasmushroom.SetActive(true);
        }
        else if (numquest == 4)
        {
            text.text = "�ӨѴ ��һ�� \n1 ���";
            canvaswolf.SetActive(true);
        }
        else if (numquest == 5)
        {
            text.text = "��Ѻ��� �����2";
            canvasnpc.SetActive(true);
        }
        else if (numquest == 6)
        {
            text.text = "�ӨѴ ��� \n1 ���";
            canvasnpc.SetActive(false);
            canvasbear.SetActive(true);
        }
        else if (numquest == 7)
        {
            text.text = "�ӨѴ �� \n1 ���";
            canvascrab.SetActive(true);
        }
        else if (numquest == 8)
        {
            text.text = "��Ѻ��� �����3";
            canvasnpc.SetActive(true);
        }
        else if (numquest == 9)
        {
            text.text = "��Һ Umbra";
            canvasnpc.SetActive(false);
            canvasboss.SetActive(true);
        }
        else if (numquest == 10)
        {
            mission.SetActive(false);
        }
    }
}
