using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestBook myQuestBook;
    public GameObject[] quests;
    public Npc[] npc;

    // 플레이어의 레벨을 받아 NPC의 DialogueTrigger.progress를 증가하여 퀘스트를 해금한다.
    public void QM_GetPlayerLevel(int lv)
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if (quests[i].GetComponent<QuestTab>().questData.restrictedLV == lv)
            {
                
                QM_SetNpcTrigger(quests[i].GetComponent<QuestTab>().questData.npcName);
                GameObject obj = Instantiate(quests[i], myQuestBook.content);
            }
        }
    }

    void QM_SetNpcTrigger(string npcName)
    {
        // progress, dialogueSign 추가
        if(npcName.Contains("Aranara"))
        {
            print("확인");
            npc[0].minimapPin.SetActive(true);
            npc[0].progress++;
        }
        else if (npcName.Contains("Test"))
        {
            npc[1].minimapPin.SetActive(true);
            npc[0].progress++;
        }
    }
}
