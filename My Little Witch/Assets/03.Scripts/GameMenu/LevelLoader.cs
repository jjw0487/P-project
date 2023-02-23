using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;

    public void LoadTownSceneWithSavedData()
    {
        DontDestroyObject.instance.withSavedData = true;
        StartCoroutine(LoadTownSceneCo());
    }


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // 애니메이션
        transition.SetTrigger("Start");
        // 기다리고
        yield return new WaitForSeconds(transitionTime);
        // 씬로드
        SceneManager.LoadScene(levelIndex);
    }


    // Title
    public void LoadTitleScene()
    {
        StartCoroutine(LoadTitleSceneCo());
    }

    IEnumerator LoadTitleSceneCo()
    {
        // 애니메이션
        transition.SetTrigger("Start");
        // 기다리고
        yield return new WaitForSeconds(transitionTime);
        // 씬로드
        SceneManager.LoadScene("Title");
    }

    // Dungeon
    public void LoadDungeonScene()
    {
        StartCoroutine(LoadDungeonSceneCo());
    }

    IEnumerator LoadDungeonSceneCo()
    {
        // 애니메이션
        transition.SetTrigger("Start");
        // 기다리고
        yield return new WaitForSeconds(transitionTime);
        // 씬로드
        SceneManager.LoadScene("Dungeon");
    }
    // Town

    public void LoadTownScene()
    {
        StartCoroutine(LoadTownSceneCo());
    }

    IEnumerator LoadTownSceneCo()
    {
        // 애니메이션
        transition.SetTrigger("Start");
        // 기다리고
        yield return new WaitForSeconds(transitionTime);
        // 씬로드
        SceneManager.LoadScene("Town");
    }


}
