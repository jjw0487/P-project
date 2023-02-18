using UnityEngine;
using System;

public class SceneData : MonoBehaviour
{
    public static SceneData Inst = null;

    public Action questItemCheckEvent;

    public Player myPlayer;
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
    public QuestManager questManager;

    public Transform ItemPool;
    private void Awake()
    {
        Inst = this;
    }

}
