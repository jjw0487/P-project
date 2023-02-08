using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObject/NPCData", order = 5)]

public class NPCData : MonoBehaviour
{
    [SerializeField]
    private int Progress;
    public float progress { get { return progress; } }
}
