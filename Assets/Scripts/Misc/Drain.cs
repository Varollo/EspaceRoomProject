using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _interactMaterial;
    [SerializeField] private InventoryItem _requiredItem;
    [SerializeField] private InventoryItem _keyItem;
    [Header("References")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private MessageController _messageController;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private ItemDisplayer _itemDisplayer;
    [SerializeField] private UIShowHide _itemShowHide;
    [SerializeField] private Collider _collider;

    private Material _originalMaterial;

    private void Start()
    {
        _originalMaterial = _renderer.material;
    }

    private void OnCloseItemPopup()
    {
        PlayerMovement.CanMove = true;
        _collider.enabled = false;
        _playerMovement.MoveToCamera(3);
        _inventory.ShowHud();
        _itemShowHide.OnHide.RemoveListener(OnCloseItemPopup);
    }

    public void OnInteract()
    {
        PlayerMovement.CanMove = false;
        _collider.enabled = false;
        _cameraController.ActiveCamera = "CameraDrain";
        _inventory.HideHud();

        if (_inventory.Contains(_requiredItem))
        {
            _messageController.ShowMessage("I can use the potion to get the key!", delegate ()
            {
                _inventory.RemoveItem(_requiredItem);
                _inventory.AddItem(_keyItem);

                _itemDisplayer.DisplayItem(_keyItem);
                _itemShowHide.OnHide.AddListener(OnCloseItemPopup);
            });
        }
        else
        {
            _messageController.ShowMessage("The key is down here... If you could just lift it somehow...", delegate ()
            {
                _inventory.ShowHud();
                PlayerMovement.CanMove = true;
                _playerMovement.MoveToCamera(3);
                _collider.enabled = true;
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
