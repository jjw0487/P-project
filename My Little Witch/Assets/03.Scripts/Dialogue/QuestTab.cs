using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTab : MonoBehaviour
{
    public QuestData questData;
    public TMPro.TMP_Text contents;
    private void Awake()
    {
        contents.text = questData.contents;
    }
}
