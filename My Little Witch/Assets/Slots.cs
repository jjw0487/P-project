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
        img.sprite = GetComponentInChildren<Item>().myItem.orgData.sprite; // 사진파일을 표현
        count.text = GetComponentInChildren<Item>().myItem.curNumber.ToString(); // 수량 인트를 스트링으로
        itemCount = GetComponentInChildren<Item>().myItem.curNumber; // 스크립트 안에서 사용 할 수량 확인
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
        // 드래그에 접근한 아이템이 한 개 이상일 때
        if(itemCount > 1)
        {
            // 드래그 할 아이템이 한 개 이상일 때 몇개를 이동 및 소모 할 것인지 물어보자.
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
    { // 다른 슬롯 위치에 놓였을 때

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
