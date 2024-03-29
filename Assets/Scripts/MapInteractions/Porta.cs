using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Porta : Collidable
{
    public ScriptableDialog dialog;

    public string[] sceneNames;
    private string sceneName;
    public Vector2 playerPosition;
    public float fadeWait;

    private bool enterPortal = false;

    void OnTriggerEnter2D(Collider2D coll)
    {
        //teleport the player
        if (coll.name == "Hero")
        {
            if (GameManager.instance.weaponLevel == 0)
            {
                StartCoroutine(DialogueManager.Instance.ShowDialog(dialog.dialogText));
                return;
            }
            else
            {
                enterPortal = true;
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        //teleport the player
        if (coll.name == "Hero" && enterPortal)
        {
            sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            GameManager.instance.actualScene = sceneName;

            GameManager.instance.nextScenePos = playerPosition;
            SaveSystem.SaveState();
            StartCoroutine(FadeCo());
        }
    }

    public IEnumerator FadeCo()
    {
        if (GameManager.instance.fadeOutPanel != null)
        {
            Instantiate(GameManager.instance.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeWait);
        SceneManager.LoadScene(sceneName);
    }
}
