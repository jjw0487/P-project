using UnityEngine;

public class Inventory : PointerCheck
{

    public Transform[] slots;
    public EquipmentSlots[] equipSlots;
    public GameObject[] usePanel;
    public GameObject floatingItemNotice;
    public Transform eventNotice;
    public TMPro.TMP_Text itemExplnation; // �κ��丮 ���� ������ ���� ��
    public QuickSlotManager quickSlotManager;

}
