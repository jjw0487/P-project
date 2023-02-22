using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] quests; //����Ʈ�� restricted lv �� �˻�

    [Header("NPCID ���� �迭�� �����Ͻÿ�.")]
    public Npc[] npc; // npcId ���� �迭�� �����Ͻÿ�. <= ���̺�ε�

    [SerializeField] private GameObject floatingQuestNotice;
    [SerializeField] private Transform eventNotice;

    // ��ųʸ� ���� => Ű������ ���� ����Ʈ�� ���������� �ߺ��� �Ǹ� �ȵȴ�.
    public List<QuestTab> questInProgress = new List<QuestTab>(); // ������ ����Ʈ üũ
    public List<int> questIndex = new List<int>(); //���̺�ε�
    public int n = 0; //���̺�ε�

    // �÷��̾��� ������ �޾� NPC�� DialogueTrigger.progress�� �����Ͽ� ����Ʈ�� �ر��Ѵ�.
    public void QM_LoadSavedQuest(int _questIndex)
    {
        // ����Ʈ ����
        if (quests[_questIndex] != null)
        {
            if (!questIndex.Contains(_questIndex)) // �̹� �����Ǿ� �ִٸ� ���̻� ���� ����.
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
                if(questInProgress.Count > 0) // �������� ����Ʈ�� �ִٸ� �����ϰ� Ʈ���Ÿ� �ߵ� �ϱ� ����
                {
                    for (int n = 0; n < questInProgress.Count; n++)
                    {
                        if (questInProgress[n].GetComponent<QuestTab>().questData.questIndex != quests[i].GetComponent<QuestTab>().questData.questIndex)
                        { //�̹� ��ϵ� ����Ʈ��� ����
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

    void QM_SetNpcTrigger(int npcId) // id�� �̿��Ͽ� �ش� npc�� �����Ͽ�
    {
        for(int i = npc[npcId].progress; i < npc[npcId].dialogue.Length; i++)
        {// ���� ���൵���� �迭�˻��Ͽ� ���� ����� ����Ʈ���� �����ϵ��� ���൵ �ø�
            if (npc[npcId].dialogue[i].type == DialogueData.Type.QuestGiver)
            {
                npc[npcId].minimapPin[0].SetActive(true); // �̴ϸ� �� false�� <DialogueManager->EndDialogue()>����
                npc[npcId].progress = i;
                GameObject obj = Instantiate(floatingQuestNotice, eventNotice);
                // ���̾�α׿��� ����Ʈ ����Ʈ�� ��� ����, �ε� �� �� �������� ����Ʈ�� ��������
                break;
            }
        }
    }

    public void QM_GetQuestSuccess(int npcId)
    {
        npc[npcId].progress++; // ��ȭ ���൵�� �÷� ������� ���� �� �� �ֵ���
        npc[npcId].minimapPin[1].SetActive(true); //exmark on
    }

    public void QM_GetTargetNpcQuestSuccess(int targetNpcId, int npcId)
    {
        for(int i = 0; i< npc[targetNpcId].dialogue.Length; i++)
        {
            if (npc[targetNpcId].dialogue[i].type == DialogueData.Type.Reward)
            {
                npc[targetNpcId].progress = i; // ��ȭ ���൵�� �÷� ������� ���� �� �� �ֵ���
                npc[targetNpcId].minimapPin[1].SetActive(true); //exmark on
                break;
            }
        }

        npc[npcId].progress++; // �Ƿ� npc
    }

    /*public void QM_GetBackToQuest(int npcId) // <<<<<<<<<<<<<<<<���� ����
    {
        npc[npcId].progress--; // ��ȭ ���൵�� �÷� ������� ���� �� �� �ֵ���
        npc[npcId].minimapPin[1].SetActive(false); //exmark on
    }*/

    public void QM_TurnMarkOff(int npcId) //������ ��ȭ ���� �� �̴ϸ� �� off
    {
        npc[npcId].minimapPin[1].SetActive(false);
    }

    public void QM_Reward(QuestTab questTab) // ���� ���� �� �������� ����Ʈ ��Ͽ��� ������� �Ѵ�.
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
