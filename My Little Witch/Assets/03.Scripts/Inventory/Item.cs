using System;
using UnityEngine;

[Serializable]
public struct ItemInformation
{
    public ItemData orgData;
    public int curNumber;
}

public class Item : MonoBehaviour
{

    public ItemInformation myItem;
    public GameObject[] contents;
    private void Start()
    {
        if (contents == null) { contents = null; }
        myItem.curNumber = myItem.orgData.count;
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

    public void GetItem()
    {
        //아이템 위치를 오브젝트 풀에 담을 수 있도록 연구 해 보자.
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // 슬롯 수량만큼 반복
        {
            if (SceneData.Inst.Inven.slots[i].childCount == 1) // 자식이 하나만 있다면
            {
                this.transform.SetParent(SceneData.Inst.Inven.slots[i]); // 부모로 빈 슬롯
                this.transform.position = new Vector3(999f, 999f, 999f); // 게임 화면에서 보이지 않도록 임의의 포지션에 위치시켜볼까?
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddItem(this.GetComponent<Item>(), myItem.curNumber);
                // slot 상에 이미지를 변경
                break; // 조건검사에 걸린다면 반복문 탈출
            }
            else if (SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.orgData.name == myItem.orgData.name)
            {
                //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                this.transform.position = new Vector3(999f, 999f, 999f);
                SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.curNumber += myItem.curNumber; // 수량을 증가
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddCount(this.myItem.curNumber); // 증가 한 후에 다시 화면에 표현
                break;
            }
        }
    }


    public void GetItemFromInteractableItems(GameObject what)
    {


        //아이템 위치를 오브젝트 풀에 담을 수 있도록 연구 해 보자.
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // 슬롯 수량만큼 반복
        {
            if (SceneData.Inst.Inven.slots[i].childCount == 1) // 자식이 하나만 있다면
            {

                GameObject obj = Instantiate(what, SceneData.Inst.Inven.slots[i]);
                //what.transform.SetParent(SceneData.Inst.Inven.slots[i]); // 부모로 빈 슬롯
                obj.transform.position = new Vector3(999f, 999f, 999f); // 게임 화면에서 보이지 않도록 임의의 포지션에 위치시켜볼까?
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddItem(obj.GetComponent<Item>(),
                    obj.GetComponent<Item>().myItem.orgData.count);
                // slot 상에 이미지를 변경

                break; // 조건검사에 걸린다면 반복문 탈출
            }
            else if (SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.orgData.name == what.GetComponent<Item>().myItem.orgData.name)
            {
                GameObject obj = Instantiate(what, SceneData.Inst.Inven.slots[i]);
                //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                obj.transform.position = new Vector3(999f, 999f, 999f);
                SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.curNumber += obj.GetComponent<Item>().myItem.curNumber; // 수량을 증가
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddCount(obj.GetComponent<Item>().myItem.curNumber); // 증가 한 후에 다시 화면에 표현
                break;
            }
        }
    }

}
