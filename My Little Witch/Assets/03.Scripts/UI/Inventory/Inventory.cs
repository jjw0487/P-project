using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Inventory : PointerCheck
{
    public Transform[] slots;
    public Slots[] slotData;
    public EquipmentSlots[] equipSlots;
    public GameObject[] usePanel;
    public GameObject floatingItemNotice;
    public Transform eventNotice;
    public TMPro.TMP_Text itemExplnation; // 인벤토리 내에 아이템 설명 란
    public QuickSlotManager quickSlotManager;

    public TMPro.TMP_Text[] playerStatus; // 0.레벨 1.필요경험치 2.마법공격력 3.방어력 4.최대체력 5.최대마력
    public TMPro.TMP_Text[] playerAddedStat;

    [SerializeField] private ItemData[] itemData;

    public void ClearSlots()
    {
        for (int i = 0; i < slotData.Length; i++)
        {
            if (slotData[i].item != null)
            {
                slotData[i].ClearSlot();
            }
        }

        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item != null)
            {
                equipSlots[i].ClearSlot();
            }
        }

    }

    public void LoadItemData(int itemId, int count)
    {
        for(int i = 0; i < slotData.Length; i++)
        {
            if (slotData[i].item == null)
            {
                slotData[i].AddItem(itemData[itemId].obj.GetComponent<Item>(), count);
                return;
            }
        }
    }

    public void LoadEquipmentItemData(int itemId)
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item == null)
            {
                equipSlots[i].ChangeSlotByCliicking(itemData[itemId].obj.GetComponent<Item>());
                return;
            }
        }
    }
}
