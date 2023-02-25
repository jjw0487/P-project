using Unity.Mathematics;
using UnityEngine;

public class Inventory : PointerCheck
{
    public Transform[] slots;
    public Slots[] slotData;
    public EquipmentSlots[] equipSlots;
    public GameObject[] usePanel; // 1.싱글아이템 2.중복아이템 3.버리는아이템(싱글) 4.버리는아이템(중복) 5.상점구입 
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

    public void LoadItemData(int _itemId, int _count) // 세이브로드
    {
        for (int i = 0; i < slotData.Length; i++)
        {
            if (slotData[i].item == null)
            {
                GameObject obj = Instantiate(itemData[_itemId].obj, new Vector3(999f, 999f, 999f), quaternion.identity);
                obj.transform.SetParent(SceneData.Inst.ItemPool);
                slotData[i].SetLoadedItem(obj.GetComponent<Item>(), _count);
                return;
            }
        }
    }

    public void LoadEquipmentItemData(int _itemId) // 세이브로드
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item == null)
            {
                GameObject obj = Instantiate(itemData[_itemId].obj, new Vector3(999f, 999f, 999f), quaternion.identity);
                obj.transform.SetParent(SceneData.Inst.ItemPool);
                equipSlots[i].ChangeSlotByCliicking(obj.GetComponent<Item>());
                return;
            }
        }
    }
}
