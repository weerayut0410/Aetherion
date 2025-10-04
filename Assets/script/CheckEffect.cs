using TMPro;
using UnityEngine;

public class CheckEffect : MonoBehaviour
{
    public GameObject player;
    Character Player;
    public bool effect1 = false;
    public bool effect2 = false;
    public bool effect3 = false;
    public bool effect4 = false;
    public bool effect5 = false;
    public GameObject E1;
    public GameObject E2;
    public GameObject E3;
    public GameObject E4;
    public GameObject E5;
    public TextMeshProUGUI textE2;
    public TextMeshProUGUI textE3;
    public TextMeshProUGUI textE4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = player.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        effect1 = TurnManager.Instance.statusEffectManager.HasBlessing(Player);

        effect2 = Player.shield || Player.taunt || Player.sanctuary;

        effect3 = Player.posion > 0 || Player.crabstack > 0;

        effect4 = TurnManager.Instance.statusEffectManager.HasBleeding(Player) || TurnManager.Instance.statusEffectManager.Hascrush(Player) || Player.weaknesses.Contains("Fire");
        
        effect5 = TurnManager.Instance.statusEffectManager.HasDarkHowl(Player);

        if (effect1)
        {
            E1.SetActive(true);
        }
        else { E1.SetActive(false); }

        if (effect2) 
        {
            E2.SetActive(true);
            if (Player.taunt)
            {
                textE2.text = "T";
            }
            else { textE2.text = "S"; }
        }
        else { E2.SetActive(false); }

        if (effect3)
        {
            E3.SetActive(true);
            if (Player.posion > 0)
            {
                textE3.text = Player.posion.ToString();
            }
            else { textE3.text = Player.crabstack.ToString(); }
        }
        else { E3.SetActive(false); }

        if (effect4)
        {
            E4.SetActive(true);
            if (TurnManager.Instance.statusEffectManager.HasBleeding(Player))
            {
                textE4.text = "B";
            }
            else if (TurnManager.Instance.statusEffectManager.Hascrush(Player))
            { 
                textE4.text = "C"; 
            }
            else
            {
                textE4.text = "F";
            }
        }
        else { E4.SetActive(false); }

        if (effect5)
        {
            E5.SetActive(true);
        }
        else { E5.SetActive(false); }

    }
}
