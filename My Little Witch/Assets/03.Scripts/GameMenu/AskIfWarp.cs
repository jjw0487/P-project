using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskIfWarp : PointerCheck
{
    [SerializeField]private LevelLoader levelLoader;
    public void MoveOnTitle()
    {
        DontDestroyObject.instance.isWarping = false;
        SceneData.Inst.myPlayer.SetPlayerBack();
        //SceneData.Inst.myPlayer.OnUI = false;
        levelLoader.LoadTitleScene();
    }
    public void MoveOnDungeon()
    {
        DontDestroyObject.instance.isWarping = true;
        SceneData.Inst.myPlayer.myAgent.enabled = false;
        SceneData.Inst.myPlayer.SetPlayerBack();
        //SceneData.Inst.myPlayer.OnUI = false;
        levelLoader.LoadDungeonScene();
    }

    public void MoveOnTown()
    {
        DontDestroyObject.instance.isWarping = true;
        SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
        SceneData.Inst.myPlayer.myAgent.enabled = false;
        SceneData.Inst.myPlayer.SetPlayerBack();
        //SceneData.Inst.myPlayer.OnUI = false;
        levelLoader.LoadTownScene();
    }

    public void MoveOnTownFromDungeon()
    {
        SceneData.Inst.myPlayer.myAgent.enabled = false;
        DontDestroyObject.instance.isWarping = true;
        SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
        SceneData.Inst.myPlayer.SetPlayerBack();
        //SceneData.Inst.myPlayer.OnUI = false;
        levelLoader.LoadTownScene();
    }

    public void CancelLoad()
    {
        SceneData.Inst.myPlayer.SetPlayerBack();
        SceneData.Inst.myPlayer.OnUI = false;
        this.gameObject.SetActive(false);
    }

}
