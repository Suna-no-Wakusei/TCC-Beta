using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableDialog : ScriptableObject
{
    public DialogueText dialogText;
    public DialogueText afterDialogText;
    public bool dialogAlreadyPlayed = false;
    public bool dialogStarted = false;
}
