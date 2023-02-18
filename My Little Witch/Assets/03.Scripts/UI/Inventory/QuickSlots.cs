using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlots : Slots
{
    public override void AddItem(Item _item, int _count)
    {
        if (orgSprite == null) { orgSprite = GetComponentInChildren<Image>().sprite; }
        // 기존 이미지를 둬서 슬롯이 clear 될 때 붙여주자
        item = _item;
        if (_item.myItem.orgData.name == item.myItem.orgData.name)
        {
            AddCount(_count);
        }
        img.sprite = _item.myItem.orgData.sprite;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                if (itemCount > 1)
                {
                    //SceneData.Inst.Inven.usePanel[1].SetActive(true);
                    //SceneData.Inst.Inven.usePanel[1].GetComponent<ItemAmountReturn>().GetItemInfo(this, itemCount);
                }
                else
                {
                    //SceneData.Inst.Inven.usePanel[0].SetActive(true);
                    //SceneData.Inst.Inven.usePanel[0].GetComponent<DecisionReturn>().ReturnDecision(this);
                }
            }

        }
    }


    public override void OnDrop(PointerEventData eventData)
    { // 다른 슬롯 위치에 놓였을 때

        if (DragImage.Inst.dragSlot.item.myItem.orgData.itemType == ItemData.ItemType.Consumable)
        {
            if (DragImage.Inst.dragSlot != null) { ChangeSlot(); }
        }
        else return;
    }

    public override void OnPointerEnter(PointerEventData eventData) // 마우스가 아이템 위에 있을 때
    {
        SceneData.Inst.myPlayer.OnUI = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnUI = false;
    }

}
