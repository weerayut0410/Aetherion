using TMPro;
using UnityEngine;

public class buy : MonoBehaviour
{
    public GameObject canvasmenu;
    public GameObject canvastalkmenu;
    public TextMeshProUGUI textcash;

    public TextMeshProUGUI textitem1;
    public TextMeshProUGUI textitem2;
    public TextMeshProUGUI textitem3;
    public TextMeshProUGUI textitem4;
    public TextMeshProUGUI textitem5;
    public bool canbuy = false;

    int cash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cash = PlayerDataManager.Getcash();
        textcash.text = $"CASH {cash.ToString()}";

        if (Input.GetKeyDown(KeyCode.F) && canbuy)
        {
            canvasmenu.SetActive(true);
            
        }
        textitem1.text = PlayerDataManager.GetItemCount("hppotion").ToString();
        textitem2.text = PlayerDataManager.GetItemCount("manaPotion").ToString();
        textitem3.text = PlayerDataManager.GetItemCount("fullHpPotion").ToString();
        textitem4.text = PlayerDataManager.GetItemCount("fullManaPotion").ToString();
        textitem5.text = PlayerDataManager.GetItemCount("phoenixFeather").ToString();
    }

    public void phoenix()
    {
        if (cash >= 300)
        {
            PlayerDataManager.AddItem("phoenixFeather", 1);
            PlayerDataManager.usecash(300);
        }
    }
    public void hppo()
    {
        if (cash >= 100)
        {
            PlayerDataManager.AddItem("hpPotion", 1);
            PlayerDataManager.usecash(100);
        }
    }
    public void mppo()
    {
        if (cash >= 100)
        {
            PlayerDataManager.AddItem("manaPotion", 1);
            PlayerDataManager.usecash(100);
        }
    }
    public void fullhppo()
    {
        if (cash >= 200)
        {
            PlayerDataManager.AddItem("fullHpPotion", 1);
            PlayerDataManager.usecash(200);
        }
    }
    public void fullmppo()
    {
        if (cash >= 200)
        {
            PlayerDataManager.AddItem("fullManaPotion", 1);
            PlayerDataManager.usecash(200);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvastalkmenu.SetActive(true);
            canbuy = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canvastalkmenu.SetActive(false);
        canbuy = false; 
    }
}
