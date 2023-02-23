public class EquipmentSlots : Slots
{
    protected override void ChangeSlot()
    {
        Item temp = item; // 아이템을 담을 공간을 만들고
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
        SceneData.Inst.myPlayer.GetEquipedItemValue(item.myItem.orgData.valueType, item.myItem.orgData.value, 0f); // 1. SP 2. DP 3. HP, 4. MP

    }

    public override void ClearSlot()
    {
        item = null;
        itemCount = 0;
        img.sprite = orgSprite;
    }
}
