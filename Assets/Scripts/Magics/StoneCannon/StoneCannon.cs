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
        GameManager.instance.state = GameState.Paused;

        //Getting the mouse direction
        stoneCannon.transform.position = startingPos;
        stoneCannon.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - stoneCannon.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        float stoneCannonSpeed = 7.5f;

        //Setting the Spell Casting animation direction
        GameManager.instance.hero.animator.SetFloat("SpellDirectionHorizontal", diferencia.x);
        GameManager.instance.hero.animator.SetFloat("SpellDirectionVertical", diferencia.y);
        GameManager.instance.hero.animator.SetBool("SpellCast", true);

        yield return null;

        GameManager.instance.hero.animator.SetBool("SpellCast", false);

        stoneCannon.gameObject.SetActive(true);

        stoneCannon.transform.rotation = Quaternion.Euler(0, 0, angulo);
        stoneCannon.transform.localPosition = stoneCannon.transform.right + new Vector3((float)0.2, 0, 0);

        yield return new WaitForSeconds(0.3f);

        GameManager.instance.state = GameState.FreeRoam;

        animator.SetTrigger("Created");

        stoneCannonRb.AddForce(stoneCannon.transform.right * stoneCannonSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);

        animator.SetTrigger("Collide");

        yield return new WaitForSeconds(0.3f);

        stoneCannon.gameObject.SetActive(false);

        stoneCannonRunning = false;
    }

    public void StopStoneCannon()
    {
        if(lastStoneCannon != null)
            StopCoroutine(lastStoneCannon);
    }
}
