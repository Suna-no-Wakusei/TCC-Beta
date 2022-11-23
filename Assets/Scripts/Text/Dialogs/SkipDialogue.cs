using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipDialogue : MonoBehaviour
{
    private bool xlr8 = false;

    public void Skip()
    {
        if (!xlr8)
        {
            xlr8 = true;
        }
        else
        {
            xlr8 = false;
        }
    }

    private void Update()
    {
        if (xlr8)
        {
            DialogueManager.Instance.letterPerSecond = 100;
            if (!DialogueManager.Instance.isTyping)
                DialogueManager.Instance.NextDialog();
        }
        else
        {
            DialogueManager.Instance.letterPerSecond = 45;
        }

        if (DialogueManager.Instance.dialogIsOver)
            xlr8 = false;
    }
}
