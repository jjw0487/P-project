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

    Dictionary<int, int> questDictionary = new Dictionary<int, int>();


    // �÷��̾��� ������ �޾� NPC�� DialogueTrigger.progress�� �����Ͽ� ����Ʈ�� �ر��Ѵ�.
    // ����Ʈ ������ List�� dictionary�� �޾� �ʿ��� �� ����Ʈ�Ͽ� �����ϰ� �ش� �ε����� ���� �����ϴ� ������ ��������
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

    void QM_SetNpcTrigger(int npcId) // id�� �̿��Ͽ� �ش� npc�� �����Ͽ�
    {
        for(int i = npc[npcId].progress; i < npc[npcId].dialogue.Length; i++)
        {// ���� ���൵���� �迭�˻��Ͽ� ���� ����� ����Ʈ���� �����ϵ��� ���൵ �ø�
            if (npc[npcId].dialogue[i].type == DialogueData.Type.QuestGiver)
            {
                npc[npcId].minimapPin.SetActive(true); // �̴ϸ� �� false�� <DialogueManager->EndDialogue()>����
                npc[npcId].progress = i;
                GameObject obj = Instantiate(floatingQuestNotice, eventNotice);
                break;
            }
        }
    }
}
