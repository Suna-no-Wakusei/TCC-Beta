using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public bool fireballRunning = false;

    public GameObject fireball;
    public Rigidbody2D fireballRb;
    private Vector3 startingPos;

    public Coroutine lastFireball = null;
    public Animator animator;

    private void Start()
    {
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
    }

    public void PlayFireball()
    {
        startingPos = GameObject.Find("Hero").transform.position;
        lastFireball = StartCoroutine(FireballPlay(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    public IEnumerator FireballPlay(Vector3 pointVector)
    {
        fireballRunning = true;
        GameManager.instance.state = GameState.Paused;

        //Getting the mouse direction
        fireball.transform.position = startingPos;
        fireball.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - fireball.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        float fireballSpeed = 7.5f;

        //Setting the Spell Casting animation direction
        GameManager.instance.hero.animator.SetFloat("SpellDirectionHorizontal", diferencia.x);
        GameManager.instance.hero.animator.SetFloat("SpellDirectionVertical", diferencia.y);
        GameManager.instance.hero.animator.SetBool("SpellCast", true);

        yield return null;

        GameManager.instance.hero.animator.SetBool("SpellCast", false);

        fireball.gameObject.SetActive(true);

        fireball.transform.rotation = Quaternion.Euler(0, 0, angulo);
        fireball.transform.localPosition = fireball.transform.right + new Vector3((float)0.2, 0, 0);

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.state = GameState.FreeRoam;

        animator.SetTrigger("Created");

        fireballRb.AddForce(fireball.transform.right * fireballSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);

        animator.SetTrigger("Collide");

        fireball.gameObject.SetActive(false);
        
        fireballRunning = false;
    }

    public void StopFireball()
    {
        StopCoroutine(lastFireball);
    }
}
