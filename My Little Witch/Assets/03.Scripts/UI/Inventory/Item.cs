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

    private bool moveOn = false;
    private void Awake()
    {
        if (contents == null) { contents = null; }
        myItem.curNumber = myItem.orgData.count;
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

    public void GetItem()
    {
        //������ ��ġ�� ������Ʈ Ǯ�� ���� �� �ֵ��� ���� �� ����.
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item == null) // slot�� item ������ null�̶��
            {
                for(int n = 0; n < SceneData.Inst.Inven.slots.Length; ++n) // item ��ġ�� ���Ƿ� �Ű������� �𸣴� �� �� �� �˻�
                {
                    if(SceneData.Inst.Inven.slots[n].GetComponent<Slots>().item != null) // item ������ null�� �ƴ϶��
                    {
                        if (SceneData.Inst.Inven.slots[n].GetComponent<Slots>().item.myItem.orgData.name == myItem.orgData.name)
                            //�̸��� ������ ���ǰ˻�
                        {
                            moveOn = true; 
                            this.transform.position = new Vector3(999f, 999f, 999f);
                            //SceneData.Inst.Inven.slots[n].GetComponentInChildren<Item>().myItem.curNumber += myItem.curNumber;
                            SceneData.Inst.Inven.slots[n].GetComponent<Slots>().AddCount(this.myItem.curNumber);
                            break;
                        }
                    }
                }
                if(!moveOn) // ���� ���ǿ� �Ȱɷ��� �� ����
                {
                    this.transform.SetParent(SceneData.Inst.Inven.slots[i]); // �θ�� �� ����
                    this.transform.position = new Vector3(999f, 999f, 999f); // ���� ȭ�鿡�� ������ �ʵ��� ������ �����ǿ� ��ġ���Ѻ���?
                    SceneData.Inst.Inven.slots[i].GetComponent<Slots>().AddItem(this.GetComponent<Item>(), myItem.curNumber);
                    // slot �� �̹����� ����
                }

                moveOn = false;
                break; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
            }
            else if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.myItem.orgData.name == myItem.orgData.name)
            {
                //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                this.transform.position = new Vector3(999f, 999f, 999f);
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
                obj.transform.position = new Vector3(999f, 999f, 999f);
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
