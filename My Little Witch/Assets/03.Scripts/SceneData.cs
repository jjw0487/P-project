using UnityEngine;
using System;

public class SceneData : MonoBehaviour
{
    public static SceneData Inst = null;

    public Action questItemCheckEvent;

    public Player myPlayer = null;
    public Skill mySkill;

    public Transform HPBars;
    public Transform FloatingDmg;

    public Transform InvenSlot;
    public Transform EagleExitPos;
    public Inventory Inven;
    public InteractionManager interactionManager;
    public InteractableUIManager interactableUIManager;
    public DialogueManager dialogueManager;
    public Animator talkSign;
    public QuestManager questManager = null;
    public Transform ItemPool;


    private void Awake()
    {

        Inst = this;

    }

    private void Start()
    {
        if (myPlayer == null) { myPlayer = FindObjectOfType<Player>(); }

        if (FindObjectOfType<QuestManager>()) { questManager = FindObjectOfType<QuestManager>(); }
        else { questManager = null; }
        
    }

}
