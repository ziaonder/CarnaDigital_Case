using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private Transform mainCam;
    private float timer = 0f;

    private void Awake()
    {
        mainCam = Camera.main.transform;
    }

    private void OnEnable()
    {
        UIManager.OnRestart += Restart;
    }

    private void OnDisable()
    {
        UIManager.OnRestart -= Restart;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Check every 1 second not every frame to reduce CPU overhead.
        if(timer >= 1f)
        {
            LookForVisibility();
            timer = 0f;
        }
    }

    // If it gets behind the camera, disable it to reuse it later.
    private void LookForVisibility()
    {
        if(transform.position.z < mainCam.position.z)
        {
            gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        gameObject.SetActive(false);
    }
}
