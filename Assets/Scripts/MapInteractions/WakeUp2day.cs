using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp2day : MonoBehaviour
{
    public ScriptableDialog dialog;

    private void Awake()
    {
        dialog.dialogStarted = true;
        StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.dialogText));
    }

    private void Update()
    {
        if (dialog.dialogStarted)
        {
            if (DialogueManager.Instance.dialogIsOver)
            {
                this.GetComponent<ChangeObjective>().AddingObjectives();
                dialog.dialogStarted = false;
                dialog.dialogAlreadyPlayed = true;
            }
        }
    }
}
