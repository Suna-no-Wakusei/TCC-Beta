using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogAfterBoss : MonoBehaviour
{
    public ScriptableDialog dialogScript;
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] Image imageDialog;
    [SerializeField] int letterPerSecond;

    public bool dialogRunning = false;
    public bool dialogIsOver;
    bool isTyping;

    Coroutine lastRoutine;

    public static DialogueManager Instance { get; private set; }

    int currentLine = 0;
    DialogueText dialog;

    public IEnumerator ShowDialog(DialogueText dialog)
    {
        dialogIsOver = false;
        dialogRunning = true;
        yield return new WaitForEndOfFrame();
        GameManager.instance.hero.animator.SetFloat("Speed", 0);

        for (int i = 0; i < GameManager.instance.scriptableEnemies.Count; i++)
            GameManager.instance.scriptableEnemies[i].canMove = false;

        GameManager.instance.state = GameState.Paused;

        this.dialog = dialog;

        dialogBox.SetActive(true);
        lastRoutine = StartCoroutine(TypeDialog(dialog.Lines[0], dialog.Icons[0]));
    }

    public void Update()
    {
        if (dialogIsOver)
            return;

        if (dialogScript.dialogAlreadyPlayed)
            return;

        if (Keyboard.current.enterKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isTyping)
            {
                ++currentLine;
                if (currentLine < dialog.Lines.Count)
                {
                    lastRoutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine], dialog.Icons[currentLine]));
                }
                else
                {
                    currentLine = 0;
                    dialogBox.SetActive(false);
                    dialogRunning = false;
                    GameManager.instance.state = GameState.FreeRoam;
                    dialogIsOver = true;
                    dialogScript.dialogAlreadyPlayed = true;
                    Time.timeScale = 1f;

                    for (int i = 0; i < GameManager.instance.scriptableEnemies.Count; i++)
                        GameManager.instance.scriptableEnemies[i].canMove = true;

                    GameManager.instance.playerMode = 0;
                    SFXManager.instance.StopMagicAmbient();
                    SFXManager.instance.PlayAmbient();
                    GameManager.instance.globalVolume.profile = GameManager.instance.tamakiProfile;
                }
            }
            else
            {
                GameManager.instance.sfxManager.dialogSound.Stop();
                GameManager.instance.sfxManager.dialogSound1.Stop();
                GameManager.instance.sfxManager.dialogSound2.Stop();
                StopCoroutine(lastRoutine);
                dialogText.text = dialog.Lines[currentLine];
                isTyping = false;
            }
        }

    }

    public IEnumerator TypeDialog(string line, Sprite icon)
    {
        int i = UnityEngine.Random.Range(0, 2);

        isTyping = true;
        dialogText.text = "";
        imageDialog.sprite = icon;
        foreach (var letter in line.ToCharArray())
        {
            switch (i)
            {
                case 0:
                    GameManager.instance.sfxManager.PlayDialogSound();
                    break;
                case 1:
                    GameManager.instance.sfxManager.PlayDialogSound1();
                    break;
                case 2:
                    GameManager.instance.sfxManager.PlayDialogSound2();
                    break;
            }

            dialogText.text += letter;
            yield return new WaitForSecondsRealtime(1f / letterPerSecond);
        }
        GameManager.instance.sfxManager.dialogSound.Stop();
        GameManager.instance.sfxManager.dialogSound1.Stop();
        GameManager.instance.sfxManager.dialogSound2.Stop();
        isTyping = false;
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

    private void Awake()
    {
        if(!dialogScript.dialogAlreadyPlayed)
            StartCoroutine(ShowDialog(dialogScript.dialogText));
    }
}
