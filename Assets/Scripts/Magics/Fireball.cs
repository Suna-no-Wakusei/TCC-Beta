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

        fireball.transform.position = startingPos;
        fireball.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 diferencia = pointVector - fireball.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        float fireballSpeed = 7.5f;

        fireball.gameObject.SetActive(true);

        fireball.transform.rotation = Quaternion.Euler(0, 0, angulo);

        fireballRb.AddRelativeForce(fireball.transform.right * fireballSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);
        fireball.gameObject.SetActive(false);

        fireballRunning = false;
    }

    public void StopFireball()
    {
        StopCoroutine(lastFireball);
    }
}
