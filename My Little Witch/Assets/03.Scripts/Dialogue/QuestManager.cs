using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] quests; //퀘스트의 restricted lv 을 검사
    [SerializeField] private Npc[] npc;
    [SerializeField] private GameObject floatingQuestNotice;
    [SerializeField] private Transform eventNotice;

    // 딕셔너리 단점 => 키값으로 받을 퀘스트의 레벨제한이 중복이 되면 안된다.
    public List<int> questInProgress = new List<int>();

    // 플레이어의 레벨을 받아 NPC의 DialogueTrigger.progress를 증가하여 퀘스트를 해금한다.

    public void QM_GetPlayerLevel(int lv)
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if (quests[i].GetComponent<QuestTab>().questData.restrictedLV <= lv)
            {
                QM_SetNpcTrigger(quests[i].GetComponent<QuestTab>().questData.npcId, i);
            }
        }
    }

    void QM_SetNpcTrigger(int npcId, int index) // id를 이용하여 해당 npc에 접근하여
    {
        for(int i = npc[npcId].progress; i < npc[npcId].dialogue.Length; i++)
        {// 현재 진행도부터 배열검사하여 가장 가까운 퀘스트부터 실행하도록 진행도 올림
            if (npc[npcId].dialogue[i].type == DialogueData.Type.QuestGiver)
            {
                npc[npcId].minimapPin.SetActive(true); // 미니맵 핀 false는 <DialogueManager->EndDialogue()>에서
                npc[npcId].progress = i;
                GameObject obj = Instantiate(floatingQuestNotice, eventNotice);
                // 다이얼로그에서 퀘스트 리스트에 담아 저장, 로드 할 때 진행중인 퀘스트만 꺼내주자
                break;
            }
        }
    }
    public void reward(int questIndex)
    {
        questInProgress.Remove(questIndex);
    }

    public void SaveQuestProgress()
    {
        /*for(int i = 0; i < questInProgress.Count; i++)
        {

        }*/
    }

    public void LoadQuestData()
    {
        //Instantiate(quests[index], SceneData.Inst.dialogueManager.questBook.transform);
    }

}
