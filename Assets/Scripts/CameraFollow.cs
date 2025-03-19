using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offsetFromPlayer = new Vector3(0f, 4.15f, -10f);
    private Vector3 initialPos = new Vector3(0f, 5.55f, -5f);
    [SerializeField] private Transform player;

    private void OnEnable()
    {
        GameManager.OnRestart += Restart;
    }

    private void OnDisable()
    {
        GameManager.OnRestart -= Restart;
    }

    void Update()
    {
        transform.position = player.position + offsetFromPlayer;
    }

    private void Restart()
    {
        transform.position = initialPos;
    }
}
