using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObject/DialogueData", order = 6)]

public class DialogueData : ScriptableObject
{
    public enum Type { QuestGiver, Reward, Obstacle, Dialogue}
    public Type type;
    public string npcName;
    public string questName;
    [TextArea(3,10)]
    public string[] contents;
    

    // 반복형인지 타입 설정
}
