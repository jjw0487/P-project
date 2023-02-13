using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
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
    public void FloatNotice(string itemName)
    {
        GameObject obj = Instantiate(SceneData.Inst.Inven.floatingItemNotice, SceneData.Inst.Inven.eventNotice);
        SceneData.Inst.Inven.floatingItemNotice.GetComponent<NotificationController>().GetText(itemName);
    }
    public void AddItem(Item _item, int _count)
    {
        if (orgSprite == null) { orgSprite = GetComponent<Image>().sprite; }
        // ���� �̹����� �ּ� ������ clear �� �� �ٿ�����
        item = _item;
        if(_item.myItem.orgData.name == item.myItem.orgData.name)
        {
            AddCount(_count);
        }
        img.sprite = _item.myItem.orgData.sprite;
    }

    public void AddCount(int howmany)
    {
        itemCount += howmany;
        item.myItem.curNumber = itemCount;
        count.text = item.myItem.curNumber.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
                {
                    if (itemCount > 1)
                    {
                        SceneData.Inst.Inven.usePanel[1].SetActive(true);
                        SceneData.Inst.Inven.usePanel[1].GetComponent<ItemAmountReturn>().GetItemInfo(this, itemCount);
                    }
                    else
                    {
                        SceneData.Inst.Inven.usePanel[0].SetActive(true);
                        SceneData.Inst.Inven.usePanel[0].GetComponent<DecisionReturn>().ReturnDecision(this);
                    }
                }
                else if (item.myItem.orgData.itemType == ItemData.ItemType.Interactable)
                {
                    SceneData.Inst.Inven.usePanel[0].SetActive(true);
                    SceneData.Inst.Inven.usePanel[0].GetComponent<DecisionReturn>().ReturnDecision(this);
                }
                
                // ���͸��� Ÿ���� ���� ���ϵ���
            }

        }
    }

   
    public void UseItems(int howmany)
    {
        SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value*howmany);
        itemCount -= howmany;
        if (itemCount == 0) { Destroy(item.gameObject); ClearSlot(); }
        else
        {
            item.myItem.curNumber = itemCount;
            count.text = item.myItem.curNumber.ToString();
        }
    }

    public void UseSingleItem()
    {
        if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
        {
            SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value);
            Destroy(item.gameObject);
            ClearSlot();
        }
        else if (item.myItem.orgData.itemType == ItemData.ItemType.Interactable)
        {//�������ڴ� �� �� �� ������ ����(single, plural)�� ���� �����Ŷ� �̷��� ����

            for (int i = 0; i < item.contents.Length; i++)
            {
                item.GetItemFromInteractableItems(item.contents[i]);
            }
            itemCount -= 1;
            if (itemCount == 0) { Destroy(item.gameObject); ClearSlot(); }
            else
            {
                item.myItem.curNumber = itemCount;
                count.text = item.myItem.curNumber.ToString();
            }
            
        }
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
        itemCount = 0;
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
