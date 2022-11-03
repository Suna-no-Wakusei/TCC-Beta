using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, ICollectable
{
    public ScriptableDialog dialog;

    public void Start()
    {
        if(GameManager.instance.weaponLevel == 1)
            Destroy(gameObject);
    }

    public void Collect()
    {
        if (dialog.dialogAlreadyPlayed == false)
        {
            GameManager.instance.weaponLevel = 1;
            if (DialogueManager.Instance.dialogRunning == false)
            {
                StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.dialogText));

                dialog.dialogStarted = true;
            }
        }
    }

    void Update()
    {
        if (dialog.dialogStarted)
        {
            if (DialogueManager.Instance.dialogIsOver)
            {
                dialog.dialogAlreadyPlayed = true;
                Destroy(this.gameObject);
            }
        }
    }
}
