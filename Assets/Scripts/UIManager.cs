using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static event Action OnRestart, OnPlay;
    [SerializeField] private Slider slider;

    public void OnRestartPressed()
    {
        OnRestart?.Invoke();
    }

    public void OnPlayPressed()
    {
        OnPlay?.Invoke();
    }

    public void OnSliderValueChanged()
    {
        // 0 means swipe mechanics are enabled, 1 means they are disabled.
        GameManager.Instance.SetSwipeMechanic(slider.value == 0);
    }
}
