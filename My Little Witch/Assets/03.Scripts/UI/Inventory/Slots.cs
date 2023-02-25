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
        if (GetComponent<Item>() != null) // 아이템을 놓고 실험할 경우
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
        // 기존 이미지를 둬서 슬롯이 clear 될 때 붙여주자
        item = _item;
        item.curNumber = _item.curNumber;
        count.text = item.curNumber.ToString();
        img.sprite = _item.myItem.orgData.sprite;
    }

    protected virtual void AddItemByDragging(Item _item)
    {
        if (orgSprite == null) { orgSprite = GetComponent<Image>().sprite; }
        // 기존 이미지를 둬서 슬롯이 clear 될 때 붙여주자
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
                        //아이템이 null이 아니거나 이미 존재하는 아이템인 경우
                        {
                            return;
                        }
                    }

                    for (int i = 0; i < SceneData.Inst.Inven.equipSlots.Length; ++i) // 슬롯 갯수만큼 반복
                    {
                        if (SceneData.Inst.Inven.equipSlots[i].item == null)
                        {

                            SceneData.Inst.Inven.equipSlots[i].ChangeSlotByCliicking(this.item);
                            this.ClearSlot();
                            break; // 조건검사에 걸린다면 반복문 탈출
                        }
                    }
                }

                // 매터리얼 타입은 반응 안하도록
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
        {//보물상자는 한 개 씩 열도록 구분(single, plural)을 두지 않을거라 이렇게 설정

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
    { // 다른 슬롯 위치에 놓였을 때

        if (DragImage.Inst.dragSlot != null) { ChangeSlot(); }
    }

    protected virtual void ChangeSlot()
    {
        if (item != null) 
        {
            Item temp = item; // 아이템을 담을 공간을 만들고
            AddItemByDragging(DragImage.Inst.dragSlot.item);
            DragImage.Inst.dragSlot.AddItemByDragging(temp);
        }
        else
        {

            AddItemByDragging(DragImage.Inst.dragSlot.item);
            DragImage.Inst.dragSlot.ClearSlot();
        }
/*
        Item temp = item; // 아이템을 담을 공간을 만들고
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

    public virtual void OnPointerEnter(PointerEventData eventData) // 마우스가 아이템 위에 있을 때
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

        if (SceneData.Inst.ItemPool.GetComponentInChildren<Item>()) // 아이템풀에 아이템이 하나도 없을 때
        {
            // 아이템을 찾아보고 이름이 같은 아이템이 있다면 SceneData.Inst.myPlayer.transform.position;
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
        if (SceneData.Inst.ItemPool.GetComponentInChildren<Item>()) // 아이템풀에 아이템이 하나도 없을 때
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

            // 매터리얼 타입은 반응 안하도록
        }
    }
}
