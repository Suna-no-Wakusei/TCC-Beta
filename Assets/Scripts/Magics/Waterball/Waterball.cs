using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterball : MonoBehaviour
{
    public bool waterballRunning = false;

    public GameObject waterball;
    public Rigidbody2D waterballRb;
    private Vector3 startingPos;

    public Coroutine lastWaterball = null;
    public Animator animator;

    private void Start()
    {
        Rigidbody2D waterballRb = waterball.GetComponent<Rigidbody2D>();
    }

    public void PlayWaterball()
    {
        startingPos = GameObject.Find("Hero").transform.position;
        lastWaterball = StartCoroutine(WaterballPlay(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    public IEnumerator WaterballPlay(Vector3 pointVector)
    {
        waterballRunning = true;
        GameManager.instance.state = GameState.Paused;

        //Getting the mouse direction
        waterball.transform.position = startingPos;
        waterball.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - waterball.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        float waterballSpeed = 7.5f;

        //Setting the Spell Casting animation direction
        GameManager.instance.hero.animator.SetFloat("SpellDirectionHorizontal", diferencia.x);
        GameManager.instance.hero.animator.SetFloat("SpellDirectionVertical", diferencia.y);
        GameManager.instance.hero.animator.SetBool("SpellCast", true);

        yield return null;

        GameManager.instance.hero.animator.SetBool("SpellCast", false);

        waterball.gameObject.SetActive(true);

        waterball.transform.rotation = Quaternion.Euler(0, 0, angulo);
        waterball.transform.localPosition = waterball.transform.right + new Vector3((float)0.2, 0, 0);

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.state = GameState.FreeRoam;

        animator.SetTrigger("Created");

        waterballRb.AddForce(waterball.transform.right * waterballSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);

        animator.SetTrigger("Collide");

        yield return new WaitForSeconds(0.3f);

        waterball.gameObject.SetActive(false);

        waterballRunning = false;
    }

    public void StopWaterball()
    {
        if(lastWaterball != null)
            StopCoroutine(lastWaterball);
    }
}
