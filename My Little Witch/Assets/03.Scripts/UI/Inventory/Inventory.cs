using UnityEngine;

public class Inventory : PointerCheck
{
    [SerializeField]private int Gold;
    public int gold { get { return Gold; } }
    [SerializeField]private TMPro.TMP_Text text;
    public Transform[] slots;
    public GameObject[] usePanel;
    public GameObject floatingItemNotice;
    public Transform eventNotice;

    private void Start()
    {
        Gold = 0;
        text.text = Gold.ToString();
    }
    public void SetGold(int gold)
    {
        Gold += gold;
        text.text = Gold.ToString();
    }

    public void PurchaseItem(int paid)
    {
        Gold -= paid;
        text.text = Gold.ToString();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Gold += 1000;
            text.text = Gold.ToString();
        }
    }
}
