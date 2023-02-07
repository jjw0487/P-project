using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmountReturn : MonoBehaviour
{
    public TMPro.TMP_Text numCount;
    private Slots _slot = null;
    private int _itemCount;
    private int maxCount;


    public void GetItemInfo(Slots slot, int count) // slots 에서 데이터를 받고
    {
        _slot = slot;
        _itemCount = maxCount = count; // 맥스카운트와 아이템카운트를 초기화
        numCount.text = count.ToString(); 
    }

    public void IncreaseCount()
    {
        if(_itemCount != maxCount) { ++_itemCount; }
        numCount.text = _itemCount.ToString();
    }

    public void ReduceCount()
    {
        if(_itemCount != 0) { --_itemCount; }
        numCount.text = _itemCount.ToString();
    }

    public void ReturnValue() //버튼
    {
        _slot.UseItems(_itemCount);
        _slot = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }

    public void Cancel() //버튼
    {
        _slot = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }
}
