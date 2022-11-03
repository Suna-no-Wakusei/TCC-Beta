using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] Image imageDialog;
    [SerializeField] int letterPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;
    public bool dialogRunning = false;
    public bool dialogIsOver;
    bool isTyping;

    Coroutine lastRoutine;

    public static DialogueManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    int currentLine = 0;
    DialogueText dialog;

    public IEnumerator ShowDialog(DialogueText dialog)
    {
        dialogIsOver = false;
        dialogRunning = true;
        yield return new WaitForEndOfFrame();
        
        OnShowDialog?.Invoke();

        this.dialog = dialog;

        dialogBox.SetActive(true);
        lastRoutine = StartCoroutine(TypeDialog(dialog.Lines[0], dialog.Icons[0]));
    }

    public void HandleUpdate()
    {
        if(dialogIsOver)
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
                    OnCloseDialog?.Invoke();
                    dialogIsOver = true;
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
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        GameManager.instance.sfxManager.dialogSound.Stop();
        GameManager.instance.sfxManager.dialogSound1.Stop();
        GameManager.instance.sfxManager.dialogSound2.Stop();
        isTyping = false;
    }
}
