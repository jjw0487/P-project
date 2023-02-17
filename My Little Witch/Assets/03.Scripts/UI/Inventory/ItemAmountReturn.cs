using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemAmountReturn : MonoBehaviour
{

    public TMPro.TMP_Text numCount;
    private Slots _slot = null;
    private int _itemCount;
    private int maxCount;

    private ItemData _itemTemp;

    // 상점
    public void GetStoreItemInfo(ItemData item) // StoreTab 에서 데이터를 받고
    {
        _itemTemp = item;
        _itemCount = 0;
        maxCount = SceneData.Inst.Inven.gold / item.currencyInStore;
        numCount.text = _itemCount.ToString();
    }

    public void IncreasePurchaseCount()
    {
        if (_itemCount <= maxCount && maxCount != 0) { ++_itemCount; } 
        numCount.text = _itemCount.ToString();
    }

    public void ReducePurchaseCount()
    {
        if (_itemCount > 0) { --_itemCount; }
        numCount.text = _itemCount.ToString();
    }

    public void ReturnPurchaseValue() // yes 버튼
    {
        if(_itemCount > 0 && _itemCount != 0)
        {
            GameObject obj = Instantiate(_itemTemp.obj, new Vector3(999f, 999f, 999f), Quaternion.identity);
            obj.transform.SetParent(SceneData.Inst.ItemPool);
            obj.GetComponent<Item>().GetItem(_itemCount);
            SceneData.Inst.Inven.PurchaseItem(_itemCount * _itemTemp.currencyInStore);
        }

        _itemTemp = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
        
    }

    public void PurchaseCancel() // no 버튼
    {
        _itemTemp = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }


    // 인벤토리 아이템 사용
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

    // 버리기
    public void ReturnThrownItemValue() //버튼
    {
        _slot.ThrowItemsAway(_itemCount);
        _slot = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }
}
