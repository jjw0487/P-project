using UnityEngine;

public class Inventory : PointerCheck
{

    public Transform[] slots;
    public EquipmentSlots[] equipSlots;
    public GameObject[] usePanel;
    public GameObject floatingItemNotice;
    public Transform eventNotice;
    public TMPro.TMP_Text itemExplnation; // 인벤토리 내에 아이템 설명 란
    public QuickSlotManager quickSlotManager;

    public TMPro.TMP_Text[] playerStatus; // 0.레벨 1.필요경험치 2.마법공격력 3.방어력 4.최대체력 5.최대마력
    public TMPro.TMP_Text[] playerAddedStat;

}
