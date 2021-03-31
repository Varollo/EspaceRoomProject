using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private FrogGameController.FrogColor _frogColor;
    [SerializeField] private Material _highlightMaterial;
    [Header("References")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private FrogGameController _frogGameController;

    private Material _originalMaterial;

    private void Start()
    {
        _originalMaterial = _renderer.material;
    }

    public void OnHighlight()
    {
        _renderer.material = _highlightMaterial;
    }

    public void OnInteract()
    {
        OnResetHighlight();
        _frogGameController.EnterAwnser(_frogColor);
    }

    public void OnResetHighlight()
    {
        _renderer.material = _originalMaterial;
    }
}
