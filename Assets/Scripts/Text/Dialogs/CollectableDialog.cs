using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableDialog : Collidable, ICollectable
{
    public ScriptableDialog dialog;

    public void Collect()
    {
        if (dialog.dialogAlreadyPlayed == false)
        {
            if (DialogueManager.Instance.dialogRunning == false)
            {
                StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.dialogText));

                dialog.dialogStarted = true;
            }
        }
        else if (dialog.afterDialogText.Lines.Count != 0)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.afterDialogText));
        }
    }

    protected override void Update()
    {
        if (dialog.dialogStarted)
        {
            if (DialogueManager.Instance.dialogIsOver)
            {
                if (this.GetComponent<ChangeObjective>() != null)
                {
                    this.GetComponent<ChangeObjective>().HandleUpdate();
                    dialog.dialogStarted = false;
                }
                dialog.dialogAlreadyPlayed = true;
            }
        }
    }
}