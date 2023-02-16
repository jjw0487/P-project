using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTab : MonoBehaviour
{
    public QuestData questData;
    public TMPro.TMP_Text contents;

    public bool isComplete = false;
    private void Awake()
    {
        contents.text = questData.contents;
        isComplete = false;
    }

    private void Start()
    {
        if(questData.type == QuestData.QuestType.Hunting)
        {
            MonsterSpawner.Inst.GetQuestData(questData);
        }
    }


}
