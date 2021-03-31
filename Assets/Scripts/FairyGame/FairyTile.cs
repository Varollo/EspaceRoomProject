using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyTile : MonoBehaviour, IInteractable
{
    [SerializeField] private float _snapSpeed = 10;
    [SerializeField] private float _offset;
    [SerializeField] private Material _highlightMaterial;
    [Header("References")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private FairyGameController _fairyGameController;
    [SerializeField] private AudioController _audioController;

    private Material _originalMaterial;

    private Vector3 _idlePosition;

    private void Start()
    {
        _originalMaterial = _renderer.material;
        _idlePosition = transform.localPosition;
    }

    private void Update()
    {
        SnapToIdlePosition();
    }

    private void SnapToIdlePosition()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _idlePosition, _snapSpeed * Time.deltaTime);
    }

    public void Move(Vector3 direction)
    {
        _idlePosition += direction * _offset;
    }

    public void OnHighlight()
    {
        _renderer.material = _highlightMaterial;
    }

    public void OnInteract()
    {
        _audioController.Play("Piece");
        _fairyGameController.MoveTile(this);
        OnResetHighlight();
    }

    public void OnResetHighlight()
    {
        _renderer.material = _originalMaterial;
    }
}
