using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform[] platforms;
    private Transform mainCam;
    private int currentIndex = 0, nextIndex = 0;
    private float platformLength;

    private void Start()
    {
        platformLength = platforms[0].localScale.z;
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        nextIndex = ((int)mainCam.position.z + 8) / (int)platformLength;

        if(currentIndex != nextIndex)
        {
            MovePlatform(currentIndex);
            currentIndex = nextIndex;
        }
    }

    private void MovePlatform(int index)
    {
        index = index % platforms.Length;
        int lastIndex = (index - 1 + platforms.Length) % platforms.Length;
        Vector3 lastPlatformPos = platforms[lastIndex].position;
        platforms[index].position = 
            new Vector3(0f, 0f, lastPlatformPos.z + platformLength);
    }
}
