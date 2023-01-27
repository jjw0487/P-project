using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    [SerializeField] private Image img;
    //private Image previousImg;
    public TMPro.TMP_Text count;
    private int itemCount = 0;
    private Sprite orgSprite;


    void Start()
    {
        if (GetComponent<Item>() != null) // �������� ���� ������ ���
        {
            item = GetComponentInChildren<Item>();
            itemCount = GetComponentInChildren<Item>().myItem.curNumber;
        }
    }


    /*public void ShowingSprite()
    {
        img.sprite = GetComponentInChildren<Item>().myItem.orgData.sprite; // ���������� ǥ��
        count.text = GetComponentInChildren<Item>().myItem.curNumber.ToString(); // ���� ��Ʈ�� ��Ʈ������
        itemCount = GetComponentInChildren<Item>().myItem.curNumber; // ��ũ��Ʈ �ȿ��� ��� �� ���� Ȯ��
        item = GetComponentInChildren<Item>();
    }*/

    public void AddItem(Item _item, int _count)
    {
        if (orgSprite == null) { orgSprite = GetComponent<Image>().sprite; }
        // ���� �̹����� �ּ� ������ clear �� �� �ٿ�����
        item = _item;
        AddCount(_count);
        img.sprite = _item.myItem.orgData.sprite;
    }

    public void AddCount(int howmany)
    {
        itemCount += howmany;
        item.myItem.curNumber = itemCount;
        count.text = item.myItem.curNumber.ToString();
    }

    void logic()
    {
        // �巡�׿� ������ �������� �� �� �̻��� ��
        if (itemCount > 1)
        {
            // �巡�� �� �������� �� �� �̻��� �� ��� �̵� �� �Ҹ� �� ������ �����.
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
                {
                    SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value);
                    Destroy(item.gameObject);
                    ClearSlot();
                    /*if (itemCount > 1)
                    {
                        //�� �� ����Ͻðڽ��ϱ�?
                    }
                    else
                    {
                        //�������� ����Ͻðڽ��ϱ�?
                    }*/
                }
                else if (item.myItem.orgData.itemType == ItemData.ItemType.Interactable)
                {
                    for (int i = 0; i < item.contents.Length; i++)
                    {
                        item.GetItemFromInteractableItems(item.contents[i]);
                    }
                    Destroy(item.gameObject);
                    ClearSlot();
                }
                else if (item.myItem.orgData.itemType == ItemData.ItemType.Material)
                {

                }
            }

        }
    }

    void UseConsumableItem(int howmany)
    {
        SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value);
        Destroy(item.gameObject);
        ClearSlot();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragImage.Inst.dragSlot = this;
            DragImage.Inst.DragSetImage(img);
            DragImage.Inst.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragImage.Inst.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Inst.SetColor(0);
        DragImage.Inst.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    { // �ٸ� ���� ��ġ�� ������ ��

        if (DragImage.Inst.dragSlot != null) { ChangeSlot(); }
    }


    private void ChangeSlot()
    {

        Item temp = item; // �������� ���� ������ �����
        int tempItemCount = itemCount;
        AddItem(DragImage.Inst.dragSlot.item, DragImage.Inst.dragSlot.itemCount);

        if (temp != null)
        {
            DragImage.Inst.dragSlot.AddItem(temp, tempItemCount);
        }
        else
        {
            DragImage.Inst.dragSlot.ClearSlot();
        }

    }

    private void ClearSlot()
    {
        item = null;
        img.sprite = orgSprite;
        count.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData) // ���콺�� ������ ���� ���� ��
    {
        if (item != null)
        {
            SceneData.Inst.itemExplnation.text = item.myItem.orgData.explanation;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            SceneData.Inst.itemExplnation.text = "";
        }
    }
}
