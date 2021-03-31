using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    [SerializeField] private UIShowHide _showHide;
    [SerializeField] private Text _text;
    [SerializeField] private AudioController _audioController;

    private Action _onCloseCallback;

    public void CloseMessage()
    {
        _audioController.Play("Confirm");
        _showHide.Hide();
        _onCloseCallback?.Invoke();
        _onCloseCallback = null;
    }

    public void ShowMessage(string text, Action onCloseMessageCallback = null)
    {
        _audioController.Play("Message");
        _text.text = text;
        _showHide.Show();
        _onCloseCallback = onCloseMessageCallback;
    }
}
