using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemAmountReturn : MonoBehaviour
{
    public TMPro.TMP_Text itemName; // ������ ������ �̸��� ���
    public TMPro.TMP_Text numCount;
    private Slots _slot = null;
    private int _itemCount;
    private int maxCount;

    private ItemData _itemTemp;

    // ����
    public void GetStoreItemInfo(ItemData item) // StoreTab ���� �����͸� �ް�
    {
        _itemTemp = item;
        itemName.text = item.itemNameInStore;
        _itemCount = 0;
        maxCount = SceneData.Inst.interactableUIManager.gold / item.currencyInStore;
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

    public void ReturnPurchaseValue() // yes ��ư
    {
        if(_itemCount > 0 && _itemCount != 0)
        {
            GameObject obj = Instantiate(_itemTemp.obj, new Vector3(999f, 999f, 999f), Quaternion.identity);
            obj.transform.SetParent(SceneData.Inst.ItemPool);
            obj.GetComponent<Item>().GetItem(_itemCount);
            SceneData.Inst.interactableUIManager.PurchaseItem(_itemCount * _itemTemp.currencyInStore);
        }

        _itemTemp = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
        
    }

    public void PurchaseCancel() // no ��ư
    {
        _itemTemp = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }


    // �κ��丮 ������ ���
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

    // ������
    public void ReturnThrownItemValue() //��ư
    {
        _slot.ThrowItemsAway(_itemCount);
        _slot = null;
        _itemCount = 0;
        numCount.text = "";
        this.gameObject.SetActive(false);
    }
}
