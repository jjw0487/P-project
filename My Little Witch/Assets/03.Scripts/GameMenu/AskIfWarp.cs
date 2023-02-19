using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskIfWarp : MonoBehaviour
{
    [SerializeField]private LevelLoader levelLoader;
    public void MoveOnTitle()
    {
        SceneData.Inst.myPlayer.SetPlayerBack();
        levelLoader.LoadTitleScene();
    }
    public void MoveOnDungeon()
    {
        SceneData.Inst.myPlayer.SetPlayerBack();
        levelLoader.LoadDungeonScene();
    }

    public void MoveOnTown()
    {
        SceneData.Inst.myPlayer.SetPlayerBack();
        levelLoader.LoadTownScene();
    }

    public void CancelLoad()
    {
        SceneData.Inst.myPlayer.SetPlayerBack();
        this.gameObject.SetActive(false);
    }

}
