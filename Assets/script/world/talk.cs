using TMPro;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = "";
        numquest = PlayerDataManager.getnumquest();
    }
    private void Update()
    {
        switch (numquest)
        {
            case 2:
                if (boar1 == null && boar2 == null)
                    PlayerDataManager.finishquest(3);   // จบหมูป่า -> ไปด่านเห็ด (3)
                break;

            case 3:
                if (mushroom == null)
                    PlayerDataManager.finishquest(4);   // จบเห็ด -> ไปด่านหมาป่า (4)
                break;

            case 4:
                if (wolf == null)
                    PlayerDataManager.finishquest(5);   // จบหมาป่า -> กลับหาพ่อมด2 (5)
                break;

            case 5:
                // ตรงนี้คือ “คุยพ่อมด2” ให้ที่อื่นเป็นคนอัปเป็น 6 (เช่นตอนคุยเสร็จ)
                break;

            case 6:
                if (bear == null)
                    PlayerDataManager.finishquest(7);   // จบหมี -> ไปด่านปู (7)
                break;

            case 7:
                if (crab == null)                       // ชื่อเดิมพิมพ์ว่า carb ให้เช็คให้ตรงตัวแปร
                    PlayerDataManager.finishquest(8);   // จบปู -> กลับหาพ่อมด3 (8)
                break;

            case 8:
                // “คุยพ่อมด3” ให้ที่อื่นอัปเป็น 9
                break;

            case 9:
                if (boss == null)
                    PlayerDataManager.finishquest(10);  // จบบอส -> ปิดมิชชั่น (10)
                break;
        }


        if (Input.GetKeyDown(KeyCode.F) && cantalk)
        {
            // ถ้า canvastalk กำลังแสดงอยู่ ให้ปิด
            if (canvastalk.activeSelf && textnum == 10)
            {
                print("ปิด canvastalk");
                canvastalk.SetActive(false);

            }
            // ถ้า canvastalk ไม่ได้แสดงอยู่ ให้เปิด
            else if (!canvastalk.activeSelf && textnum == 0 && boss != null & wolf != null) 
            {
                canvastalk.SetActive(true);
                text.text = "ดินแดน Aetherion เคยรุ่งเรื่อง...จนกระทั่งเงามืดกลืนกินทุกสิ่ง";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss != null & wolf != null) 
            {
                text.text = "แสงสว่างจะกลับคืนมาได้ก็ต่อเมื่อปราบ Umbra แล้วเท่านั้น";
                textnum = 2;
            }
            else if (canvastalk.activeSelf && textnum == 2 && boss != null & wolf != null)
            {
                text.text = "การจะไปหา Umbra ได้นั้น จะต้องรวบรวมชิ้นส่วนทั้ง 2 ชิ้น";
                textnum = 3;
            }
            else if (canvastalk.activeSelf && textnum == 3 && boss != null & wolf != null)
            {
                text.text = "ชิ้นแรกอยู่กับสัตว์ดุร้ายในป่าลึก จงไปนำชิ้นส่วนแรกมา";
                PlayerDataManager.finishquest(2);
                print(PlayerDataManager.getnumquest());
                textnum = 10;
                
            }
            else if (!canvastalk.activeSelf && textnum == 0 && boss != null & wolf == null & crab != null)
            {
                canvastalk.SetActive(true);
                text.text = "ได้ชิ้นส่วนแรกมาแล้วสินะ";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss != null & wolf == null & crab != null)
            {
                text.text = "ข่าวของชิ้นส่วนที่สอง อยู่ที่แอ่งน้ำ \nเดินตามสายน้ำทางขวา ซึ่งมีหมีขวางทางอยู่";
                textnum = 2;
            }

            else if (canvastalk.activeSelf && textnum == 2 && boss != null & wolf == null & crab != null)
            {
                text.text = "จงไปนำชิ้นส่วนที่สองมา";
                PlayerDataManager.finishquest(6);
                textnum = 10;
                
            }
            else if (!canvastalk.activeSelf && textnum == 0 && boss != null & crab == null)
            {
                canvastalk.SetActive(true);
                text.text = "ได้ชิ้นส่วนทั้งสองมาแล้วสินะ";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss != null & crab == null)
            {
                text.text = "ไปปราบ Umbra ให้ได้นะ";
                PlayerDataManager.finishquest(9);
                textnum = 10;
                
            }
            else if (!canvastalk.activeSelf && textnum == 0 && boss == null & crab == null)
            {
                canvastalk.SetActive(true);
                text.text = "ปราบ Umbra สำเร็จแล้วสินะเหล่าผู้กล้า";
                textnum = 1;
            }
            else if (canvastalk.activeSelf && textnum == 1 && boss == null & crab == null)
            {
                canvastalk.SetActive(true);
                text.text = "แสงสว่างของดินแดน Aetherion กลับคืนมาแล้ว";
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
}
