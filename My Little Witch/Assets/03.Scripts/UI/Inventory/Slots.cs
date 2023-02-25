using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    [SerializeField] protected Image img;
    //private Image previousImg;
    public TMPro.TMP_Text count;
    protected Sprite orgSprite;

    private bool isDone = false;

    void Start()
    {
        if (GetComponent<Item>() != null) // �������� ���� ������ ���
        {
            item = GetComponentInChildren<Item>();
        }
    }
    public void FloatNotice(string itemName)
    {
        GameObject obj = Instantiate(SceneData.Inst.Inven.floatingItemNotice, SceneData.Inst.Inven.eventNotice);
        //SceneData.Inst.Inven.floatingItemNotice.GetComponent<NotificationController>().GetText(itemName);
        obj.GetComponent<NotificationController>().GetText(itemName);
    }
    

    public virtual void AddItem(Item _item, int _count)
    {
        if (orgSprite == null) { orgSprite = GetComponent<Image>().sprite; }
        // ���� �̹����� �ּ� ������ clear �� �� �ٿ�����
        item = _item;
        item.curNumber = _item.curNumber;
        count.text = item.curNumber.ToString();
        img.sprite = _item.myItem.orgData.sprite;
    }

    protected virtual void AddItemByDragging(Item _item)
    {
        if (orgSprite == null) { orgSprite = GetComponent<Image>().sprite; }
        // ���� �̹����� �ּ� ������ clear �� �� �ٿ�����
        item = _item;
        item.curNumber = _item.curNumber;
        count.text = item.curNumber.ToString();
        img.sprite = item.myItem.orgData.sprite;
    }


    public void SetLoadedItem(Item _item, int _count)
    {
        item = _item;
        img.sprite = _item.myItem.orgData.sprite;
        item.curNumber = _count;
        if (count != null) count.text = item.curNumber.ToString();

    }

    public void AddCount(int howmany)
    {

        item.curNumber += howmany;
        if (count != null) count.text = item.curNumber.ToString();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
                {
                    if (item.curNumber > 1)
                    {
                        SceneData.Inst.Inven.usePanel[1].SetActive(true);
                        SceneData.Inst.Inven.usePanel[1].GetComponent<ItemAmountReturn>().GetItemInfo(this, item.curNumber);
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
                else if (item.myItem.orgData.itemType == ItemData.ItemType.Equipment)
                {

                    for (int n = 0; n < SceneData.Inst.Inven.equipSlots.Length; ++n)
                    {
                        if (SceneData.Inst.Inven.equipSlots[n].item != null && SceneData.Inst.Inven.equipSlots[n].item.myItem.orgData.itemName == this.item.myItem.orgData.itemName)
                        //�������� null�� �ƴϰų� �̹� �����ϴ� �������� ���
                        {
                            return;
                        }
                    }

                    for (int i = 0; i < SceneData.Inst.Inven.equipSlots.Length; ++i) // ���� ������ŭ �ݺ�
                    {
                        if (SceneData.Inst.Inven.equipSlots[i].item == null)
                        {

                            SceneData.Inst.Inven.equipSlots[i].ChangeSlotByCliicking(this.item);
                            this.ClearSlot();
                            break; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
                        }
                    }
                }

                // ���͸��� Ÿ���� ���� ���ϵ���
            }

        }
    }


    public void UseItems(int howmany)
    {
        SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value * howmany);
        item.curNumber -= howmany;
        if (item.curNumber == 0) { if (item.gameObject != null) { /**/ ClearSlot(); } }
        else
        {
            count.text = item.curNumber.ToString();
        }
    }

    public void UseSingleItem()
    {
        if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
        {
            SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value);
            if (item.gameObject != null)
            {
/**/
                ClearSlot();
            }
        }
        else if (item.myItem.orgData.itemType == ItemData.ItemType.Interactable)
        {//�������ڴ� �� �� �� ������ ����(single, plural)�� ���� �����Ŷ� �̷��� ����

            for (int i = 0; i < item.contents.Length; i++)
            {
                item.GetItemFromInteractableItems(item.contents[i]);
            }
            item.curNumber -= 1;

            if (item.curNumber == 0) { /*Destroy(item.gameObject);*/ ClearSlot(); }
            else
            {
                count.text = item.curNumber.ToString();
            }

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragImage.Inst.dragSlot = this;
            DragImage.Inst.DragSetImage(img);
            DragImage.Inst.dragSlot.item.curNumber = item.curNumber;
            DragImage.Inst.transform.position = eventData.position;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragImage.Inst.transform.position = eventData.position;
            DragImage.Inst.transform.SetAsLastSibling();
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Inst.SetColor(0);
        DragImage.Inst.dragSlot = null;
        if (SceneData.Inst.myPlayer.OnUI == false)
        {
            ThrowItemAway();
        }

    }

    public virtual void OnDrop(PointerEventData eventData)
    { // �ٸ� ���� ��ġ�� ������ ��

        if (DragImage.Inst.dragSlot != null) { ChangeSlot(); }
    }

    protected virtual void ChangeSlot()
    {
        if (item != null) 
        {
            Item temp = item; // �������� ���� ������ �����
            AddItemByDragging(DragImage.Inst.dragSlot.item);
            DragImage.Inst.dragSlot.AddItemByDragging(temp);
        }
        else
        {

            AddItemByDragging(DragImage.Inst.dragSlot.item);
            DragImage.Inst.dragSlot.ClearSlot();
        }
/*
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
        }*/



    }

    public virtual void ClearSlot()
    {
        //if(item != null) item.curNumber = 0;
        item = null;
        if (orgSprite == null) { orgSprite = GetComponent<Image>().sprite; }
        img.sprite = orgSprite;
        count.text = "";
    }

    public virtual void OnPointerEnter(PointerEventData eventData) // ���콺�� ������ ���� ���� ��
    {
        if (item != null)
        {
            SceneData.Inst.Inven.itemExplnation.text = item.myItem.orgData.explanation;

        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            SceneData.Inst.Inven.itemExplnation.text = "";
        }
    }


    public void ThrowAwaySingleItem()
    {

        if (SceneData.Inst.ItemPool.GetComponentInChildren<Item>()) // ������Ǯ�� �������� �ϳ��� ���� ��
        {
            // �������� ã�ƺ��� �̸��� ���� �������� �ִٸ� SceneData.Inst.myPlayer.transform.position;
            for (int i = 0; i < SceneData.Inst.ItemPool.childCount; i++)
            {
                if (SceneData.Inst.ItemPool.GetChild(i).GetComponent<Item>().myItem.orgData.itemName
                    == item.myItem.orgData.itemName)
                {
                    SceneData.Inst.ItemPool.GetChild(i).transform.position = SceneData.Inst.myPlayer.transform.position + new Vector3(0f, 1f, 0.5f);
                    ClearSlot();
                    return;
                }
            }
            GameObject obj = Instantiate(item.gameObject, SceneData.Inst.myPlayer.transform.position + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
            obj.transform.SetParent(SceneData.Inst.ItemPool);
        }
        else
        {
            GameObject obj = Instantiate(item.gameObject, SceneData.Inst.myPlayer.transform.position + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
            obj.transform.SetParent(SceneData.Inst.ItemPool);
        }
        ClearSlot();
    }

    public void ThrowItemsAway(int howmany)
    {
        if (SceneData.Inst.ItemPool.GetComponentInChildren<Item>()) // ������Ǯ�� �������� �ϳ��� ���� ��
        {
            isDone = false;
            for (int i = 0; i < SceneData.Inst.ItemPool.childCount; i++)
            {
                if (SceneData.Inst.ItemPool.GetChild(i).GetComponent<Item>().myItem.orgData.itemId
                    == item.myItem.orgData.itemId)
                {
                    SceneData.Inst.ItemPool.GetChild(i).transform.position = SceneData.Inst.myPlayer.transform.position + new Vector3(0f, 1f, 0.5f);
                    SceneData.Inst.ItemPool.GetChild(i).GetComponent<Item>().curNumber = howmany;
                    isDone = true;
                    break;
                }
            }
            if (!isDone)
            {
                GameObject obj = Instantiate(item.gameObject, SceneData.Inst.myPlayer.transform.position + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
                obj.GetComponent<Item>().curNumber = howmany;
                obj.transform.SetParent(SceneData.Inst.ItemPool);
            }

        }
        else
        {
            GameObject obj = Instantiate(item.gameObject, SceneData.Inst.myPlayer.transform.position + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
            obj.GetComponent<Item>().curNumber = howmany;
            obj.transform.SetParent(SceneData.Inst.ItemPool);
        }

        item.curNumber -= howmany;

        if (item.curNumber == 0) { ClearSlot(); }
        else
        {

            count.text = item.curNumber.ToString();
        }
    }

    private void ThrowItemAway()
    {
        if (item != null)
        {
            if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
            {
                if (item.curNumber > 1)
                {
                    SceneData.Inst.Inven.usePanel[3].SetActive(true);
                    SceneData.Inst.Inven.usePanel[3].GetComponent<ItemAmountReturn>().GetItemInfo(this, item.curNumber);
                }
                else
                {
                    SceneData.Inst.Inven.usePanel[2].SetActive(true);
                    SceneData.Inst.Inven.usePanel[2].GetComponent<DecisionReturn>().ReturnDecision(this);
                }
            }
            else if (item.myItem.orgData.itemType == ItemData.ItemType.Interactable)
            {
                SceneData.Inst.Inven.usePanel[2].SetActive(true);
                SceneData.Inst.Inven.usePanel[2].GetComponent<DecisionReturn>().ReturnDecision(this);
            }

            // ���͸��� Ÿ���� ���� ���ϵ���
        }
    }
}
