using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;
    

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // �ִϸ��̼�
        transition.SetTrigger("Start");
        // ��ٸ���
        yield return new WaitForSeconds(transitionTime);
        // ���ε�
        SceneManager.LoadScene(levelIndex);
    }

    // Title
    public void LoadTitleScene()
    {
        StartCoroutine(LoadTitleSceneCo());
    }

    IEnumerator LoadTitleSceneCo()
    {
        // �ִϸ��̼�
        transition.SetTrigger("Start");
        // ��ٸ���
        yield return new WaitForSeconds(transitionTime);
        // ���ε�
        SceneManager.LoadScene("Title");
    }

    // Dungeon
    public void LoadDungeonScene()
    {
        StartCoroutine(LoadDungeonSceneCo());
    }

    IEnumerator LoadDungeonSceneCo()
    {
        // �ִϸ��̼�
        transition.SetTrigger("Start");
        // ��ٸ���
        yield return new WaitForSeconds(transitionTime);
        // ���ε�
        SceneManager.LoadScene("Dungeon");
    }
    // Town

    public void LoadTownScene()
    {
        StartCoroutine(LoadTownSceneCo());
    }

    IEnumerator LoadTownSceneCo()
    {
        // �ִϸ��̼�
        transition.SetTrigger("Start");
        // ��ٸ���
        yield return new WaitForSeconds(transitionTime);
        // ���ε�
        SceneManager.LoadScene("Town");
    }

    
}
