using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableDialog : Collidable
{
    public ScriptableDialog dialog;

    public void OnCollide()
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
