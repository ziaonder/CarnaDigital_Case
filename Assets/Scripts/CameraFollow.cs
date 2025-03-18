using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offsetFromPlayer = new Vector3(0f, 4.15f, -10f);
    [SerializeField] private Transform player;

    void Update()
    {
        transform.position = player.position + offsetFromPlayer;
    }
}
