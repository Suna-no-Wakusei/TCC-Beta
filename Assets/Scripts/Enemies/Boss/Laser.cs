using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool laserRunning = false;

    public GameObject laser;
    public Transform laserLight;
    public SpriteRenderer laserSR;
    public Vector3 startingPos;

    public Coroutine lastLaser = null;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        laserSR = laser.GetComponent<SpriteRenderer>();
    }

    public void PlayLaser()
    {
        lastLaser = StartCoroutine(LaserPlay(GameManager.instance.hero.transform.position));
    }

    public IEnumerator LaserPlay(Vector3 pointVector)
    {
        laserRunning = true;

        laser.transform.position = startingPos;
        laser.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - startingPos;
        float angulo = (Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg) + 90;

        //Setting the Spell Casting animation direction
        this.GetComponent<Animator>().SetBool("Attack", true);

        laser.gameObject.SetActive(true);

        GameManager.instance.sfxManager.PlayLaserBoss();

        laser.transform.rotation = Quaternion.Euler(0, 0, angulo);

        laserSR.size = new Vector2((float)8, Vector2.Distance(pointVector, startingPos) + 5.75f);
        laserLight.localScale = new Vector2(0.125f * (Vector2.Distance(pointVector, startingPos) + 5.75f), laserLight.localScale.y);
        laserSR.enabled = true;

        laserLight.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        laserLight.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        this.GetComponent<Animator>().SetBool("Attack", false);

        laserSR.enabled = false;
        laserLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;

        yield return new WaitForSeconds(0.05f);

        laserLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;

        laser.gameObject.SetActive(false);

        laserRunning = false;
    }

    public void StopLaser()
    {
        StopCoroutine(lastLaser);
    }
}
