using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObject/QuestData", order = 5)]

public class QuestData : ScriptableObject
{
    public string questName;
    public int npcId;
    [TextArea(3, 10)]
    public string contents;
    //public GameObject questObj; // Äù½ºÆ®ºÏ¿¡ µé¾î°¥ object

    [Header("Selective")]
    public int restrictedLV;
    public GameObject reward;
    public int currency;
    public int exp;
}
