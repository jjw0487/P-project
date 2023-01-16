using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Slots : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    Image img;
    public TMPro.TMP_Text count;
    private int itemCount = 0;


    void Start()
    {
        img = this.GetComponent<Image>();

        if (GetComponent<Item>() != null)
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
        item = _item;
        count.text = _item.myItem.curNumber.ToString();
        itemCount = _count;
        img.sprite = _item.myItem.orgData.sprite;
    }

    public void AddCount()
    {
        count.text = item.myItem.curNumber.ToString();
    }

    void logic()
    {
        // �巡�׿� ������ �������� �� �� �̻��� ��
        if(itemCount > 1)
        {
            // �巡�� �� �������� �� �� �̻��� �� ��� �̵� �� �Ҹ� �� ������ �����.
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null) 
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
            //this.transform.position = eventData.position;
            DragImage.Inst.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Inst.SetColor(0);
        DragImage.Inst.dragSlot = null;
        //this.transform.localPosition = Vector3.zero; 
    }

    public void OnDrop(PointerEventData eventData)
    { // �ٸ� ���� ��ġ�� ������ ��

        //this.transform.SetParent(eventData.pointerDrag.transform);
        //this.transform.localPosition = Vector3.zero;
        if(DragImage.Inst.dragSlot != null) { ChangeSlot();}
    }


    private void ChangeSlot()
    {
        Item temp = item;
        int tempItemCount = itemCount;
        AddItem(DragImage.Inst.dragSlot.item, DragImage.Inst.dragSlot.itemCount);

        if(temp != null)
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
        img.sprite = null;
        count.text = "";
    }

}
