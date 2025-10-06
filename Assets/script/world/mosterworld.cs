using UnityEngine;
using UnityEngine.SceneManagement;

public class monsterworld : MonoBehaviour
{
    // ชื่อ Prefab ที่ต้องการบันทึกเมื่อหมูป่าชน (ใช้เป็น Key ใน JSON)
    // ตั้งค่าใน Inspector ให้ตรงกับชื่อ Prefab ที่คุณต้องการให้ Spawn ใน Scene ถัดไป
    public string enemyPrefabNameToSave = "";
    bool firstplayerplay;
    string thisname;
    public bool boss = false;
    public Material daySkyboxMaterial;
    public GameObject win;

    // ช่วงจำนวนที่จะสุ่มเมื่อเกิดการชน
    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;

    [Header("Collision/Trigger Settings")]
    public bool useTrigger = false; // true = OnTriggerEnter, false = OnCollisionEnter
    public string collisionTag = "sword"; // Tag ของวัตถุที่หมูป่าจะชนด้วย

    public AudioClip soundattack;
    public GameObject sound;
    AudioSource audiosound;

    public GameObject block;

    public Light myLight;

    void Start()
    {
        thisname = gameObject.name;
        if (PlayerDataManager.IsEnemyDefeated(thisname))
        {
            if (boss) 
            {
                RenderSettings.skybox = daySkyboxMaterial;
                win.SetActive(true);
                if (myLight != null)
                {
                    myLight.intensity = 5f;
                }

                DynamicGI.UpdateEnvironment();
            }
            if (block != null) 
            {
                block.SetActive(false);
            }
            Destroy(gameObject);
        }
            // ตรวจสอบให้แน่ใจว่าได้ตั้งชื่อ Prefab แล้วใน Inspector
            if (string.IsNullOrEmpty(enemyPrefabNameToSave))
        {
            enemyPrefabNameToSave = gameObject.name; // ใช้ชื่อ GameObject เป็นชื่อ Prefab ถ้าไม่ได้ตั้งค่า
        }

        // ตรวจสอบว่ามี Collider อยู่บน GameObject นี้
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError($"Controller on {gameObject.name}: Missing Collider component. Please add a Collider (e.g., BoxCollider, CapsuleCollider) and ensure 'Is Trigger' is set correctly based on 'useTrigger' setting.");
        }
        // ตรวจสอบว่ามี Rigidbody อยู่บน GameObject นี้ ถ้าใช้ OnCollisionEnter 
        if (!useTrigger && GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning($"Controller on {gameObject.name}: Missing Rigidbody component for OnCollisionEnter. Adding one now.");
            gameObject.AddComponent<Rigidbody>().isKinematic = true; // เพิ่ม Rigidbody แบบ Kinematic เพื่อไม่ให้ฟิสิกส์ส่งผล แต่ยังตรวจจับการชนได้
        }
        audiosound = sound.GetComponent<AudioSource>();

    }



    // ใช้เมื่อ Collider เป็น Is Trigger = false
    void OnCollisionEnter(Collision collision)
    {
        if (!useTrigger && collision.gameObject.CompareTag(collisionTag))
        {
            firstplayerplay = true;
            PlayerDataManager.SetWinStatus(false);
            PlayerDataManager.setnamemon(thisname);
            HandleInteraction(collision.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (soundattack != null)
            {
                audiosound.clip = soundattack;
                audiosound.Play();
            }
            firstplayerplay = false;
            PlayerDataManager.SetWinStatus(false);
            PlayerDataManager.setnamemon(thisname);
            HandleInteraction(other.gameObject);
        }
    }

    private void HandleInteraction(GameObject otherObject)
    {
        // สุ่มจำนวนที่จะบันทึก
        int randomCount = Random.Range(minSpawnCount, maxSpawnCount + 1); // +1 เพราะ max ไม่รวมค่าสุดท้าย

        Debug.Log($"collided/triggered with {otherObject.name}! Saving '{enemyPrefabNameToSave}' with count {randomCount}.");
        // บันทึกข้อมูลการชนไปยัง GameDataManager
        GameDataManager.AddOrUpdateEnemyHit(enemyPrefabNameToSave, randomCount, firstplayerplay);
        Cursor.lockState = CursorLockMode.None; // ทำให้เคอร์เซอร์ไม่ถูกล็อกกลางจอ
        Cursor.visible = true;
        // คุณอาจต้องการทำลายหมูป่าตัวนี้ หรือปิดการทำงานของมัน
        gameObject.SetActive(false);
        SceneManager.LoadScene("battle");
    }
}
