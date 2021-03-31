using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventBool : UnityEvent<bool> { }

public class UIShowHide : MonoBehaviour
{
    [SerializeField] private float _snapSpeed = 10;
    [SerializeField] private bool _startOffScreen;
    [Space]
    [SerializeField] private Vector2 _onScreenPosition;
    [SerializeField] private Vector2 _offScreenPosition;
    [Space]

    public UnityEvent OnShow = new UnityEvent();
    public UnityEvent OnHide = new UnityEvent();
    public UnityEventBool OnToggle = new UnityEventBool();

    private Vector2 _idlePosition;

    private RectTransform _transform;

    private void Start()
    {
        _transform = (RectTransform)transform;
        _idlePosition = _startOffScreen ? _offScreenPosition : _onScreenPosition;
    }

    private void Update()
    {
        SnapToPosition();
    }

    private void SnapToPosition()
    {
        _transform.anchoredPosition = Vector2.Lerp(_transform.anchoredPosition, _idlePosition, _snapSpeed * Time.deltaTime);
    }

    public void Toggle()
    {
        bool isOnScreen = _idlePosition == _onScreenPosition;

        _idlePosition = isOnScreen ? _offScreenPosition : _onScreenPosition;

        OnToggle?.Invoke(!isOnScreen);
    }

    public void Show()
    {
        _idlePosition = _onScreenPosition;

        OnShow?.Invoke();
    }

    public void Hide()
    {
        _idlePosition = _offScreenPosition;
        OnHide?.Invoke();
    }
}
