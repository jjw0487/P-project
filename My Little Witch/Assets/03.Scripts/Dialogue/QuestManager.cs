using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] quests; //����Ʈ�� restricted lv �� �˻�
    [SerializeField] private Npc[] npc;
    [SerializeField] private GameObject floatingQuestNotice;
    [SerializeField] private Transform eventNotice;

    // ��ųʸ� ���� => Ű������ ���� ����Ʈ�� ���������� �ߺ��� �Ǹ� �ȵȴ�.
    public List<int> questInProgress = new List<int>();

    // �÷��̾��� ������ �޾� NPC�� DialogueTrigger.progress�� �����Ͽ� ����Ʈ�� �ر��Ѵ�.

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

    void QM_SetNpcTrigger(int npcId, int index) // id�� �̿��Ͽ� �ش� npc�� �����Ͽ�
    {
        for(int i = npc[npcId].progress; i < npc[npcId].dialogue.Length; i++)
        {// ���� ���൵���� �迭�˻��Ͽ� ���� ����� ����Ʈ���� �����ϵ��� ���൵ �ø�
            if (npc[npcId].dialogue[i].type == DialogueData.Type.QuestGiver)
            {
                npc[npcId].minimapPin.SetActive(true); // �̴ϸ� �� false�� <DialogueManager->EndDialogue()>����
                npc[npcId].progress = i;
                GameObject obj = Instantiate(floatingQuestNotice, eventNotice);
                // ���̾�α׿��� ����Ʈ ����Ʈ�� ��� ����, �ε� �� �� �������� ����Ʈ�� ��������
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
