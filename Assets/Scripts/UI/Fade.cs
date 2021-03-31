using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeSmooth;
    [SerializeField] private bool _startFaded;

    private float _idleAlpha;

    private void Awake()
    {
        _canvasGroup.alpha = _idleAlpha = _startFaded ? 0 : 1;
    }

    private void Update()
    {
        _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _idleAlpha, Time.deltaTime * _fadeSmooth);
    }

    public void FadeOut()
    {
        _idleAlpha = 1;
    }

    public void FadeIn()
    {
        _idleAlpha = 0;
    }
}
