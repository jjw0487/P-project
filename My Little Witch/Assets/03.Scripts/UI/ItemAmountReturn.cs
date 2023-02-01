using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmountReturn : MonoBehaviour
{
    public TMPro.TMP_Text numCount;
    private Slots _slot = null;
    private int _itemCount;
    private int maxCount;


    public void GetItemInfo(Slots slot, int count) // slots ���� �����͸� �ް�
    {
        _slot = slot;
        _itemCount = maxCount = count; // �ƽ�ī��Ʈ�� ������ī��Ʈ�� �ʱ�ȭ
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

    public void ReturnValue() //��ư
    {
        _slot.UseItems(_itemCount);
        _slot = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }

    public void Cancel() //��ư
    {
        _slot = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }
}
