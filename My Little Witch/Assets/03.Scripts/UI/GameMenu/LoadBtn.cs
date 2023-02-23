using UnityEngine;

public class LoadBtn : MonoBehaviour
{
    [SerializeField] private GameObject loadTab;
    public void LoadButton()
    {
        loadTab.SetActive(true);
    }
}
