using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObject/DialogueData", order = 6)]

public class DialogueData : ScriptableObject
{
    public enum Type { QuestGiver, Repeat, Reward, Obstacle, Dialogue, OpenStore }
    public Type type;
    public int npcId;
    public string npcName;
    public string questName;
    [TextArea(3,10)]
    public string[] contents;

    [Header("Selective")]
    public GameObject questObj; // ����Ʈ�Ͽ� �� object
    public QuestData questRewardData; // ��������

}
