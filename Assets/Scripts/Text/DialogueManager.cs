using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;
    public bool dialogRunning = false;
    bool isTyping;

    Coroutine lastRoutine;

    public static DialogueManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

        DontDestroyOnLoad(gameObject);
    }

    int currentLine = 0;
    DialogueText dialog;

    public IEnumerator ShowDialog(DialogueText dialog)
    {
        dialogRunning = true;
        yield return new WaitForEndOfFrame();
        
        OnShowDialog?.Invoke();

        this.dialog = dialog;
        dialogBox.SetActive(true);
        lastRoutine = StartCoroutine(TypeDialog(dialog.Lines[0]));

    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
