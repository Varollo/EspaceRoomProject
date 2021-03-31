using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _interactMaterial;
    [SerializeField] private UIShowHide _letterImage;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private AudioController _audioController;

    private Material _originalMaterial;

    private void Start()
    {
        _originalMaterial = _renderer.material;
    }

    public void ShowLetterImage()
    {
        _audioController.Play("Letter");
        InteractableManager.CanInteract = false;
        _inventory.HideHud();
        PlayerMovement.CanMove = false;
        _letterImage.Show();
    }

    public void HideLetterImage()
    {
        _audioController.Play("Confirm");
        InteractableManager.CanInteract = true;
        _inventory.ShowHud();
        PlayerMovement.CanMove = true;
        _letterImage.Hide();
    }

    public void OnInteract()
    {
        ShowLetterImage();
    }

    public void OnHighlight()
    {
        _renderer.material = _interactMaterial;
    }

    public void OnResetHighlight()
    {
        _renderer.material = _originalMaterial;
    }
}
