using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorruptedTree : MonoBehaviour, ICollectable
{
    public ScriptableDialog dialog;

    public void Collect()
    {
        Debug.Log("Collect");
        StartCoroutine(Dialog());
    }

    IEnumerator Dialog()
    {
        StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.dialogText));

        yield return new WaitUntil(() => DialogueManager.Instance.dialogIsOver);

        GameManager.instance.hero.characterUnableToMove = false;
        GameManager.instance.hero.availableToInteract = false;

        yield return new WaitForSeconds(1f);

        GameManager.instance.hero.characterUnableToMove = true;

        if (DialogueManager.Instance.dialogIsOver)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.afterDialogText));

            yield return new WaitUntil(() => DialogueManager.Instance.dialogIsOver);

            if (DialogueManager.Instance.dialogIsOver)
            {
                GameManager.instance.actualScene = "Boss";

                GameManager.instance.nextScenePos = new Vector2(14.23f, -8.56f);
                SaveSystem.SaveState();

                StartCoroutine(FadeCo());
            }
        }
    }

    public IEnumerator FadeCo()
    {
        GameManager.instance.ChangeModeAnim();
        if (GameManager.instance.fadeOutPanel != null)
        {
            Instantiate(GameManager.instance.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        GameManager.instance.hero.availableToInteract = true;
        SceneManager.LoadScene("Boss");
    }
}