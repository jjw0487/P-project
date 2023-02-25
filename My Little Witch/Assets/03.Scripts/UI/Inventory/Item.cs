using System;
using UnityEngine;

[Serializable]
public struct ItemInformation
{
    public ItemData orgData;
}

public class Item : MonoBehaviour
{

    public ItemInformation myItem;
    public GameObject[] contents;
    public int curNumber;
    private Slots[] invenSlots;

    private void Awake()
    {
        if (contents == null) { contents = null; }
        curNumber = myItem.orgData.count;
        invenSlots = SceneData.Inst.Inven.slotData;
    }

    private void Start()
    {
    }

    private void OnMouseEnter()
    {
        if (this.GetComponent<Renderer>())
        {
            this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 1.0f);
        }

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetItem();
        }
    }

    private void OnMouseExit()
    {
        if (this.GetComponent<Renderer>())
        {
            this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 0.0f);
        }
    }

    /* public void GetItem(int count = 0)
     {
         //아이템 위치를 오브젝트 풀에 담을 수 있도록 연구 해 보자.
         for (int i = 0; i < invenSlots.Length; ++i) // 슬롯 수량만큼 반복
         {
             if (invenSlots[i].item == null) // slot에 item 정보가 null이라면
             {
                 for (int n = 0; n < invenSlots.Length; ++n) // item 위치를 임의로 옮겼을지도 모르니 한 번 더 검사
                 {
                     if (invenSlots[n].item != null) // item 정보가 null이 아니라면
                     {
                         if (invenSlots[n].item.myItem.orgData.itemId == myItem.orgData.itemId)
                         //이름이 같은지 조건검사
                         {
                             moveOn = true;
                             this.transform.position = new Vector3(999f, 999f, 999f);
                             if (count != 0) { invenSlots[n].AddCount(count); }
                             else { invenSlots[n].AddCount(curNumber); }
                             invenSlots[n].FloatNotice(this.myItem.orgData.itemName); // 이름플러팅
                             SceneData.Inst.questItemCheckEvent?.Invoke();
                             break;
                         }
                     }
                 }
                 if (!moveOn) // 위에 조건에 안걸렸을 때 실행
                 {
                     this.transform.SetParent(SceneData.Inst.Inven.slots[i]); // 부모로 빈 슬롯
                     this.transform.position = new Vector3(999f, 999f, 999f); // 게임 화면에서 보이지 않도록 임의의 포지션에 위치시켜볼까?
                     this.transform.SetParent(SceneData.Inst.ItemPool);

                     if (count != 0) { invenSlots[i].AddItem(this.GetComponent<Item>(), count); }
                     else { invenSlots[i].AddItem(this.GetComponent<Item>(), curNumber); }
                     invenSlots[i].FloatNotice(this.myItem.orgData.itemName); // 이름플러팅
                     SceneData.Inst.questItemCheckEvent?.Invoke();
                 }

                 moveOn = false;
                 break; // 조건검사에 걸린다면 반복문 탈출
             }
             else if (invenSlots[i].item.myItem.orgData.name == myItem.orgData.name)
             {
                 //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                 this.transform.position = new Vector3(999f, 999f, 999f);
                 if (count != 0) { invenSlots[i].AddCount(count); }
                 else { invenSlots[i].AddCount(this.curNumber); }// 증가 한 후에 다시 화면에 표현
                 invenSlots[i].FloatNotice(this.myItem.orgData.itemName); // 이름플러팅
                 Destroy(this.gameObject, 1f);
                 break;
             }
         }
     }*/

    public void GetItem(int count = 0)
    {

        for (int n = 0; n < invenSlots.Length; ++n) // item 위치를 임의로 옮겼을지도 모르니 한 번 더 검사
        {
            if (invenSlots[n].item != null) // item 정보가 null이 아니라면
            {
                if (invenSlots[n].item.myItem.orgData.itemId == myItem.orgData.itemId)
                //이름이 같은지 조건검사
                {
                    print("중복");
                    this.transform.position = new Vector3(999f, 999f, 999f);
                    if (count != 0) { invenSlots[n].AddCount(count); } // 매개변수로 받은 값이 있다면
                    else { invenSlots[n].AddCount(curNumber); } //인자값이 없으면 현재 아이템 정보의 수량
                    invenSlots[n].FloatNotice(this.myItem.orgData.itemName); // 이름플러팅
                    SceneData.Inst.questItemCheckEvent?.Invoke();
                    return;
                }
            }
        }

        //아이템 위치를 오브젝트 풀에 담을 수 있도록 연구 해 보자.
        for (int i = 0; i < invenSlots.Length; ++i) // 슬롯 수량만큼 반복
        {

            if (invenSlots[i].item == null) // slot에 item 정보가 null이라면
            {
                this.transform.SetParent(SceneData.Inst.Inven.slots[i]); // 부모로 빈 슬롯
                this.transform.position = new Vector3(999f, 999f, 999f); // 게임 화면에서 보이지 않도록 임의의 포지션에 위치시켜볼까?
                this.transform.SetParent(SceneData.Inst.ItemPool);
                if (count != 0) { invenSlots[i].AddItem(this.GetComponent<Item>(), count); }
                else { invenSlots[i].AddItem(this.GetComponent<Item>(), curNumber); print("한개"); }
                invenSlots[i].FloatNotice(this.myItem.orgData.itemName); // 이름플러팅
                SceneData.Inst.questItemCheckEvent?.Invoke();
                return; // 조건검사에 걸린다면 반복문 탈출
            }

        }
    }
    public void GetItemFromInteractableItems(GameObject what)
    {

        GameObject obj = Instantiate(what, new Vector3(999f, 999f, 999f), Quaternion.identity);
        obj.transform.SetParent(SceneData.Inst.ItemPool);

        for (int i = 0; i < invenSlots.Length; ++i) // 슬롯 수량만큼 반복
        {
            if (invenSlots[i].item != null && invenSlots[i].item.myItem.orgData.itemId == obj.GetComponent<Item>().myItem.orgData.itemId)
            {
                invenSlots[i].item.curNumber += obj.GetComponent<Item>().curNumber; // 수량을 증가
                invenSlots[i].count.text = invenSlots[i].item.curNumber.ToString(); // 증가 한 후에 다시 화면에 표현
                Destroy(obj.gameObject);
                return;
            }
        }

        //아이템 위치를 오브젝트 풀에 담을 수 있도록 연구 해 보자.
        for (int i = 0; i < invenSlots.Length; ++i) // 슬롯 수량만큼 반복
        {
            if (invenSlots[i].item == null) // 자식이 하나만 있다면
            {
                invenSlots[i].AddItem(obj.GetComponent<Item>(),
                    obj.GetComponent<Item>().myItem.orgData.count);
                return; // 조건검사에 걸린다면 반복문 탈출
            }
        }
    }

}
