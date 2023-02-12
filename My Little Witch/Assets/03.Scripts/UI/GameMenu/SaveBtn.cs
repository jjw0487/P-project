using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBtn : MonoBehaviour
{
    [SerializeField] private GameObject askIfSave;
    public void SaveButton()
    {
        askIfSave.SetActive(true);
    }
}
