using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : Collidable, ICollectable
{
    public ScriptableDialog dialog;
    private bool itemAdded;
    private bool lateStarted;

    public void Collect()
    {
        if (dialog.objectiveID != 0)
        {
            if(GameManager.instance.objectiveManager.objectiveActive != null)
                if (dialog.objectiveID == GameManager.instance.objectiveManager.objectiveActive.id)
                {
                    if (!dialog.timeRunningDialog)
                        Time.timeScale = 0f;

                    dialog.dialogStarted = true;
                    StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.objectiveText));
                    return;
                }
        }
    }

    IEnumerator LateDialog()
    {
        GameManager.instance.hero.availableToInteract = false;
        yield return new WaitForSecondsRealtime(2f);
        GameManager.instance.hero.characterUnableToMove = true;
        GameManager.instance.hero.animator.SetFloat("Speed", 0);
        GameManager.instance.floorType = GameManager.FloorType.Null;
        Camera.main.transform.GetComponent<CameraMotor>().enabled = false;

        if (dialog.camFocus != Vector3.zero)
        {
            StartCoroutine(LerpFromTo(Camera.main.transform.position, dialog.camFocus, 1f));
        }

        StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.lateDialogText));

        yield return new WaitUntil(() => DialogueManager.Instance.dialogIsOver);

        if (DialogueManager.Instance.dialogIsOver)
        {
            StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(GameManager.instance.hero.transform.position.x, GameManager.instance.hero.transform.position.y, -10), 1f));
            yield return new WaitForSecondsRealtime(dialog.lateDelay);

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
            if (DialogueManager.Instance.currentLine == 3 && GameManager.instance.playerMode == 0)
            {
                GameManager.instance.playerMode = 1;
                SFXManager.instance.StopAmbient();
                SFXManager.instance.PlayMagicAmbient();
                StartCoroutine(GameManager.instance.ChangeModeAnim());
                GameManager.instance.globalVolume.profile = GameManager.instance.akemiProfile;
            }

            if (DialogueManager.Instance.currentLine == 100 && GameManager.instance.playerMode == 1)
            {
                GameManager.instance.playerMode = 0;
                GameManager.instance.globalVolume.profile = GameManager.instance.tamakiProfile;
            }

            if (DialogueManager.Instance.dialogIsOver)
            {
                if (this.GetComponent<ChangeObjective>() != null)
                {
                    this.GetComponent<ChangeObjective>().AddingObjectives();
                    dialog.dialogStarted = false;
                }
                dialog.dialogAlreadyPlayed = true;

                if (dialog.lateDialog && !lateStarted)
                {
                    lateStarted = true;
                    StartCoroutine(LateDialog());
                }
            }

            if (dialog.getItemOnDialog && dialog.dialogLineItem == DialogueManager.Instance.currentLine && !itemAdded)
            {
                itemAdded = true;
                GameManager.instance.inventory.AddItem(dialog.item);
                GameManager.instance.ShowText("+" + dialog.item.amount + " " + dialog.item.ItemName(), 20, Color.white, transform.position + new Vector3(0, (float)1), Vector3.up * 35, 1f);
            }
        }

        if (!dialog.timeRunningDialog)
            DialogueManager.Instance.dialogTimeStop = true;
        else
            DialogueManager.Instance.dialogTimeStop = false;
    }
}
