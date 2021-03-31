using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _interactMaterial;
    [Header("References")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItem _requiredItem;
    [SerializeField] private MessageController _messageController;
    [SerializeField] private GameController _gameController;


    private Material _originalMaterial;

    private void Start()
    {
        _originalMaterial = _renderer.material;
    }

    public void OnInteract()
    {
        PlayerMovement.CanMove = false;
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        _inventory.HideHud();

        if (_inventory.Contains(_requiredItem))
        {
            _messageController.ShowMessage("This is it! you are finally out!", delegate ()
            {
                _inventory.RemoveItem(_requiredItem);
                _gameController.WinGame();
            });
        }
        else
        {
            _messageController.ShowMessage("It's locked.\nMaybe you dropped the key somewhere?", delegate ()
            {
                PlayerMovement.CanMove = true;
                col.enabled = true;
                _inventory.ShowHud();
            });
        }
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
