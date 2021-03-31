using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    public static bool CanInteract { get; set; }

    private IInteractable _currentSelected;

    private void Start()
    {
        CanInteract = false;
    }

    private void Update()
    {
        if (!CanInteract) return;

        if (_currentSelected != null)
        {
            _currentSelected.OnResetHighlight();

            if (Input.GetMouseButtonDown(0))
            {
                _currentSelected.OnInteract();
            }
        }

        CheckInteractable();
    }

    private void CheckInteractable()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();

            if (interactable != null)
            {
                _currentSelected = interactable;
                interactable.OnHighlight();
                return;
            }
        }

        _currentSelected = null;
    }
}
