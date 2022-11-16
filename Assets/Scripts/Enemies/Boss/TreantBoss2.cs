using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreantBoss2 : Fighter
{
    public int xpValue;
    public Laser laser;
    public Collider2D hitbox;
    public float leftCorner, rightCorner, bottomCorner, topCorner;

    public GameObject rootAttack;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("Attacking", 3f, 3f);
    }

    // Update is called once per frame
    void Attacking()
    {
        if(rightCorner >= GameManager.instance.hero.transform.position.x && GameManager.instance.hero.transform.position.x >= leftCorner)
        {
            if(topCorner >= GameManager.instance.hero.transform.position.y && GameManager.instance.hero.transform.position.y >= bottomCorner)
            {
                int i;

                i = Random.Range(0, 2);

                switch (i)
                {
                    case 0:
                        if (!laser.laserRunning)
                        {
                            laser.PlayLaser();
                        }
                        break;
                    case 1:
                        StartCoroutine(RootAttack());
                        break;
                }
            }
        }
    }

    IEnumerator RootAttack()
    {
        rootAttack.transform.position = GameManager.instance.hero.transform.position;
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.sfxManager.PlayRootAttack();
        rootAttack.SetActive(true);

        yield return new WaitForSeconds(1.375f);

        rootAttack.SetActive(false);
    }

    protected override void Death()
    {
        rootAttack.SetActive(false);

        GameManager.instance.sfxManager.PlayBossTreantDamage();
        GameManager.instance.experience += xpValue;

        GameManager.instance.actualScene = "Level4";
        SaveSystem.SaveState();

        StartCoroutine(FadeCo());
    }

    public IEnumerator FadeCo()
    {
        GameManager.instance.ChangeModeAnim();
        if (GameManager.instance.fadeOutPanel != null)
        {
            Instantiate(GameManager.instance.fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        GameManager.instance.hero.availableToInteract = true;
        SceneManager.LoadScene("Level4");

        Destroy(gameObject);
    }

}
