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

    IEnumerator LateDialog()
    {
        GameManager.instance.hero.availableToInteract = false;
        yield return new WaitForSeconds(2f);
        GameManager.instance.hero.characterUnableToMove = true;
        GameManager.instance.hero.animator.SetFloat("Speed", 0);
        GameManager.instance.floorType = GameManager.FloorType.Null;
        Camera.main.transform.GetComponent<CameraMotor>().enabled = false;
        StartCoroutine(LerpFromTo(Camera.main.transform.position, dialog.camFocus, 1f));
        StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.lateDialogText));

        yield return new WaitUntil(() => DialogueManager.Instance.dialogIsOver);

        if (DialogueManager.Instance.dialogIsOver)
        {
            StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(GameManager.instance.hero.transform.position.x, GameManager.instance.hero.transform.position.y, -10), 1f));
            yield return new WaitForSeconds(1f);

            GameManager.instance.hero.characterUnableToMove = false;
            Camera.main.transform.GetComponent<CameraMotor>().enabled = true;
            GameManager.instance.hero.availableToInteract = true;
        }
    }

    IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        Camera.main.transform.position = pos2;
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

                if (dialog.lateDialog)
                    StartCoroutine(LateDialog());
            }
        }
    }
}