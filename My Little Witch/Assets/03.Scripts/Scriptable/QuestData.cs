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
    //public GameObject questObj; // ����Ʈ�Ͽ� �� object

    [Header("Selective")]
    [SerializeField]
    private int GoalNumber; // ����Ʈ ���� ���� reach��
    public int goalNumber { get { return GoalNumber; } }
  
    [SerializeField] 
    private int RestrictedLV; // ����Ʈ�� ��������
    public int restrictedLV { get { return RestrictedLV; } }

    [SerializeField]
    private GameObject Reward; // ����
    public GameObject reward { get { return Reward; } }

    [SerializeField]
    private int Currency; // ���� ��ȭ
    public int currency { get { return Currency; } }

    [SerializeField]
    private int Exp; //���� ����ġ
    public int exp { get { return Exp; } }
}
