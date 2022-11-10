using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool laserRunning = false;

    public GameObject laser;
    public Transform laserLight;
    public SpriteRenderer laserSR;
    private Vector3 startingPos;

    public Coroutine lastLaser = null;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        laserSR = laser.GetComponent<SpriteRenderer>();
    }

    public void PlayLaser()
    {
        startingPos = new Vector2(7.56f, -17.36f);
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

        yield return new WaitForSeconds(1f);

        this.GetComponent<Animator>().SetBool("Attack", false);

        laserSR.enabled = false;
        laserLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        laser.gameObject.SetActive(true);

        laser.transform.rotation = Quaternion.Euler(0, 0, angulo);

        laserSR.size = new Vector2((float)8, Vector2.Distance(pointVector, startingPos) + 5.75f);
        laserLight.localScale = new Vector2(0.1f * (Vector2.Distance(pointVector, startingPos) + 5.75f), laserLight.localScale.y);

        yield return new WaitForSeconds(0.05f);

        laserSR.enabled = true;
        laserLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;

        yield return new WaitForSeconds(0.578f);
        laser.gameObject.SetActive(false);

        laserRunning = false;
    }

    public void StopLaser()
    {
        StopCoroutine(lastLaser);
    }
}
