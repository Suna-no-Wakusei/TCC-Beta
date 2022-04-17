using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.4f;
    public float boundY = 0.2f;

    public Vector2 Min, Max;

    private void Start()
    {
        lookAt = GameObject.Find("Hero").transform;
    }

    void LateUpdate()
    {

        Vector3 delta = Vector3.zero;

        float deltaX = Mathf.Clamp(lookAt.position.x, Min.x, Max.x) - transform.position.x;
        if(deltaX > boundX || deltaX < -boundX) 
        {
            if (transform.position.x < Mathf.Clamp(lookAt.position.x, Min.x, Max.x))
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }

        float deltaY = Mathf.Clamp(lookAt.position.y, Min.y, Max.y) - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < Mathf.Clamp(lookAt.position.y, Min.y, Max.y))
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(
            delta.x,
            delta.y,
            0
            );

    }
}
