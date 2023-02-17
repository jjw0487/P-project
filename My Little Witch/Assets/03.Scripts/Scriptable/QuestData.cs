using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObject/QuestData", order = 5)]

public class QuestData : ScriptableObject
{
    public enum QuestType { Hunting, Delivery, Gather }
    public QuestType type;

    public int questIndex;
    public string questName;
    public int npcId;
    [TextArea(3, 10)]
    public string contents;
    //public GameObject questObj; // 퀘스트북에 들어갈 object

    [Header("Selective")]
    [SerializeField]
    private int GoalNumber; // 퀘스트 최종 성공 reach값
    public int goalNumber { get { return GoalNumber; } }
  
    [SerializeField] 
    private int RestrictedLV; // 퀘스트의 레벨제한
    public int restrictedLV { get { return RestrictedLV; } }

    [SerializeField]
    private GameObject Reward; // 보상
    public GameObject reward { get { return Reward; } }

    [SerializeField]
    private int Currency; // 보상 재화
    public int currency { get { return Currency; } }

    [SerializeField]
    private int Exp; //보상 경험치
    public int exp { get { return Exp; } }
}
