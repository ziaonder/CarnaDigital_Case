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
    [SerializeField] private GameObject panelSet, panelGameOver;
    public bool isSwipeMechanicsEnabled { private set; get; } = true;
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

    private void OnEnable()
    {
        PlayerMovement.OnObstacleCrash += GameOver;
        PlayerMovement.OnFall += GameOver;
        UIManager.OnRestart += Set;
        UIManager.OnPlay += Play;
    }

    private void OnDisable()
    {
        PlayerMovement.OnObstacleCrash -= GameOver;
        PlayerMovement.OnFall -= GameOver;
        UIManager.OnRestart -= Set;
        UIManager.OnPlay -= Play;
    }

    private void Start()
    {
        Set();
        // Some devices?(or Unity) limit it to 30fps on mobile by default.
        Application.targetFrameRate = 120;
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

    private void Play()
    {
        StateProperty = State.Play;
        panelSet.SetActive(false);
    }

    public void SetSwipeMechanic(bool value)
    {
        isSwipeMechanicsEnabled = value;
    }
}
