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

    public TMPro.TMP_Text[] playerStatus; // 0.���� 1.�ʿ����ġ 2.�������ݷ� 3.���� 4.�ִ�ü�� 5.�ִ븶��
    public TMPro.TMP_Text[] playerAddedStat;

}
