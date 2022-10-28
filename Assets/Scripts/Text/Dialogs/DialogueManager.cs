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
        lastRoutine = StartCoroutine(TypeDialog(dialog.Lines[0]));

    }

    public void HandleUpdate()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return; // No gamepad connected.
        var mouse = Mouse.current;
        if (mouse == null)
            return;

        if(dialogIsOver)
            return;

        if (keyboard.enterKey.wasPressedThisFrame || mouse.leftButton.wasPressedThisFrame)
        {
            if (!isTyping)
            {
                ++currentLine;
                if (currentLine < dialog.Lines.Count)
                {
                    lastRoutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
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
                StopCoroutine(lastRoutine);
                dialogText.text = dialog.Lines[currentLine];
                isTyping = false;
            }
        }
            
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach(var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isTyping = false;
    }
}
