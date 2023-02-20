using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Slots : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    [SerializeField] protected Image img;
    //private Image previousImg;
    public TMPro.TMP_Text count;
    protected int itemCount = 0;
    protected Sprite orgSprite;

    private bool isDone = false;

    void Start()
    {
        if (GetComponent<Item>() != null) // 아이템을 놓고 실험할 경우
        {
            item = GetComponentInChildren<Item>();
            itemCount = GetComponentInChildren<Item>().curNumber;
        }
    }
    /*public void ShowingSprite()
    {
        img.sprite = GetComponentInChildren<Item>().myItem.orgData.sprite; // 사진파일을 표현
        count.text = GetComponentInChildren<Item>().myItem.curNumber.ToString(); // 수량 인트를 스트링으로
        itemCount = GetComponentInChildren<Item>().myItem.curNumber; // 스크립트 안에서 사용 할 수량 확인
        item = GetComponentInChildren<Item>();
    }*/
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
        if (_item.myItem.orgData.name == item.myItem.orgData.name)
        {
            AddCount(_count);
        }
        img.sprite = _item.myItem.orgData.sprite;
    }

    public void AddCount(int howmany)
    {
        itemCount += howmany;
        item.curNumber = itemCount;
        if(count != null) count.text = item.curNumber.ToString();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
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
                
                // 매터리얼 타입은 반응 안하도록
            }

        }
    }

   
    public void UseItems(int howmany)
    {
        SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value*howmany);
        itemCount -= howmany;
        if (itemCount == 0) { if (item.gameObject != null) { Destroy(item.gameObject); } ClearSlot(); }
        else
        {
            item.curNumber = itemCount;
            count.text = item.curNumber.ToString();
        }
    }

    public void UseSingleItem()
    {
        print("사용");
        if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
        {
            SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value);
            if(item.gameObject != null) Destroy(item.gameObject);
            ClearSlot();
        }
        else if (item.myItem.orgData.itemType == ItemData.ItemType.Interactable)
        {//보물상자는 한 개 씩 열도록 구분(single, plural)을 두지 않을거라 이렇게 설정

            for (int i = 0; i < item.contents.Length; i++)
            {
                item.GetItemFromInteractableItems(item.contents[i]);
            }
            itemCount -= 1;
            if (itemCount == 0) { Destroy(item.gameObject); ClearSlot(); }
            else
            { 
                item.curNumber = itemCount;
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
        if(SceneData.Inst.myPlayer.OnUI == false)
        {
            ThrowItemAway();
        }

    }

    public void ThrowAwaySingleItem()
    {

        if(SceneData.Inst.ItemPool.GetComponentInChildren<Item>()) // 아이템풀에 아이템이 하나도 없을 때
        {
            // 아이템을 찾아보고 이름이 같은 아이템이 있다면 SceneData.Inst.myPlayer.transform.position;
            for(int i = 0; i< SceneData.Inst.ItemPool.childCount; i++)
            {
                if(SceneData.Inst.ItemPool.GetChild(i).GetComponent<Item>().myItem.orgData.itemName 
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
                if (SceneData.Inst.ItemPool.GetChild(i).GetComponent<Item>().myItem.orgData.itemName
                    == item.myItem.orgData.itemName)
                {
                    SceneData.Inst.ItemPool.GetChild(i).transform.position = SceneData.Inst.myPlayer.transform.position + new Vector3(0f, 1f, 0.5f);
                    SceneData.Inst.ItemPool.GetChild(i).GetComponent<Item>().curNumber = howmany;
                    isDone = true;
                    break;
                }
            }
            if(!isDone)
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

        itemCount -= howmany;

        if (itemCount == 0) { ClearSlot(); }
        else
        {
            item.curNumber = itemCount;
            count.text = item.curNumber.ToString();
        }
    }

    private void ThrowItemAway()
    {
        if (item != null)
        {
            if (item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
            {
                if (itemCount > 1)
                {
                    SceneData.Inst.Inven.usePanel[3].SetActive(true);
                    SceneData.Inst.Inven.usePanel[3].GetComponent<ItemAmountReturn>().GetItemInfo(this, itemCount);
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
    public virtual void OnDrop(PointerEventData eventData)
    { // 다른 슬롯 위치에 놓였을 때

        if (DragImage.Inst.dragSlot != null) { ChangeSlot(); }
    }

    protected void ChangeSlot()
    {
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
        }
    }

    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
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
}
