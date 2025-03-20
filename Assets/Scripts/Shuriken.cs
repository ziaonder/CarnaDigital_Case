using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private float rotationSpeed = 300f;
    // distance is the amount it can get away from the platform.
    private float distance = 5f;
    private int isMovingLeft = 1;
    private float time = 0f, timeToMove = 1f;

    void Update()
    {
        time += Time.deltaTime;
        if(time >= timeToMove)
        {
            isMovingLeft = -isMovingLeft;
            time = 0f;
        }

        float xPos = Mathf.Lerp(distance * isMovingLeft, 
            -distance * isMovingLeft, time / timeToMove);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
