using UnityEngine;
using UnityEngine.SceneManagement;

public class monsterworld : MonoBehaviour
{
    // ���� Prefab ����ͧ��úѹ�֡�������ٻ�Ҫ� (���� Key � JSON)
    // ��駤��� Inspector ���ç�Ѻ���� Prefab ���س��ͧ������ Spawn � Scene �Ѵ�
    public string enemyPrefabNameToSave = "";
    bool firstplayerplay;
    string thisname;
    public bool boss = false;
    public Material daySkyboxMaterial;
    public GameObject win;

    // ��ǧ�ӹǹ��������������Դ��ê�
    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;

    [Header("Collision/Trigger Settings")]
    public bool useTrigger = false; // true = OnTriggerEnter, false = OnCollisionEnter
    public string collisionTag = "sword"; // Tag �ͧ�ѵ�ط����ٻ�ҨЪ�����

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
            // ��Ǩ�ͺ������������駪��� Prefab ����� Inspector
            if (string.IsNullOrEmpty(enemyPrefabNameToSave))
        {
            enemyPrefabNameToSave = gameObject.name; // ����� GameObject �繪��� Prefab ���������駤��
        }

        // ��Ǩ�ͺ����� Collider ���躹 GameObject ���
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError($"Controller on {gameObject.name}: Missing Collider component. Please add a Collider (e.g., BoxCollider, CapsuleCollider) and ensure 'Is Trigger' is set correctly based on 'useTrigger' setting.");
        }
        // ��Ǩ�ͺ����� Rigidbody ���躹 GameObject ��� ����� OnCollisionEnter 
        if (!useTrigger && GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning($"Controller on {gameObject.name}: Missing Rigidbody component for OnCollisionEnter. Adding one now.");
            gameObject.AddComponent<Rigidbody>().isKinematic = true; // ���� Rigidbody Ẻ Kinematic ������������ԡ���觼� ���ѧ��Ǩ�Ѻ��ê���
        }
        audiosound = sound.GetComponent<AudioSource>();

    }



    // ������� Collider �� Is Trigger = false
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
        // �����ӹǹ���кѹ�֡
        int randomCount = Random.Range(minSpawnCount, maxSpawnCount + 1); // +1 ���� max ����������ش����

        Debug.Log($"collided/triggered with {otherObject.name}! Saving '{enemyPrefabNameToSave}' with count {randomCount}.");
        // �ѹ�֡�����š�ê���ѧ GameDataManager
        GameDataManager.AddOrUpdateEnemyHit(enemyPrefabNameToSave, randomCount, firstplayerplay);
        Cursor.lockState = CursorLockMode.None; // ����������������١��͡��ҧ��
        Cursor.visible = true;
        // �س�Ҩ��ͧ��÷������ٻ�ҵ�ǹ�� ���ͻԴ��÷ӧҹ�ͧ�ѹ
        gameObject.SetActive(false);
        SceneManager.LoadScene("battle");
    }
}
