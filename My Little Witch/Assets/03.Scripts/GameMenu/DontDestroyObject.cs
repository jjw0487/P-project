using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyObject : MonoBehaviour
{
    public static DontDestroyObject instance = null;
    public Transform playerPos;
    public Transform camPos;

    void Awake() // called zero
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }

    private void OnEnable() // called first
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // called second
    {
        print("Scene on scene");

        if (scene.name == "Town")
        { 
            SceneData.Inst.questManager = FindObjectOfType<QuestManager>();
            SceneData.Inst.myPlayer.myAgent.enabled = true; // 위치값 변동 후에 켜준다.
            /*ChangeState(State.Menu);
            GamePanel_Sliders[0].onValueChanged.AddListener((float v) => GraphicManager.Inst.fbrightness = v);
            GamePanel_Sliders[1].onValueChanged.AddListener((float v) => GraphicManager.Inst.fconstrast = v);
            GamePanel_Sliders[2].onValueChanged.AddListener((float v) => SoundManager.Inst.bgmVolume = v);
            GamePanel_Sliders[3].onValueChanged.AddListener((float v) => SoundManager.Inst.effectVolume = v);
            newGameSceneName = "testScene";
            faderAnim?.SetTrigger("FadeIn");
            DisableUI();*/
        }

        if (scene.name == "Dungeon")
        {
            playerPos.position = new Vector3(161.59f, 25.0f, 132.47f);
            SceneData.Inst.myPlayer.myAgent.enabled = true; // 위치값 변동 후에 켜준다.
            SceneData.Inst.questManager = null; // 던전에서는 퀘스트 상호작용 없음
            /*newGameSceneName = "GameStage2";
            faderAnim.SetTrigger("FadeIn");*/
        }
    }

    void OnDisable() // called when the game is terminated
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
