using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cama : Collidable, ICollectable
{
    public ScriptableDialog dialog;
    private bool itemAdded;
    private bool lateStarted;

    public void Collect()
    {
        if (GameManager.instance.objectiveManager.objectiveActive != null)
            if (dialog.objectiveID != 0)
            {
                if (dialog.objectiveID == GameManager.instance.objectiveManager.objectiveActive.id)
                {
                    if (!dialog.timeRunningDialog)
                        Time.timeScale = 0f;

                    StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.objectiveText));
                    return;
                }
            }

        if (dialog.dialogAlreadyPlayed == false)
        {
            if (DialogueManager.Instance.dialogRunning == false)
            {
                if (!dialog.timeRunningDialog)
                    Time.timeScale = 0f;

                StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.dialogText));

                if (this.GetComponent<ChangeObjective>() != null)
                    this.GetComponent<ChangeObjective>().FinishingObjectives();

                dialog.dialogStarted = true;
            }
        }
        else if (dialog.afterDialogText.Lines.Count != 0)
        {
            if (!dialog.timeRunningDialog)
                Time.timeScale = 0f;

            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.afterDialogText));
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

                GameManager.instance.actualScene = "Casa_4";
                SaveSystem.SaveState();

                StartCoroutine(FadeCo());
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

    public IEnumerator FadeCo()
    {
        if (GameManager.instance.fadeOutPanel != null)
        {
            Instantiate(GameManager.instance.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        GameManager.instance.hero.availableToInteract = true;
        SceneManager.LoadScene("Casa_4");

        Destroy(gameObject);
    }
}
