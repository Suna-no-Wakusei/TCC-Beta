using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IceShard : MonoBehaviour
{
    public bool iceShardRunning = false;

    public GameObject iceShard;
    public Rigidbody2D iceShardRb;
    private Vector3 startingPos;

    public Coroutine lastIceShard = null;
    public Animator animator;

    private void Start()
    {
        Rigidbody2D iceShardRb = iceShard.GetComponent<Rigidbody2D>();
    }

    public void PlayIceShard()
    {
        startingPos = GameObject.Find("Hero").transform.position;
        lastIceShard = StartCoroutine(IceShardPlay(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())));
    }

    public IEnumerator IceShardPlay(Vector3 pointVector)
    {
        iceShardRunning = true;
        GameManager.instance.hero.characterUnableToMove = true;

        GameManager.instance.sfxManager.PlayCastingIce();

        //Getting the mouse direction
        iceShard.transform.position = startingPos;
        iceShard.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - iceShard.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        float iceShardSpeed = 7.5f;

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

        iceShard.gameObject.SetActive(true);

        iceShard.transform.rotation = Quaternion.Euler(0, 0, angulo);
        iceShard.transform.localPosition = iceShard.transform.right + new Vector3((float)0.2, 0, 0);

        yield return new WaitForSeconds(0.3f);

        GameManager.instance.hero.characterUnableToMove = false;

        animator.SetTrigger("Created");

        iceShardRb.AddForce(iceShard.transform.right * iceShardSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);

        animator.SetTrigger("Collide");

        GameManager.instance.sfxManager.PlayIceImpact();

        yield return new WaitForSeconds(0.3f);

        iceShard.gameObject.SetActive(false);

        iceShardRunning = false;
    }

    public void StopIceShard()
    {
        if(lastIceShard != null)
        {
            GameManager.instance.sfxManager.PlayIceImpact();
            StopCoroutine(lastIceShard);
        } 
    }
}
