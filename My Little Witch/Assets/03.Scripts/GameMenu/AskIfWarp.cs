using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskIfWarp : PointerCheck
{
    [SerializeField]private LevelLoader levelLoader;
    public void MoveOnTitle()
    {
        SceneData.Inst.myPlayer.SetPlayerBack();
        SceneData.Inst.myPlayer.OnUI = false;
        levelLoader.LoadTitleScene();
    }
    public void MoveOnDungeon()
    {
        SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
        SceneData.Inst.myPlayer.myAgent.enabled = false;
        SceneData.Inst.myPlayer.SetPlayerBack();
        SceneData.Inst.myPlayer.OnUI = false;
        levelLoader.LoadDungeonScene();
    }

    public void MoveOnTown()
    {
        SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
        SceneData.Inst.myPlayer.myAgent.enabled = false;
        SceneData.Inst.myPlayer.SetPlayerBack();
        SceneData.Inst.myPlayer.OnUI = false;
        levelLoader.LoadTownScene();
    }

    public void CancelLoad()
    {
        SceneData.Inst.myPlayer.SetPlayerBack();
        SceneData.Inst.myPlayer.OnUI = false;
        this.gameObject.SetActive(false);
    }

}
