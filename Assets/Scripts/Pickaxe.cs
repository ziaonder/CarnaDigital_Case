using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private int rotatingForward = 1;
    private float minAngle = -120f, maxAngle = 120f;
    private float time = 0f, timeToMove = 2f;

    private void Update()
    {
        time += Time.deltaTime;

        float currentZ = Mathf.Lerp(minAngle * rotatingForward, 
            maxAngle * rotatingForward, time / timeToMove);
        
        transform.rotation = Quaternion.Euler(0f, 0f, currentZ);

        if (time >= timeToMove)
        {
            time = 0f;
            rotatingForward = -rotatingForward;
        }
    }
}
