using UnityEngine;

public class chest : MonoBehaviour
{
    Animator animator;
    public GameObject canvasmenu;
    bool open= false;

    public talk talkScript;
    string thisname;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisname = gameObject.name;
        animator = GetComponent<Animator>();
        if (PlayerDataManager.IsEnemyDefeated(thisname))
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && open)
        {
            animator.SetBool("open",true);
            talkScript.chest();
            open = false;
            PlayerDataManager.AddDefeatedEnemy(thisname);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasmenu.SetActive(true);
            if (!animator.GetBool("open"))
            {
                open = true;
            }
            

        }
    }
    private void OnTriggerExit(Collider other)
    {
        canvasmenu.SetActive(false);
        open = false;
    }
}
