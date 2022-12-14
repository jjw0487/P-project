//작성자 : 전정우
//설명 :
/*using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;





public class EnhanceableItems : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler
{
    //ublic SaveItemData myData;
    public GameObject[] myIng;
    GameObject obj;
    GameObject obj2;



    
    //public SaveItemData myData;

    public int _AP
    {
        get => myData.AP;
    }
    public int _DP
    {
        get => myData.DP;
    }
    public int _EnchantCost
    {
        get => myData.EnchantCost;
    }
    public float _Possibility
    {
        get => myData.Possibility;
    }
    public bool _GetMaxLevel
    {
        get => myData.GetMaxLevel;
    }
    public bool _EnhanceableItem
    {
        get => myData.GetEnhanceableItem;
    }

    public void ConsumeGold()
    {
        DataController.instance.gameData.gold -= myData.EnchantCost;
    }

    public bool CheckSuccess()
    {
        float rnd = Random.Range(0, 100);
        if (rnd <= _Possibility)
        {
            Debug.Log($"랜덤숫자{rnd}");
            return true;
        }
        Debug.Log($"랜덤숫자{rnd}");

        return false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        *//*GameObject myEnchant = GameObject.FindWithTag("Enhance");
        myEnchant.gameObject.transform.SetAsLastSibling();*//*
        if (EnhancementController.inst.isOpen)
        {
            Vector3 pos = new Vector3(58, 168, 0);
            obj = Instantiate(myIng[0], pos, Quaternion.identity, GameObject.FindWithTag("Enhance").transform);
            obj.transform.localPosition = new Vector3(-126, -27, 0);
            obj2 = Instantiate(myIng[1], pos, Quaternion.identity, GameObject.FindWithTag("Enhance").transform);
            obj2.transform.localPosition = new Vector3(26, -27, 0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(obj);
        Destroy(obj2);
    }
}*/