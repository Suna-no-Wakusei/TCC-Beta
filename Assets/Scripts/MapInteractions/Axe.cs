using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, ICollectable
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
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (dialog.dialogStarted)
        {
            if (DialogueManager.Instance.dialogIsOver)
            {
                dialog.dialogAlreadyPlayed = true;
            }
        }
        if (dialog.dialogAlreadyPlayed)
        {
            Destroy(this.gameObject);
        }
    }
}
