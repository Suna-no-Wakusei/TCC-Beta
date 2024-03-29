using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoneCannon : MonoBehaviour
{
    public bool stoneCannonRunning = false;

    public GameObject stoneCannon;
    public Rigidbody2D stoneCannonRb;
    private Vector3 startingPos;

    public Coroutine lastStoneCannon = null;
    public Animator animator;

    private void Start()
    {
        Rigidbody2D stoneCannonRb = stoneCannon.GetComponent<Rigidbody2D>();
    }

    public void PlayStoneCannon()
    {
        startingPos = GameObject.Find("Hero").transform.position;
        lastStoneCannon = StartCoroutine(StoneCannonPlay(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())));
    }

    public IEnumerator StoneCannonPlay(Vector3 pointVector)
    {
        stoneCannonRunning = true;
        GameManager.instance.hero.characterUnableToMove = true;

        GameManager.instance.sfxManager.PlayCastingEarth();

        //Getting the mouse direction
        stoneCannon.transform.position = startingPos;
        stoneCannon.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - stoneCannon.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        float stoneCannonSpeed = 7.5f;

        //Setting the Spell Casting animation direction
        GameManager.instance.hero.animator.SetFloat("SpellDirectionHorizontal", diferencia.x);
        GameManager.instance.hero.animator.SetFloat("SpellDirectionVertical", diferencia.y);

        if (GameManager.instance.playerMode == 0)
            GameManager.instance.hero.animator.SetBool("SpellCast", true);
        else
            GameManager.instance.hero.animator.SetBool("SpellAkemi", true);

        yield return null;

        if (GameManager.instance.playerMode == 0)
            GameManager.instance.hero.animator.SetBool("SpellCast", false);
        else
            GameManager.instance.hero.animator.SetBool("SpellAkemi", false);

        stoneCannon.gameObject.SetActive(true);

        stoneCannon.transform.rotation = Quaternion.Euler(0, 0, angulo);
        stoneCannon.transform.localPosition = stoneCannon.transform.right + new Vector3((float)0.2, 0, 0);

        yield return new WaitForSeconds(0.3f);

        GameManager.instance.hero.characterUnableToMove = false;

        animator.SetTrigger("Created");

        stoneCannonRb.AddForce(stoneCannon.transform.right * stoneCannonSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);

        animator.SetTrigger("Collide");

        GameManager.instance.sfxManager.PlayEarthImpact();

        yield return new WaitForSeconds(0.3f);

        stoneCannon.gameObject.SetActive(false);

        stoneCannonRunning = false;
    }

    public void StopStoneCannon()
    {
        if(lastStoneCannon != null)
        {
            GameManager.instance.sfxManager.PlayEarthImpact();
            StopCoroutine(lastStoneCannon);
        }
            
    }
}
