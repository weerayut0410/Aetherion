using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    public TextMeshProUGUI textnext;
    public TextMeshProUGUI texttip;
    public Image imageshow;
    public Sprite tip1;
    public Sprite tip2;
    public Sprite tip3;
    public Sprite tip4;
    public Sprite tip5;
    public Sprite tip6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imageshow.sprite = tip1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
