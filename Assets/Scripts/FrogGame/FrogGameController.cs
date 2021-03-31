using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogGameController : MonoBehaviour, IInteractable
{
    public enum FrogColor { None = 0, Red = 1, Green = 2, Blue = 3 }
    [Header("Settings")]
    [SerializeField] private int _startDifficulty = 2;
    [SerializeField] private int _rounds = 4;
    [SerializeField] private InventoryItem _frogEyeItem;
    [Header("Materials")]
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private Material _redGlowMaterial;
    [SerializeField] private Material _blueGlowMaterial;
    [SerializeField] private Material _greenGlowMaterial;
    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Renderer _rockRenderer;
    [SerializeField] private Renderer _redFrogRenrer;
    [SerializeField] private Renderer _greenFrogRenrer;
    [SerializeField] private Renderer _blueFrogRenrer;
    [SerializeField] private ItemDisplayer _itemDisplayer;
    [SerializeField] private UIShowHide _itemShowHide;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private AudioController _audioController;

    private int _currentRound;
    private List<FrogColor> _answerSequence;
    private List<FrogColor> _sequence;

    private Material _originalRockMaterial;
    private Material _originalRedFrogMaterial;
    private Material _originalGreenFrogMaterial;
    private Material _originalBlueFrogMaterial;

    private bool IsFullAwnser() => _answerSequence.Count >= _sequence.Count;
    private bool IsLastRound() => _currentRound >= _rounds;

    private void Start()
    {
        _originalRedFrogMaterial = _redFrogRenrer.material;
        _originalGreenFrogMaterial = _greenFrogRenrer.material;
        _originalBlueFrogMaterial = _blueFrogRenrer.material;
        _originalRockMaterial = _rockRenderer.material;
    }

    private void StartGame()
    {
        _currentRound = 0;
        _sequence = new List<FrogColor>();
        StartCoroutine(FrogGame(_startDifficulty));

        PlayerMovement.CanMove = false;
    }

    private FrogColor GetRandomFrogColor()
    {
        Array frogColorValues = Enum.GetValues(typeof(FrogColor));
        return (FrogColor)frogColorValues.GetValue(UnityEngine.Random.Range(1, frogColorValues.Length));
    }

    private Material GetMaterialForColor(FrogColor frogColor)
    {
        switch (frogColor)
        {
            case FrogColor.Red:
                _audioController.Play("Tone", 0.9f, true);
                return _redGlowMaterial;
            case FrogColor.Green:
                _audioController.Play("Tone", 1f, true);
                return _greenGlowMaterial;
            case FrogColor.Blue:
                _audioController.Play("Tone", 1.1f, true);
                return _blueGlowMaterial;
            default:
                return _originalRockMaterial;
        }
    }

    public void EnterAwnser(FrogColor frogColor)
    {
        if (_sequence == null) return;

        switch (frogColor)
        {
            case FrogColor.Red:
                _audioController.Play("Frog", 0.9f, true);
                break;
            case FrogColor.Green:
                _audioController.Play("Frog", 1f, true);
                break;
            case FrogColor.Blue:
                _audioController.Play("Frog", 1.1f, true);
                break;
        }

        _answerSequence.Add(frogColor);

        if (_sequence[_answerSequence.Count - 1] != frogColor)
        {
            WrongAnswer();
            return;
        }

        if (IsFullAwnser())
        {
            CorrectAnswer();
            return;
        }
    }

    private void WrongAnswer()
    {
        _audioController.Play("Wrong");
        StartGame();
    }

    private void CorrectAnswer()
    {
        if (IsLastRound())
        {
            EndGame();
            return;
        }

        _audioController.Play("Right");

        _currentRound++;

        StartCoroutine(FrogGame(1));
    }

    private void EndGame()
    {
        _sequence = null;

        _inventory.AddItem(_frogEyeItem);
        _itemDisplayer.DisplayItem(_frogEyeItem);

        _itemShowHide.OnHide.AddListener(OnCloseItemPopup);
    }

    private void OnCloseItemPopup()
    {
        _redFrogRenrer.transform.GetComponentInParent<Collider>().enabled = false;
        _greenFrogRenrer.transform.GetComponentInParent<Collider>().enabled = false;
        _blueFrogRenrer.transform.GetComponentInParent<Collider>().enabled = false;

        PlayerMovement.CanMove = true;
        _playerMovement.MoveToCamera(0);
        _inventory.ShowHud();

        _itemShowHide.OnHide.RemoveListener(OnCloseItemPopup);
    }

    private IEnumerator FrogGame(int newToSequence)
    {
        InteractableManager.CanInteract = false;

        _answerSequence = new List<FrogColor>();

        for (int i = 0; i < newToSequence; i++)
        {
            _sequence.Add(GetRandomFrogColor());
        }

        float t = (1 / (_currentRound + 1)) + 0.5f;

        for (int i = 0; i < _sequence.Count; i++)
        {
            _rockRenderer.material = GetMaterialForColor(FrogColor.None);

            yield return new WaitForSeconds(t);

            _rockRenderer.material = GetMaterialForColor(_sequence[i]);

            yield return new WaitForSeconds(t);
        }

        OnResetHighlight();

        InteractableManager.CanInteract = true;

        yield break;
    }

    public void OnInteract()
    {
        GetComponent<Collider>().enabled = false;
        _inventory.HideHud();
        _cameraController.ActiveCamera = "CameraFrog";
        StartGame();
    }

    public void OnHighlight()
    {
        _redFrogRenrer.material = _highlightMaterial;
        _greenFrogRenrer.material = _highlightMaterial;
        _blueFrogRenrer.material = _highlightMaterial;
        _rockRenderer.material = _highlightMaterial;
    }

    public void OnResetHighlight()
    {
        _redFrogRenrer.material = _originalRedFrogMaterial;
        _greenFrogRenrer.material = _originalGreenFrogMaterial;
        _blueFrogRenrer.material = _originalBlueFrogMaterial;
        _rockRenderer.material = _originalRockMaterial;
    }
}
