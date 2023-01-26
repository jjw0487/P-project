using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemInformation
{
    public ItemData orgData;
    public int curNumber;
}

public class Item : MonoBehaviour
{

    public ItemInformation myItem;
    private void Start()
    {
        myItem.curNumber = myItem.orgData.number;
    }

    private void OnMouseEnter()
    {
        if(this.GetComponent<Renderer>())
        {
            this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 1.0f);
        }
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetItem(this.gameObject);
        }
    }

    private void OnMouseExit()
    {
        if (this.GetComponent<Renderer>())
        {
            this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 0.0f);
        }
    }

    public void GetItem(GameObject what)
    {
        //아이템 위치를 오브젝트 풀에 담을 수 있도록 연구 해 보자.

        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // 슬롯 수량만큼 반복
        {
            if (SceneData.Inst.Inven.slots[i].childCount == 1) // 자식이 하나만 있다면
            {
                what.transform.SetParent(SceneData.Inst.Inven.slots[i]); // 부모로 빈 슬롯
                what.transform.position = new Vector3(999f, 999f, 999f); // 게임 화면에서 보이지 않도록 임의의 포지션에 위치시켜볼까?
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddItem(this.GetComponent<Item>(), myItem.curNumber); 
                // slot 상에 이미지를 변경
                break; // 조건검사에 걸린다면 반복문 탈출
            }
            else if (SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.orgData.name == myItem.orgData.name)
            {
                //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                what.transform.position = new Vector3(999f, 999f, 999f);
                SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.curNumber += myItem.curNumber; // 수량을 증가
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddCount(); // 증가 한 후에 다시 화면에 표현
                break;
            }
        }
    }


}
