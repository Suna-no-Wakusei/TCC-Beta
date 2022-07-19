using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable

{
    public string[] sceneNames;
    private string sceneName;
    public Vector2 playerPosition;
    public TeleportPoint Teleport;
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    public void Awake()
    {
        if(fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        //teleport the player
        if(coll.name == "Hero")
        {
            GameManager.instance.SaveState();
            sceneName = sceneNames[Random.Range(0, sceneNames.Length)];

            Teleport.initialValue = playerPosition;
            GameManager.instance.actualScene = sceneName;
            StartCoroutine(FadeCo());
        }
    }

    public IEnumerator FadeCo()
    {
        if(fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeWait);
        SceneManager.LoadScene(sceneName);
    }
}
