using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Zap : MonoBehaviour
{
    public bool zapRunning = false;

    public GameObject zap;
    public Transform zapLight;
    public SpriteRenderer zapSR;
    private Vector3 startingPos;

    public Coroutine lastZap = null;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        zapSR = zap.GetComponent<SpriteRenderer>();
    }

    public void PlayZap()
    {
        startingPos = GameObject.Find("Hero").transform.position;
        lastZap = StartCoroutine(ZapPlay(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    public IEnumerator ZapPlay(Vector3 pointVector)
    {
        zapRunning = true;
        GameManager.instance.state = GameState.Paused;

        zap.transform.position = startingPos;
        zap.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - zap.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;

        //Setting the Spell Casting animation direction
        GameManager.instance.hero.animator.SetFloat("SpellDirectionHorizontal", diferencia.x);
        GameManager.instance.hero.animator.SetFloat("SpellDirectionVertical", diferencia.y);
        GameManager.instance.hero.animator.SetBool("SpellCast", true);

        yield return null;

        GameManager.instance.hero.animator.SetBool("SpellCast", false);

        zapSR.enabled = false;
        zapLight.GetComponent<Light2D>().enabled = false;
        zap.gameObject.SetActive(true);

        zap.transform.rotation = Quaternion.Euler(0, 0, angulo);
        zap.transform.localPosition = zap.transform.right + new Vector3((float)0.2, 0, 0);

        zapSR.size = new Vector2(Vector2.Distance(pointVector, GameManager.instance.hero.transform.position), (float)0.6875);
        zapLight.localScale = new Vector2(0.3f * Vector2.Distance(pointVector, GameManager.instance.hero.transform.position), zapLight.localScale.y);

        yield return new WaitForSeconds(0.05f);

        zapSR.enabled = true;
        zapLight.GetComponent<Light2D>().enabled = true;

        yield return new WaitForSeconds(0.3f);
        zap.gameObject.SetActive(false);

        GameManager.instance.state = GameState.FreeRoam;

        yield return new WaitForSeconds(1f);

        zapRunning = false;
    }

    public void StopZap()
    {
        StopCoroutine(lastZap);
    }
}
