using UnityEngine;
// ไม่จำเป็นต้องใช้ using Cinemachine; ใน ClickSelector นี้โดยตรง
// เพราะเราจะอ้างอิงถึง Camera.main ซึ่งเป็นกล้องหลักที่ถูกจัดการโดย CinemachineBrain

public class ClickSelector : MonoBehaviour
{
    public GameObject target;            // เก็บ GameObject ที่คลิก
    public Character selectedCharacter;  // เก็บ Character ที่เกี่ยวข้อง

    private GameObject currentHoveredObject;
    private Outline lastOutline;         // outline ล่าสุดที่ถูกเปิด

    // *** เปลี่ยนจาก public Camera cam; เป็น private Camera mainCamera; ***
    private Camera mainCamera;

    public bool showOutline = false;

    void Start()
    {
        target = null;
        // *** กำหนดค่า mainCamera ใน Start() ***
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Make sure your Main Camera has the 'MainCamera' tag.");
        }
    }

    void Update()
    {
        if (!showOutline)
        {
            if (lastOutline != null)
            {
                lastOutline.enabled = false;
                lastOutline = null;
            }
            return;
        }

        // ส่วนนี้ทำงานเฉพาะตอนเปิด showOutline
        if (lastOutline != null)
        {
            lastOutline.enabled = false;
            lastOutline = null;
        }

        // *** ใช้ mainCamera แทน cam ***
        if (mainCamera == null) return; // ป้องกัน Null Reference Exception ถ้าหา Main Camera ไม่เจอ

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hoveredObject = hit.collider.gameObject;

            if (hoveredObject.CompareTag("enemy") || hoveredObject.CompareTag("Player"))
            {
                Outline outline = hoveredObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                    lastOutline = outline;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    target = hoveredObject;
                    selectedCharacter = hoveredObject.GetComponentInParent<Character>();
                }
            }
        }
    }
}