using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private static Timer instance;
    public static Timer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Timer>();
                if (instance == null)
                {
                    instance = new GameObject("Timer").AddComponent<Timer>();
                }
            }
            return instance;
        }
    }
    private Timer()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public float TimeElapsed { private set; get; } = 0f;
    private TextMeshProUGUI timeText;
    [SerializeField] private Vector3 offset;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        timeText = GetComponent<TextMeshProUGUI>();
    }

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
        Vector3 pos = new Vector3(rectTransform.position.x, Screen.safeArea.yMax) + offset;
        rectTransform.position = pos;
    }

    void Update()
    {
        if(GameManager.Instance.StateProperty == GameManager.State.Play)
        {
            TimeElapsed += Time.deltaTime;
            timeText.text = TimeElapsed.ToString("F0");
        }
    }

    private void Restart()
    {
        TimeElapsed = 0f;
        timeText.text = TimeElapsed.ToString("F0");
    }
}
