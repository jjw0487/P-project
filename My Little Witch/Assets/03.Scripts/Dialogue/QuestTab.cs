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
    private int progressNum; // 퀘스트 화면에 보여줄 진행도

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
            //아직 만들지 않음
        }
    }

    private void QT_FindQuestItemInInventory()
    {
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // 슬롯 수량만큼 반복
        {
            if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item != null)
            {
                if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.myItem.orgData.name == questData.goalKeyword)
                //이름이 같은지 조건검사
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
        content.text = "퀘스트 완료 \nNCP와 대화하시오"; // 대화성공 후 텍스트 띄움
        Instantiate(Resources.Load("UI/QuestComplete"), SceneData.Inst.interactionManager.transform);
        SceneData.Inst.questManager.QM_GetQuestSuccess(questData.npcId);
    }

    public void QT_GetDeliveryQuestSuccess()
    {
        isComplete = true;
        content.text = "퀘스트 완료 \nNCP와 대화하시오"; // 대화성공 후 텍스트 띄움
        SceneData.Inst.questManager.QM_GetTargetNpcQuestSuccess(questData.targetNpcId, questData.npcId);
        SceneData.Inst.questItemCheckEvent -= QT_FindQuestItemInInventory; // 델리게이트 삭제
    }

    public void QT_DestroyQuestTab(QuestTab questTab)
    {
        SceneData.Inst.questManager.QM_Reward(questTab);
        Destroy(this.gameObject);
    }


}
