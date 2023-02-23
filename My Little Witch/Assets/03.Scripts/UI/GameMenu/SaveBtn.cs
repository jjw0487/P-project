using UnityEngine;

public class SaveBtn : MonoBehaviour
{
    [SerializeField] private GameObject[] askIfSaveAndLoad;
    public void SaveButton()
    {
        askIfSaveAndLoad[0].SetActive(true);
    }

    public void LoadButton()
    {
        askIfSaveAndLoad[1].SetActive(true);
    }
}
