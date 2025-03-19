using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State { Set, Play, GameOver }
    public State StateProperty { private set; get; } = State.Set;
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    private GameManager() 
    {
        instance = this;
    }
    [SerializeField] private GameObject panelSet, panelGameOver;
    public static event Action OnRestart;

    private void OnEnable()
    {
        PlayerMovement.OnObstacleCrash += GameOver;
    }

    private void OnDisable()
    {
        PlayerMovement.OnObstacleCrash -= GameOver;
    }

    private void Start()
    {
        Set();
    }

    private void Update()
    {
        if(StateProperty == State.Set && Input.touchCount > 0)
        {
            StateProperty = State.Play;
            panelSet.SetActive(false);
        }
    }

    private void Set()
    {
        panelGameOver.SetActive(false);
        panelSet.SetActive(true);
        StateProperty = State.Set;
    }

    private void GameOver()
    {
        panelGameOver.SetActive(true);
        StateProperty = State.GameOver;
    }

    public void Restart()
    {
        OnRestart?.Invoke();
        Set();
    }
}
