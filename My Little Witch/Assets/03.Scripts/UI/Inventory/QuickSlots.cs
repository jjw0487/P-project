using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlots : Slots
{
    
    public override void AddItem(Item _item, int _count)
    {
        if (orgSprite == null) { orgSprite = GetComponentInChildren<Image>().sprite; }
        // ���� �̹����� �ּ� ������ clear �� �� �ٿ�����
        item = _item;
        if (_item.myItem.orgData.name == item.myItem.orgData.name)
        {
            AddCount(_count);
        }
        img.sprite = _item.myItem.orgData.sprite;
    }


    public void UseQuickSlotItem()
    {
        
        if (item != null)
        {
            print("���");
            if (item.curNumber > 0)
            {
                --item.curNumber;
                count.text = item.curNumber.ToString();
                SceneData.Inst.myPlayer.GetItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value);
            }

            if (item.curNumber == 0)
            {
                if (item.gameObject != null) Destroy(item.gameObject);
                ClearSlot();
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                UseQuickSlotItem();
            }
        }
    }


    public override void OnDrop(PointerEventData eventData)
    { // �ٸ� ���� ��ġ�� ������ ��
        SceneData.Inst.Inven.quickSlotManager.StartQuickSlot(); //������ ��ŸƮ �ڷ�ƾ ����

        if (DragImage.Inst.dragSlot.item.myItem.orgData.itemType == ItemData.ItemType.Consumable) 
        {
            if (DragImage.Inst.dragSlot != null) { ChangeSlot(); }
        }
        else return; //�Ҹ�ǰ �̿ܿ��� ����
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Inst.SetColor(0);
        DragImage.Inst.dragSlot = null;
    }

    public override void OnPointerEnter(PointerEventData eventData) // ���콺�� ������ ���� ���� ��
    {
        SceneData.Inst.myPlayer.OnUI = true;
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnUI = false;
    }




}
