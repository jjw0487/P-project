using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTab : MonoBehaviour
{
    public QuestData questData;
    public TMPro.TMP_Text contents;

    public int progressNum; // 퀘스트 화면에 보여줄 진행도

    public bool isComplete = false;
    private void Awake()
    {
        contents.text = questData.contents;
        isComplete = false;
        progressNum = 0;
    }

    private void Start()
    {
        if (questData.type == QuestData.QuestType.Hunting)
        {
            MonsterSpawner.Inst.MS_GetQuestData(this);
        }
        else if (questData.type == QuestData.QuestType.Delivery)
        {

        }
        else if (questData.type == QuestData.QuestType.Gather)
        {

        }
    }

    public void QT_GetProgressNum(int count)
    {
        progressNum = count;
    }

    public void QT_GetSuccess()
    {
        isComplete = true;
        Instantiate(Resources.Load("UI/QuestComplete"), SceneData.Inst.interactionManager.transform);
        SceneData.Inst.questManager.QM_GetQuestSuccess(questData.npcId);
    }


}
