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
        //������ ��ġ�� ������Ʈ Ǯ�� ���� �� �ֵ��� ���� �� ����.

        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (SceneData.Inst.Inven.slots[i].childCount == 1) // �ڽ��� �ϳ��� �ִٸ�
            {
                what.transform.SetParent(SceneData.Inst.Inven.slots[i]); // �θ�� �� ����
                what.transform.position = new Vector3(999f, 999f, 999f); // ���� ȭ�鿡�� ������ �ʵ��� ������ �����ǿ� ��ġ���Ѻ���?
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddItem(this.GetComponent<Item>(), myItem.curNumber); 
                // slot �� �̹����� ����
                break; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
            }
            else if (SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.orgData.name == myItem.orgData.name)
            {
                //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                what.transform.position = new Vector3(999f, 999f, 999f);
                SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.curNumber += myItem.curNumber; // ������ ����
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddCount(); // ���� �� �Ŀ� �ٽ� ȭ�鿡 ǥ��
                break;
            }
        }
    }


}
