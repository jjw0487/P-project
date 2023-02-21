using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlots : Slots
{
    protected override void ChangeSlot()
    {
        Item temp = item; // �������� ���� ������ �����
        int tempItemCount = itemCount;
        //AddItem(DragImage.Inst.dragSlot.item, DragImage.Inst.dragSlot.itemCount);

        if (temp != null)
        {
            DragImage.Inst.dragSlot.AddItem(temp, tempItemCount);
        }
        else
        {
            DragImage.Inst.dragSlot.ClearSlot();
        }
    }

    public void ChangeSlotByCliicking(Item _item)
    {
        item = _item;
        img.sprite = item.myItem.orgData.sprite;
    }
}
