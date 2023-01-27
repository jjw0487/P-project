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
        //������ ��ġ�� ������Ʈ Ǯ�� ���� �� �ֵ��� ���� �� ����.
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (SceneData.Inst.Inven.slots[i].childCount == 1) // �ڽ��� �ϳ��� �ִٸ�
            {
                this.transform.SetParent(SceneData.Inst.Inven.slots[i]); // �θ�� �� ����
                this.transform.position = new Vector3(999f, 999f, 999f); // ���� ȭ�鿡�� ������ �ʵ��� ������ �����ǿ� ��ġ���Ѻ���?
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddItem(this.GetComponent<Item>(), myItem.curNumber);
                // slot �� �̹����� ����
                break; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
            }
            else if (SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.orgData.name == myItem.orgData.name)
            {
                //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                this.transform.position = new Vector3(999f, 999f, 999f);
                SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.curNumber += myItem.curNumber; // ������ ����
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddCount(this.myItem.curNumber); // ���� �� �Ŀ� �ٽ� ȭ�鿡 ǥ��
                break;
            }
        }
    }


    public void GetItemFromInteractableItems(GameObject what)
    {


        //������ ��ġ�� ������Ʈ Ǯ�� ���� �� �ֵ��� ���� �� ����.
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (SceneData.Inst.Inven.slots[i].childCount == 1) // �ڽ��� �ϳ��� �ִٸ�
            {

                GameObject obj = Instantiate(what, SceneData.Inst.Inven.slots[i]);
                //what.transform.SetParent(SceneData.Inst.Inven.slots[i]); // �θ�� �� ����
                obj.transform.position = new Vector3(999f, 999f, 999f); // ���� ȭ�鿡�� ������ �ʵ��� ������ �����ǿ� ��ġ���Ѻ���?
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddItem(obj.GetComponent<Item>(),
                    obj.GetComponent<Item>().myItem.orgData.count);
                // slot �� �̹����� ����

                break; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
            }
            else if (SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.orgData.name == what.GetComponent<Item>().myItem.orgData.name)
            {
                GameObject obj = Instantiate(what, SceneData.Inst.Inven.slots[i]);
                //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                obj.transform.position = new Vector3(999f, 999f, 999f);
                SceneData.Inst.Inven.slots[i].GetComponentInChildren<Item>().myItem.curNumber += obj.GetComponent<Item>().myItem.curNumber; // ������ ����
                SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddCount(obj.GetComponent<Item>().myItem.curNumber); // ���� �� �Ŀ� �ٽ� ȭ�鿡 ǥ��
                break;
            }
        }
    }

}
