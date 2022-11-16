using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ScriptableDialog : ScriptableObject
{
    public DialogueText dialogText;
    public DialogueText afterDialogText;
    public DialogueText objectiveText;
    public int objectiveID;
    public bool finishObjective;
    public bool timeRunningDialog;
    public bool dialogAlreadyPlayed = false;
    public bool dialogStarted = false;
    public bool lateDialog = false;
    public DialogueText lateDialogText;
    public Vector3 camFocus;
    public float lateDelay;
    public bool getItemOnDialog;
    public Item item;
    public int dialogLineItem;
}
