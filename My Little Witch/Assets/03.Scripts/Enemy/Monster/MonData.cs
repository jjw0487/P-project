using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsData", menuName = "ScriptableObject/MonData", order = 1)]

public class MonData : ScriptableObject
{
    [field: SerializeField]
    public float HP
    {
        get;
        private set;
    }
    [field: SerializeField]
    public float AT
    {
        get; private set;
    }
    public float agentSpeed;
    public float attackRadius;
}
