using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorruptedTree : MonoBehaviour
{
    public ScriptableDialog dialog;
    public Vector2 bossPlayerPos;
    private Animator animator;
    private int i = 0;

    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == 10 && i == 0)
        {
            i = 1;
            GameManager.instance.sfxManager.rootAttack2.Play();
            animator.SetTrigger("Break");
            StartCoroutine(Dialog());
        }      
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

                GameManager.instance.nextScenePos = bossPlayerPos;
                SaveSystem.SaveState();

                StartCoroutine(FadeCo());
            }
        }
    }

    public IEnumerator FadeCo()
    {
        GameManager.instance.ChangeModeAnim();

        CameraShake.Shake(1f, 0.5f);
        GameManager.instance.sfxManager.thunder.Play();

        if (GameManager.instance.fadeOutPanel != null)
        {
            Instantiate(GameManager.instance.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        GameManager.instance.hero.availableToInteract = true;
        SceneManager.LoadScene("Boss");
    }
}