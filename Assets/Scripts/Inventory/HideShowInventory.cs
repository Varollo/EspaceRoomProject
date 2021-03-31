using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowInventory : MonoBehaviour
{
    [SerializeField] private RectTransform _inventoryUI;
    [SerializeField] private float _snapSpeed;

    private Vector2 _idlePosition;
    private Vector2 _onScreenPosition;
    private Vector2 _offScreenPosition;

    private void Start()
    {
        _offScreenPosition = _inventoryUI.anchoredPosition;
        _onScreenPosition = Vector2.zero;
        _idlePosition = _offScreenPosition;
    }

    private void Update()
    {
        SnapToPosition();
    }

    private void SnapToPosition()
    {
        _inventoryUI.anchoredPosition = Vector2.Lerp(_inventoryUI.anchoredPosition, _idlePosition, _snapSpeed);
    }

    public void Toggle()
    {
        if (_idlePosition == _onScreenPosition)
        {
            _idlePosition = _offScreenPosition;
        }
        else
        {
            _idlePosition = _onScreenPosition;
        }
    }

    public void Show()
    {
        _idlePosition = _onScreenPosition;
    }

    public void Hide()
    {
        _idlePosition = _offScreenPosition;
    }
}
