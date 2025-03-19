using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float zSpeed = 10f, restrictionTime = 0f;
    private bool isInAction = false;
    private Rigidbody rb;
    public static event Action OnObstacleCrash;
    private bool isMovementRestricted = true;
    private Vector3 initialPos = new Vector3(0f, 1.1f, 5f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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
        if (GameManager.Instance.StateProperty == GameManager.State.Play)
        {
            // This is to prevent the player from moving while the game is starting.
            if (isMovementRestricted)
            {
                restrictionTime += Time.deltaTime;
                if (restrictionTime >= .2f)
                {
                    isMovementRestricted = false;
                    restrictionTime = 0f;
                }
                else
                    return;
            }

            transform.position += new Vector3(0f, 0f, zSpeed) * Time.deltaTime;
            if (!isInAction && IsGrounded())
                HandleUserInput();
        }
    }

    private void HandleUserInput()
    {
        if (Input.touchCount <= 0)
            return;

        if (Input.GetTouch(0).position.x < Screen.width / 3)
            StartCoroutine(Move(-1));
        else if (Input.GetTouch(0).position.x > Screen.width / 3 * 2)
            StartCoroutine(Move(1));
        else
            Jump();
    }

    private IEnumerator Move(int direction)
    {
        isInAction = true;
        float targetPosX;
        if(transform.position.x < .1f && transform.position.x > -.1f)
            targetPosX = (1.5f * direction);
        else
            targetPosX = transform.position.x + (1.5f * direction);

        if (targetPosX < -1.5f || targetPosX > 1.5f)
        {
            isInAction = false;
            yield break;
        }

        while (Mathf.Abs(transform.position.x - targetPosX) > 0.01f)
        {
            transform.position += new Vector3(5f * direction, 0f, 0f) * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetPosX, transform.position.y, transform.position.z);
        isInAction = false;
    }

    private void Jump()
    {
        rb.velocity = new Vector3(0f, 6f, 0f);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void Restart()
    {
        transform.position = initialPos;
        rb.velocity = Vector3.zero;
        isMovementRestricted = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            OnObstacleCrash?.Invoke();
        }
    }
}
