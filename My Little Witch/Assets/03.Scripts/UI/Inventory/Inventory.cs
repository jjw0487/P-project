using UnityEngine;

public class Inventory : PointerCheck
{

    public Transform[] slots;
    public GameObject[] usePanel;
    public GameObject floatingItemNotice;
    public Transform eventNotice;
    public TMPro.TMP_Text itemExplnation; // 인벤토리 내에 아이템 설명 란

}
