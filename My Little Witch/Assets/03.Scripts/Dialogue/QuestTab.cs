using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class QuestTab : MonoBehaviour
{
    public QuestData questData;
    [SerializeField] private TMPro.TMP_Text[] nums;
    [SerializeField] private TMPro.TMP_Text content;
    private int progressNum; // ����Ʈ ȭ�鿡 ������ ���൵

    public bool isComplete = false;

    private void Awake()
    {
        isComplete = false;
        progressNum = 0;
        if (nums != null)
        {
            nums[0].text = questData.goalNumber.ToString();
            nums[1].text = 0.ToString();
        }
    }

    private void Start()
    {
        if (questData.type == QuestData.QuestType.Hunting)
        {
            MonsterSpawner.Inst.MS_GetQuestData(this);
        }
        else if (questData.type == QuestData.QuestType.Delivery)
        {
            SceneData.Inst.questItemCheckEvent += QT_FindQuestItemInInventory;
            SceneData.Inst.questItemCheckEvent.Invoke();
        }
        else if (questData.type == QuestData.QuestType.Gather)
        {
            //���� ������ ����
        }
    }

    private void QT_FindQuestItemInInventory()
    {
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item != null)
            {
                if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.myItem.orgData.name == questData.goalKeyword)
                //�̸��� ������ ���ǰ˻�
                {
                    isComplete = true;
                    QT_GetDeliveryQuestSuccess();
                    return;
                }
            }
        }
    }

    public void QT_GetProgressNum(int count)
    {
        progressNum = count;
        if (nums != null) nums[1].text = progressNum.ToString();
    }

    public void QT_GetSuccess()
    {
        isComplete = true;
        content.text = "����Ʈ �Ϸ� \nNCP�� ��ȭ�Ͻÿ�"; // ��ȭ���� �� �ؽ�Ʈ ���
        Instantiate(Resources.Load("UI/QuestComplete"), SceneData.Inst.interactionManager.transform);
        SceneData.Inst.questManager.QM_GetQuestSuccess(questData.npcId);
    }

    public void QT_GetDeliveryQuestSuccess()
    {
        isComplete = true;
        content.text = "����Ʈ �Ϸ� \nNCP�� ��ȭ�Ͻÿ�"; // ��ȭ���� �� �ؽ�Ʈ ���
        SceneData.Inst.questManager.QM_GetTargetNpcQuestSuccess(questData.targetNpcId, questData.npcId);
        SceneData.Inst.questItemCheckEvent -= QT_FindQuestItemInInventory; // ��������Ʈ ����
    }

    public void QT_DestroyQuestTab(QuestTab questTab)
    {
        SceneData.Inst.questManager.QM_Reward(questTab);
        Destroy(this.gameObject);
    }


}
