using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _startTimeInSeconds;
    [SerializeField] private Text _timerText;
    [SerializeField] private float _animationScaler;
    [SerializeField] private GameController _gameController;
    [SerializeField] private UIShowHide _timerShowHide;

    public float TimeLeftInSeconds { get => _timeLeftInSeconds; private set => _timeLeftInSeconds = value; }

    private float _timeLeftInSeconds;

    public bool Stop { get; set; }

    private void Start()
    {
        TimeLeftInSeconds = _startTimeInSeconds;
        Stop = true;
    }

    public void StartTimer()
    {
        _timerShowHide.Show();
        Stop = false;
    }

    private void Update()
    {
        if (!Stop)
        {
            TickTimer();
        }

        UpdateUI();
    }

    private void TickTimer()
    {
        TimeLeftInSeconds -= Time.deltaTime;

        if (TimeLeftInSeconds <= 0)
        {
            TimeLeftInSeconds = 0;
            OnEndTimer();
            Stop = true;
        }
    }

    private void UpdateUI()
    {
        TimeSpan t = TimeSpan.FromSeconds(TimeLeftInSeconds);
        _timerText.text = $"{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}:{(t.Milliseconds / 10).ToString("00")}";

        AnimateTextAfterHalfway();
    }

    /// <summary>
    /// Makes the text red and animates it if you go pass the halfway point
    /// </summary>
    private void AnimateTextAfterHalfway()
    {
        if (TimeLeftInSeconds <= _startTimeInSeconds / 2f)
        {
            float mappedTime = TimeLeftInSeconds / (_startTimeInSeconds / 2f); // maps time from 0 to 1

            AnimateScaleWithSineWave();

            AddTint(mappedTime);
        }
    }

    private void AnimateScaleWithSineWave()
    {
        _timerText.transform.localScale = Vector3.one * (Mathf.Sin((TimeLeftInSeconds + Mathf.PI * 1.5f) * _animationScaler) + 7f) / 8f;
    }

    private void AddTint(float mappedTime)
    {
        Color c = Color.white;
        c.g = c.b = mappedTime;
        _timerText.color = c;
    }

    private void OnEndTimer()
    {
        _gameController.LoseGame();
    }

    /// <summary>
    /// Maps a value from one range to another
    /// </summary>
    private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}
