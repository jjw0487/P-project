using System.Collections;
using UnityEngine;

public class OpeningScene : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    private void Start()
    {
        StartCoroutine(PressAnyKey());
    }
    IEnumerator PressAnyKey()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                levelLoader.LoadTitleScene();
                yield break;
            }
            yield return null;
        }

    }

}
