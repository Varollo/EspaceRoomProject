using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowGameController : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _interactMaterial;
    [SerializeField] private Question[] _questions;
    [SerializeField] private InventoryItem _crowFeatherItem;

    [Header("References")]
    [SerializeField] private Text _questionText;
    [SerializeField] private Text[] _answersText;
    [SerializeField] private UIShowHide _panelShowHide;
    [SerializeField] private UIShowHide _questionShowHide;
    [SerializeField] private UIShowHide _answersShowHide;
    [SerializeField] private Collider _interactCollider;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private ItemDisplayer _itemDisplayer;
    [SerializeField] private UIShowHide _itemShowHide;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Renderer _crowRenderer;
    [SerializeField] private AudioController _audioController;

    private Material _originalMaterial;
    private Material _originalCrowMaterial;

    private int _questionCount;

    private void Start()
    {
        _originalMaterial = _renderer.material;
        _originalCrowMaterial = _crowRenderer.material;
    }

    private void ShowMessage(string msg)
    {
        _questionText.text = msg;
        _questionShowHide.Show();
    }

    public void SubmitAnswer(int index)
    {
        if (_questions[_questionCount].CorrectAnswer == index)
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    private void CorrectAnswer()
    {
        _audioController.Play("Right");
        StartCoroutine(ShowResponse(_questions[_questionCount].CorrectResponse, delegate ()
         {
             _questionCount++;

             if (_questionCount >= _questions.Length)
             {
                 EndGame();
             }

             ShowQuestion(_questions[_questionCount]);
         }));
    }

    private void WrongAnswer()
    {
        _audioController.Play("Wrong");
        StartCoroutine(ShowResponse(_questions[_questionCount].WrongResponse, delegate ()
        {
            _questionCount = 0;

            ShowQuestion(_questions[_questionCount]);
        }));
    }

    private void ShowQuestion(Question q)
    {
        _questionText.text = q.QuestionTxt;

        for (int i = 0; i < _answersText.Length; i++)
        {
            if (i >= q.AnswersTxt.Length)
            {
                _answersText[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                _answersText[i].transform.parent.gameObject.SetActive(true);
                _answersText[i].text = q.AnswersTxt[i];
            }
        }

        _questionShowHide.Show();
        _answersShowHide.Show();
    }

    private void EndGame()
    {
        _panelShowHide.Hide();
        _questionShowHide.Hide();
        _answersShowHide.Hide();

        _inventory.AddItem(_crowFeatherItem);
        _itemDisplayer.DisplayItem(_crowFeatherItem);

        _itemShowHide.OnHide.AddListener(OnCloseItemPopup);
    }

    private void OnCloseItemPopup()
    {
        PlayerMovement.CanMove = true;
        _playerMovement.MoveToCamera(2);
        _inventory.ShowHud();
        _itemShowHide.OnHide.RemoveListener(OnCloseItemPopup);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.9f);
        _panelShowHide.Show();
        ShowQuestion(_questions[_questionCount]);
    }

    private IEnumerator ShowResponse(string response, Action callback)
    {
        _questionShowHide.Hide();
        _answersShowHide.Hide();

        yield return new WaitForSeconds(0.5f);

        _audioController.Play("Crow");
        _questionText.text = response;
        _questionShowHide.Show();

        yield return new WaitForSeconds(3);

        _questionShowHide.Hide();

        yield return new WaitForSeconds(0.5f);

        callback?.Invoke();
    }

    public void OnInteract()
    {
        _interactCollider.enabled = false;
        _cameraController.ActiveCamera = "CameraCrow";
        PlayerMovement.CanMove = false;
        _inventory.HideHud();
        StartCoroutine(StartGame());
    }

    public void OnHighlight()
    {
        _renderer.material = _interactMaterial;
        _crowRenderer.material = _interactMaterial;
    }

    public void OnResetHighlight()
    {
        _renderer.material = _originalMaterial;
        _crowRenderer.material = _originalCrowMaterial;
    }
}
