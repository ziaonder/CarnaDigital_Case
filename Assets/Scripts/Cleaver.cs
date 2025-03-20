using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaver : MonoBehaviour
{
    private float timeToStrike = .5f, timeToDraw = 1f;
    private float time = 0f;
    private bool isStriking = true;

    private void Start()
    {
        if(transform.position.x > 0)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (isStriking)
        {
            float currentZ = Mathf.Lerp(0f, 90f, time / timeToStrike);
            transform.rotation = Quaternion.Euler(0f, 
                transform.rotation.eulerAngles.y, currentZ);
        
            if(time >= timeToStrike)
            {
                time = 0f;
                isStriking = false;
            }
        }
        else
        {
            float currentZ = Mathf.Lerp(90f, 0f, time / timeToDraw);
            transform.rotation = Quaternion.Euler(0f, 
                transform.rotation.eulerAngles.y, currentZ);

            if (time >= timeToDraw)
            {
                time = 0f;
                isStriking = true;
            }
        }

    }
}
