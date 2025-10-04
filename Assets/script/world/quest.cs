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
            text.text = "ไปหา พ่อมด1";
            canvasnpc.SetActive(true);
        }
        else if (numquest == 2)
        {
            text.text = "กำจัด หมูป่า \n2 ตัว";
            canvasnpc.SetActive(false);
            if (canvasboar1 != null)
            {
                canvasboar1.SetActive(true);
            }
            
        }
        else if (numquest == 3)
        {
            text.text = "กำจัด เห็ด \n1 ตัว";
            canvasmushroom.SetActive(true);
        }
        else if (numquest == 4)
        {
            text.text = "กำจัด หมาป่า \n1 ตัว";
            canvaswolf.SetActive(true);
        }
        else if (numquest == 5)
        {
            text.text = "กลับไปหา พ่อมด2";
            canvasnpc.SetActive(true);
        }
        else if (numquest == 6)
        {
            text.text = "กำจัด หมี \n1 ตัว";
            canvasnpc.SetActive(false);
            canvasbear.SetActive(true);
        }
        else if (numquest == 7)
        {
            text.text = "กำจัด ปู \n1 ตัว";
            canvascrab.SetActive(true);
        }
        else if (numquest == 8)
        {
            text.text = "กลับไปหา พ่อมด3";
            canvasnpc.SetActive(true);
        }
        else if (numquest == 9)
        {
            text.text = "ปราบ Umbra";
            canvasnpc.SetActive(false);
            canvasboss.SetActive(true);
        }
        else if (numquest == 10)
        {
            mission.SetActive(false);
        }
    }
}
