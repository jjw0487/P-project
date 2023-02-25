using Unity.Mathematics;
using UnityEngine;

public class Inventory : PointerCheck
{
    public Transform[] slots;
    public Slots[] slotData;
    public EquipmentSlots[] equipSlots;
    public GameObject[] usePanel; // 1.�̱۾����� 2.�ߺ������� 3.�����¾�����(�̱�) 4.�����¾�����(�ߺ�) 5.�������� 
    public GameObject floatingItemNotice;
    public Transform eventNotice;
    public TMPro.TMP_Text itemExplnation; // �κ��丮 ���� ������ ���� ��
    public QuickSlotManager quickSlotManager;

    public TMPro.TMP_Text[] playerStatus; // 0.���� 1.�ʿ����ġ 2.�������ݷ� 3.���� 4.�ִ�ü�� 5.�ִ븶��
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

    public void LoadItemData(int _itemId, int _count) // ���̺�ε�
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

    public void LoadEquipmentItemData(int _itemId) // ���̺�ε�
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
