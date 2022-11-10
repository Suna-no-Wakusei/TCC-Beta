using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantBoss2 : Fighter
{
    public Laser laser;
    public Collider2D hitbox;

    public GameObject rootAttack;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("Attacking", 3f, 3f);
    }

    // Update is called once per frame
    void Attacking()
    {
        if(10.6 >= GameManager.instance.hero.transform.position.x && GameManager.instance.hero.transform.position.x >= 4.18)
        {
            if(-17.30 >= GameManager.instance.hero.transform.position.y && GameManager.instance.hero.transform.position.y >= -25.91)
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
        rootAttack.SetActive(true);

        yield return new WaitForSeconds(0.875f);

        rootAttack.SetActive(false);
    }

    protected override void Death()
    {
        rootAttack.SetActive(false);
        Destroy(gameObject);
    }

}
