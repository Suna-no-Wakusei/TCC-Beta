using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TreantBoss1 : Fighter
{
    Animator animator;
    public ScriptableDialog dialogScript;
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] Image imageDialog;
    [SerializeField] int letterPerSecond;

    [SerializeField] GameObject TreantBoss2;
    [SerializeField] BoxCollider2D hitbox;

    public GameObject treantPF;
    public Vector2 posSpawn;
    private bool spawnEnemies = false;

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

        if (Keyboard.current.enterKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isTyping)
            {
                ++currentLine;
                if (currentLine < dialog.Lines.Count)
                {
                    lastRoutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine], dialog.Icons[currentLine]));
                    switch (currentLine)
                    {
                        case 7:
                            GameManager.instance.playerMode = 1;
                            SFXManager.instance.StopAmbient();
                            SFXManager.instance.PlayMagicAmbient();
                            StartCoroutine(GameManager.instance.ChangeModeAnim());
                            GameManager.instance.globalVolume.profile = GameManager.instance.akemiProfile;
                            break;
                        case 11:
                            GameManager.instance.playerMode = 0;
                            SFXManager.instance.StopMagicAmbient();
                            SFXManager.instance.PlayAmbient();
                            GameManager.instance.globalVolume.profile = GameManager.instance.tamakiProfile;
                            break;
                        case 21:
                            GameManager.instance.playerMode = 1;
                            SFXManager.instance.StopAmbient();
                            SFXManager.instance.PlayMagicAmbient();
                            StartCoroutine(GameManager.instance.ChangeModeAnim());
                            GameManager.instance.globalVolume.profile = GameManager.instance.akemiProfile;
                            Camera.main.transform.GetComponent<CameraMotor>().enabled = false;
                            StartCoroutine(LerpFromTo(Camera.main.transform.position, dialogScript.camFocus, 1f));
                            break;
                        case 26:
                            StartCoroutine(LerpFromTo(Camera.main.transform.position, dialogScript.camFocus, 1f));
                            break;
                        case 27:
                            Camera.main.transform.GetComponent<CameraMotor>().enabled = true;
                            break;
                        case 30:
                            GameManager.instance.magicProficiency = 1;
                            GameManager.instance.xpPoints = 1;
                            Spell spell = new Spell();
                            spell.spellType = Spell.SpellType.Fireball;
                            GameManager.instance.spellBook.AddSpell(spell);
                            GameManager.instance.UseSpell(spell);
                            GameManager.instance.currentMana = 0;
                            break;
                    }
                }
                else
                {
                    currentLine = 0;
                    dialogBox.SetActive(false);
                    dialogRunning = false;
                    GameManager.instance.state = GameState.FreeRoam;
                    dialogIsOver = true;
                    Time.timeScale = 1f;

                    for (int i = 0; i < GameManager.instance.scriptableEnemies.Count; i++)
                        GameManager.instance.scriptableEnemies[i].canMove = true;

                    spawnEnemies = true;

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

    private void SpawnEnemies()
    {
        if (!spawnEnemies) return;

        GameObject treantGO = Instantiate(treantPF, posSpawn, Quaternion.identity);
        Treant treant = treantGO.GetComponent<Treant>();

        treant.enemy = (ScriptableEnemy)ScriptableObject.CreateInstance("ScriptableEnemy");

        treant.xpValue = 0;

        treant.enemy.alive = true;
        treant.enemy.canMove = true;
    }

    private void Awake()
    {
        StartCoroutine(ShowDialog(dialogScript.dialogText));

        InvokeRepeating("SpawnEnemies", 2f, 10f);

        animator = GetComponent<Animator>();
    }

    protected override void Death()
    {
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        GameManager.instance.sfxManager.PlayBossTreantDamage();
        hitbox.gameObject.SetActive(false);
        yield return null;
        StartCoroutine(FadeCo());
    }

    public IEnumerator FadeCo()
    {
        Transform transform1 = null;
        if (GameManager.instance.fadeOutPanel != null)
        {
            transform1 = Instantiate(GameManager.instance.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        animator.SetTrigger("Death");
        yield return new WaitForSecondsRealtime(1f);
        Destroy(transform1.gameObject);
        Destroy(gameObject);

        StartCoroutine(GameManager.instance.FadeStart());

        TreantBoss2.SetActive(true);
    }
}
