using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable

{
    public string[] sceneNames;
    public Vector2 playerPosition;
    public TeleportPoint Teleport;

    protected override void OnCollide(Collider2D coll)
    {
        //teleport the player
        if(coll.name == "Hero")
        {
            GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];

            Teleport.initialValue = playerPosition;
            SceneManager.LoadScene(sceneName);
        }
    }
}
