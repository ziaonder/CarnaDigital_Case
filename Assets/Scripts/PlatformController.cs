using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform[] platforms;
    private Transform mainCam;
    private int currentIndex = 0, nextIndex = 0, hasEmptySpace = 0;
    private float platformLength, emptySpace = 8f, platformZPos = 0f;
    public static event Action<float> OnPlatformMoved;
    private Vector3 platformInitialPos;

    private void OnEnable()
    {
        UIManager.OnRestart += Restart;
    }

    private void OnDisable()
    {
        UIManager.OnRestart -= Restart;
    }

    private void Start()
    {
        platformLength = platforms[0].localScale.z;
        platformInitialPos = new Vector3(0f, 0f, platformLength / 2);
        mainCam = Camera.main.transform;
        SetPlatformsInitially();
    }

    private void Update()
    {
        // platformZPos holds the platform's beginning position. The said platform is the one
        // camera has lost the sight of. +8 is just an adjustment to make it more precise.
        // Camera and player has 10 z-distance in between.

        if (mainCam.position.z + 8f > platformZPos + platformLength + 
            hasEmptySpace * emptySpace)
        {
            nextIndex++;
            platformZPos += platformLength + hasEmptySpace * emptySpace;
        }

        if (currentIndex != nextIndex)
        {
            MovePlatform(currentIndex);
            currentIndex = nextIndex;
        }
    }

    private void MovePlatform(int index)
    {
        index = index % platforms.Length;
        hasEmptySpace = UnityEngine.Random.Range(0, 2);
        int lastIndex = (index - 1 + platforms.Length) % platforms.Length;
        Vector3 lastPlatformPos = platforms[lastIndex].position;
        platforms[index].position = new Vector3(0f, 0f, 
            lastPlatformPos.z + platformLength + emptySpace * hasEmptySpace);

        // Division by 2 is to get the beginning of the platform.
        OnPlatformMoved?.Invoke(lastPlatformPos.z + platformLength + 
            emptySpace * hasEmptySpace);
    }

    private void Restart()
    {
        currentIndex = 0;
        nextIndex = 0;
        platformZPos = 0f;
        hasEmptySpace = 0;

        SetPlatformsInitially();
    }

    private void SetPlatformsInitially()
    {
        for(int i = 0; i < platforms.Length; i++)
        {
            platforms[i].position = platformInitialPos +
                new Vector3(0f, 0f, platformLength) * i;

            if(i > 0)
            {
                OnPlatformMoved?.Invoke(platforms[i].position.z);
            }
        }
    }
}
