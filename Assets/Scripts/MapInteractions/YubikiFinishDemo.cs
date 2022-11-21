using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YubikiFinishDemo : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.objectiveManager.objectiveActive.id == 8)
            if (DialogueManager.Instance.dialogIsOver)
                StartCoroutine(FadeCo());
    }

    public IEnumerator FadeCo()
    {
        if (GameManager.instance.fadeOutPanel != null)
        {
            Instantiate(GameManager.instance.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }

        GameManager.instance.hero.availableToInteract = false;
        GameManager.instance.hero.characterUnableToMove = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("0Credits");
    }
}
