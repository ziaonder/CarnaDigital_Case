using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float zSpeed = 10f;
    private bool isInAction = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position += new Vector3(0f, 0f, zSpeed) * Time.deltaTime;
        if (!isInAction && IsGrounded())
            HandleUserInput();
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
        rb.velocity = new Vector3(0f, 5f, 0f);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
