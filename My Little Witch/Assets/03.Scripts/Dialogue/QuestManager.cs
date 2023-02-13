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

    Dictionary<int, int> questDictionary = new Dictionary<int, int>();


    // 플레이어의 레벨을 받아 NPC의 DialogueTrigger.progress를 증가하여 퀘스트를 해금한다.
    // 퀘스트 정보를 List나 dictionary로 받아 필요할 때 퀘스트북에 전달하고 해당 인덱스의 값을 삭제하는 로직을 가져야함
    private void Start()
    {
        
    }
    public void QM_GetPlayerLevel(int lv)
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if (quests[i].GetComponent<QuestTab>().questData.restrictedLV <= lv)
            {
                QM_SetNpcTrigger(quests[i].GetComponent<QuestTab>().questData.npcId);
            }
        }
    }

    void QM_SetNpcTrigger(int npcId) // id를 이용하여 해당 npc에 접근하여
    {
        for(int i = npc[npcId].progress; i < npc[npcId].dialogue.Length; i++)
        {// 현재 진행도부터 배열검사하여 가장 가까운 퀘스트부터 실행하도록 진행도 올림
            if (npc[npcId].dialogue[i].type == DialogueData.Type.QuestGiver)
            {
                npc[npcId].minimapPin.SetActive(true); // 미니맵 핀 false는 <DialogueManager->EndDialogue()>에서
                npc[npcId].progress = i;
                GameObject obj = Instantiate(floatingQuestNotice, eventNotice);
                break;
            }
        }
    }
}
