using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float initialSpeed = 10f, currentSpeed, accelerationRate = 0.01f, maxSpeed = 15f;
    private float restrictionTime = 0f, elapsedTime;
    private bool isInAction = false, isMovementRestricted = true;
    private Rigidbody rb;
    public static event Action OnObstacleCrash, OnFall;
    private Vector3 initialPos = new Vector3(0f, 1.1f, 5f);
    private Vector3 touchBeginPos, touchEndPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

            if (transform.position.y < -2f)
            {
                rb.velocity = Vector3.zero;
                rb.useGravity = false;
                OnFall?.Invoke();
            }

            elapsedTime += Time.deltaTime;
            if(currentSpeed < maxSpeed)
                currentSpeed = Mathf.Min(initialSpeed * Mathf.Pow(1 + accelerationRate, elapsedTime), maxSpeed);
            transform.position += new Vector3(0f, 0f, currentSpeed) * Time.deltaTime;
            if (!isInAction && IsGrounded())
                HandleUserInput();
        }
    }

    private void HandleUserInput()
    {
        if(GameManager.Instance.isSwipeMechanicsEnabled)
            HandleSwipeInput();
        else
            HandleTapInput();
    }

    private void HandleSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                touchBeginPos = Input.GetTouch(0).position;
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // For better UX instead of calculating the swipe vector the moment touch
                // phase ended, we could calculate its magnitude every frame. The moment
                // the magnitude reaches a certain threshold, we could decide the action.
                // Better UX, but more CPU overhead. As we are more concerned about the
                // performance, I will stick with this implementation.
                touchEndPos = Input.GetTouch(0).position;
                Vector3 swipeVector = touchEndPos - touchBeginPos;
                if (swipeVector.magnitude > 50f)
                {
                    if (Mathf.Abs(swipeVector.y) > Mathf.Abs(swipeVector.x))
                        Jump();
                    else
                    {
                        if (swipeVector.x < 0)
                            StartCoroutine(Move(-1));
                        else
                            StartCoroutine(Move(1));
                    }
                }
                else
                    return;
            }
        }
    }

    // Tap Input divides the screen into 3 vertical parts. Left third is for moving
    // left, right third is for moving right, and the middle third is for jumping.
    private void HandleTapInput()
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

        // Do not let any movement beyond the platform.
        if(transform.position.x == -1.5f && direction == -1)
        {
            isInAction = false;
            yield break;
        }
        
        if(transform.position.x == 1.5f && direction == 1)
        {
            isInAction = false;
            yield break;
        }

        targetPosX = transform.position.x + (1.5f * direction);

        while (Mathf.Abs(transform.position.x - targetPosX) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(targetPosX, transform.position.y, transform.position.z),
                5f * Time.deltaTime
            );
            yield return null;
        }

        transform.position = new Vector3(targetPosX, transform.position.y, transform.position.z);
        isInAction = false;
    }

    private void Jump()
    {
        rb.velocity = new Vector3(0f, 6f, 0f);
    }

    // To decrease the CPU overhead, instead of casting rays every frame in order to
    // check if it's grounded, we could only cast if the player has jumped. So raycasting
    // would only be used to check if the player has landed. A local variable named isGrounded
    // will be set to true when Jump() method is called. The below method is called to
    // set it false. As I am due to time constraints, I will not implement this.
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void Restart()
    {
        transform.position = initialPos;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        isMovementRestricted = true;
        currentSpeed = initialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            OnObstacleCrash?.Invoke();
        }
    }
}
