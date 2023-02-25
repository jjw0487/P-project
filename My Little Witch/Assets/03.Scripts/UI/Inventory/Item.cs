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
         //������ ��ġ�� ������Ʈ Ǯ�� ���� �� �ֵ��� ���� �� ����.
         for (int i = 0; i < invenSlots.Length; ++i) // ���� ������ŭ �ݺ�
         {
             if (invenSlots[i].item == null) // slot�� item ������ null�̶��
             {
                 for (int n = 0; n < invenSlots.Length; ++n) // item ��ġ�� ���Ƿ� �Ű������� �𸣴� �� �� �� �˻�
                 {
                     if (invenSlots[n].item != null) // item ������ null�� �ƴ϶��
                     {
                         if (invenSlots[n].item.myItem.orgData.itemId == myItem.orgData.itemId)
                         //�̸��� ������ ���ǰ˻�
                         {
                             moveOn = true;
                             this.transform.position = new Vector3(999f, 999f, 999f);
                             if (count != 0) { invenSlots[n].AddCount(count); }
                             else { invenSlots[n].AddCount(curNumber); }
                             invenSlots[n].FloatNotice(this.myItem.orgData.itemName); // �̸��÷���
                             SceneData.Inst.questItemCheckEvent?.Invoke();
                             break;
                         }
                     }
                 }
                 if (!moveOn) // ���� ���ǿ� �Ȱɷ��� �� ����
                 {
                     this.transform.SetParent(SceneData.Inst.Inven.slots[i]); // �θ�� �� ����
                     this.transform.position = new Vector3(999f, 999f, 999f); // ���� ȭ�鿡�� ������ �ʵ��� ������ �����ǿ� ��ġ���Ѻ���?
                     this.transform.SetParent(SceneData.Inst.ItemPool);

                     if (count != 0) { invenSlots[i].AddItem(this.GetComponent<Item>(), count); }
                     else { invenSlots[i].AddItem(this.GetComponent<Item>(), curNumber); }
                     invenSlots[i].FloatNotice(this.myItem.orgData.itemName); // �̸��÷���
                     SceneData.Inst.questItemCheckEvent?.Invoke();
                 }

                 moveOn = false;
                 break; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
             }
             else if (invenSlots[i].item.myItem.orgData.name == myItem.orgData.name)
             {
                 //this.transform.SetParent(SceneData.Inst.Inven.slots[i]);
                 this.transform.position = new Vector3(999f, 999f, 999f);
                 if (count != 0) { invenSlots[i].AddCount(count); }
                 else { invenSlots[i].AddCount(this.curNumber); }// ���� �� �Ŀ� �ٽ� ȭ�鿡 ǥ��
                 invenSlots[i].FloatNotice(this.myItem.orgData.itemName); // �̸��÷���
                 Destroy(this.gameObject, 1f);
                 break;
             }
         }
     }*/

    public void GetItem(int count = 0)
    {

        for (int n = 0; n < invenSlots.Length; ++n) // item ��ġ�� ���Ƿ� �Ű������� �𸣴� �� �� �� �˻�
        {
            if (invenSlots[n].item != null) // item ������ null�� �ƴ϶��
            {
                if (invenSlots[n].item.myItem.orgData.itemId == myItem.orgData.itemId)
                //�̸��� ������ ���ǰ˻�
                {
                    print("�ߺ�");
                    this.transform.position = new Vector3(999f, 999f, 999f);
                    if (count != 0) { invenSlots[n].AddCount(count); } // �Ű������� ���� ���� �ִٸ�
                    else { invenSlots[n].AddCount(curNumber); } //���ڰ��� ������ ���� ������ ������ ����
                    invenSlots[n].FloatNotice(this.myItem.orgData.itemName); // �̸��÷���
                    SceneData.Inst.questItemCheckEvent?.Invoke();
                    return;
                }
            }
        }

        //������ ��ġ�� ������Ʈ Ǯ�� ���� �� �ֵ��� ���� �� ����.
        for (int i = 0; i < invenSlots.Length; ++i) // ���� ������ŭ �ݺ�
        {

            if (invenSlots[i].item == null) // slot�� item ������ null�̶��
            {
                this.transform.SetParent(SceneData.Inst.Inven.slots[i]); // �θ�� �� ����
                this.transform.position = new Vector3(999f, 999f, 999f); // ���� ȭ�鿡�� ������ �ʵ��� ������ �����ǿ� ��ġ���Ѻ���?
                this.transform.SetParent(SceneData.Inst.ItemPool);
                if (count != 0) { invenSlots[i].AddItem(this.GetComponent<Item>(), count); }
                else { invenSlots[i].AddItem(this.GetComponent<Item>(), curNumber); print("�Ѱ�"); }
                invenSlots[i].FloatNotice(this.myItem.orgData.itemName); // �̸��÷���
                SceneData.Inst.questItemCheckEvent?.Invoke();
                return; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
            }

        }
    }
    public void GetItemFromInteractableItems(GameObject what)
    {

        GameObject obj = Instantiate(what, new Vector3(999f, 999f, 999f), Quaternion.identity);
        obj.transform.SetParent(SceneData.Inst.ItemPool);

        for (int i = 0; i < invenSlots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (invenSlots[i].item != null && invenSlots[i].item.myItem.orgData.itemId == obj.GetComponent<Item>().myItem.orgData.itemId)
            {
                invenSlots[i].item.curNumber += obj.GetComponent<Item>().curNumber; // ������ ����
                invenSlots[i].count.text = invenSlots[i].item.curNumber.ToString(); // ���� �� �Ŀ� �ٽ� ȭ�鿡 ǥ��
                Destroy(obj.gameObject);
                return;
            }
        }

        //������ ��ġ�� ������Ʈ Ǯ�� ���� �� �ֵ��� ���� �� ����.
        for (int i = 0; i < invenSlots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (invenSlots[i].item == null) // �ڽ��� �ϳ��� �ִٸ�
            {
                invenSlots[i].AddItem(obj.GetComponent<Item>(),
                    obj.GetComponent<Item>().myItem.orgData.count);
                return; // ���ǰ˻翡 �ɸ��ٸ� �ݺ��� Ż��
            }
        }
    }

}
