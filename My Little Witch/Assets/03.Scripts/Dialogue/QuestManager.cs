using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] quests; //퀘스트의 restricted lv 을 검사

    [Header("NPCID 순서 배열로 나열하시오.")]
    public Npc[] npc; // npcId 순서 배열로 나열하시오. <= 세이브로드

    [SerializeField] private GameObject floatingQuestNotice;
    [SerializeField] private Transform eventNotice;

    // 딕셔너리 단점 => 키값으로 받을 퀘스트의 레벨제한이 중복이 되면 안된다.
    public List<QuestTab> questInProgress = new List<QuestTab>(); // 진행중 퀘스트 체크
    public List<int> questIndex = new List<int>(); //세이브로드
    public int n = 0; //세이브로드

    // 플레이어의 레벨을 받아 NPC의 DialogueTrigger.progress를 증가하여 퀘스트를 해금한다.
    public void QM_LoadSavedQuest(int _questIndex)
    {
        // 퀘스트 생성
        if (quests[_questIndex] != null)
        {
            if (!questIndex.Contains(_questIndex)) // 이미 생성되어 있다면 더이상 생성 안함.
            {
                GameObject obj = Instantiate(quests[_questIndex], SceneData.Inst.dialogueManager.questBook.content);
            }
        }   
    }

    public void QM_LoadSavedNpcProgress(int _progress)
    {
        if (npc[n] != null) npc[n].progress = _progress;
        n++;
    }


    public void QM_GetPlayerLevel(int lv)
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if (quests[i].GetComponent<QuestTab>().questData.restrictedLV <= lv)
            {
                if(questInProgress.Count > 0) // 진행중인 퀘스트가 있다면 제외하고 트리거를 발동 하기 위함
                {
                    for (int n = 0; n < questInProgress.Count; n++)
                    {
                        if (questInProgress[n].GetComponent<QuestTab>().questData.questIndex != quests[i].GetComponent<QuestTab>().questData.questIndex)
                        { //이미 등록된 퀘스트라면 제외
                            QM_SetNpcTrigger(quests[i].GetComponent<QuestTab>().questData.npcId);
                            break;
                        }
                    }
                }
                else
                {
                    QM_SetNpcTrigger(quests[i].GetComponent<QuestTab>().questData.npcId);
                }
            }
        }
    }

    void QM_SetNpcTrigger(int npcId) // id를 이용하여 해당 npc에 접근하여
    {
        for(int i = npc[npcId].progress; i < npc[npcId].dialogue.Length; i++)
        {// 현재 진행도부터 배열검사하여 가장 가까운 퀘스트부터 실행하도록 진행도 올림
            if (npc[npcId].dialogue[i].type == DialogueData.Type.QuestGiver)
            {
                npc[npcId].minimapPin[0].SetActive(true); // 미니맵 핀 false는 <DialogueManager->EndDialogue()>에서
                npc[npcId].progress = i;
                GameObject obj = Instantiate(floatingQuestNotice, eventNotice);
                // 다이얼로그에서 퀘스트 리스트에 담아 저장, 로드 할 때 진행중인 퀘스트만 꺼내주자
                break;
            }
        }
    }

    public void QM_GetQuestSuccess(int npcId)
    {
        npc[npcId].progress++; // 대화 진행도를 올려 리워드로 진행 할 수 있도록
        npc[npcId].minimapPin[1].SetActive(true); //exmark on
    }

    public void QM_GetTargetNpcQuestSuccess(int targetNpcId, int npcId)
    {
        for(int i = 0; i< npc[targetNpcId].dialogue.Length; i++)
        {
            if (npc[targetNpcId].dialogue[i].type == DialogueData.Type.Reward)
            {
                npc[targetNpcId].progress = i; // 대화 진행도를 올려 리워드로 진행 할 수 있도록
                npc[targetNpcId].minimapPin[1].SetActive(true); //exmark on
                break;
            }
        }

        npc[npcId].progress++; // 의뢰 npc
    }

    /*public void QM_GetBackToQuest(int npcId) // <<<<<<<<<<<<<<<<아직 안함
    {
        npc[npcId].progress--; // 대화 진행도를 올려 리워드로 진행 할 수 있도록
        npc[npcId].minimapPin[1].SetActive(false); //exmark on
    }*/

    public void QM_TurnMarkOff(int npcId) //리워드 대화 시작 시 미니맵 핀 off
    {
        npc[npcId].minimapPin[1].SetActive(false);
    }

    public void QM_Reward(QuestTab questTab) // 보상 진행 후 진행중인 퀘스트 목록에서 지워줘야 한다.
    {
        questInProgress.Remove(questTab);
        questIndex.Remove(questTab.questData.questIndex);
    }

    public void QM_SaveQuestProgress()
    {
        /*for(int i = 0; i < questInProgress.Count; i++)
        {

        }*/
    }

    public void QM_LoadQuestData()
    {
        //Instantiate(quests[index], SceneData.Inst.dialogueManager.questBook.transform);
    }

}
