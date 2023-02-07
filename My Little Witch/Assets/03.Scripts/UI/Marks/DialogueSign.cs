using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSign : MarkController
{
    public void NextAction()
    {
        Destroy(gameObject);
    }
}
