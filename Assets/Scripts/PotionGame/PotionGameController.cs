using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionGameController : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private float _sliderScaler = 0.1f;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private InventoryItem _levitationPotionItem;
    [SerializeField] private InventoryItem _requiredItem1;
    [SerializeField] private InventoryItem _requiredItem2;
    [SerializeField] private InventoryItem _requiredItem3;
    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private ItemDisplayer _itemDisplayer;
    [SerializeField] private UIShowHide _itemShowHide;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private MessageController _messageController;
    [SerializeField] private UIShowHide _uiShowHide;
    [SerializeField] private Slider _redSlider;
    [SerializeField] private Slider _greenSlider;
    [SerializeField] private Slider _blueSlider;
    [SerializeField] private Image _arrowImage;
    [SerializeField] private Image _potionImage;
    [SerializeField] private AudioController _audioController;

    private Material _originalMaterial;

    private bool HasRequiredItems() => _inventory.Contains(_requiredItem1) && _inventory.Contains(_requiredItem2) && _inventory.Contains(_requiredItem3);

    private void Start()
    {
        _originalMaterial = _renderer.material;
    }

    private void StartGame()
    {
        RandomizeSliders();
        _uiShowHide.Show();
    }

    /// <summary>
    /// +red = -blue
    /// +green = -red -blue
    /// +blue = -green
    /// </summary>
    /// <param name="r">red value</param>
    /// <param name="g">green value</param>
    /// <param name="b">blue value</param>
    private void ChangeSliderValues(float r, float g, float b)
    {
        float rChange = (r - g) * _sliderScaler;
        float gChange = (g - b) * _sliderScaler;
        float bChange = (b - r - g) * _sliderScaler;

        _redSlider.value += rChange;
        _greenSlider.value += gChange;
        _blueSlider.value += bChange;

        float stdDev = GetStandardDeviation(_redSlider.value, _greenSlider.value, _blueSlider.value);

        _arrowImage.fillAmount = Map(stdDev, 0.408f, 0f, 0f, 1f);

        if (stdDev <= 0.001f)
        {
            EndGame();
        }
    }

    public void ChangeRedValue()
    {
        _audioController.Play("Bubbles", 0.9f, true);
        ChangeSliderValues(1, 0, 0);
    }

    public void ChangeGreenValue()
    {
        _audioController.Play("Bubbles", 1f, true);
        ChangeSliderValues(0, 1, 0);
    }

    public void ChangeBlueValue()
    {
        _audioController.Play("Bubbles", 1.1f, true);
        ChangeSliderValues(0, 0, 1);
    }

    private void EndGame()
    {
        _uiShowHide.Hide();

        _inventory.RemoveItem(_requiredItem1);
        _inventory.RemoveItem(_requiredItem2);
        _inventory.RemoveItem(_requiredItem3);

        _inventory.AddItem(_levitationPotionItem);
        _itemDisplayer.DisplayItem(_levitationPotionItem);

        _itemShowHide.OnHide.AddListener(OnCloseItemPopup);
    }

    private void OnCloseItemPopup()
    {
        PlayerMovement.CanMove = true;
        _playerMovement.MoveToCamera(1);
        _inventory.ShowHud();
        _itemShowHide.OnHide.RemoveListener(OnCloseItemPopup);
    }

    /// <summary>
    /// Standard deviation is how close the values are together ( equal = 0 )
    /// </summary>
    /// <returns></returns>
    private float GetStandardDeviation(float r, float g, float b)
    {
        // calculate the mean of the values = (v1 + v2 + v3) / n - 1
        float mean = (r + g + b) / 3f;

        // calculate the deviation for each value = (v-mean)^2
        float rDev = (r - mean) * (r - mean);
        float gDev = (g - mean) * (g - mean);
        float bDev = (b - mean) * (b - mean);

        // get the mean of the deviations
        float variance = (rDev + gDev + bDev) / 3f;

        // take the square root of the variance to get the standard deviation
        float stdDev = Mathf.Sqrt(variance);

        return stdDev;
    }

    /// <summary>
    /// Maps a value from one range to another
    /// </summary>
    private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    /// <summary>
    /// Randomizes the sliders with values from 0 to 1 with only 1 decimal case (0.1, 0.2, etc)
    /// </summary>
    private void RandomizeSliders()
    {
        _redSlider.value = Mathf.Floor(Random.value * 10) / 10;
        _greenSlider.value = Mathf.Floor(Random.value * 10) / 10;
        _blueSlider.value = Mathf.Floor(Random.value * 10) / 10;
    }

    public void OnInteract()
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        _cameraController.ActiveCamera = "CameraCauldron";
        PlayerMovement.CanMove = false;
        _inventory.HideHud();

        if (HasRequiredItems())
        {
            StartGame();
        }
        else
        {
            _messageController.ShowMessage("You don't have the itens requiered.", delegate ()
            {
                _inventory.ShowHud();
                col.enabled = true;
                PlayerMovement.CanMove = true;
                _playerMovement.MoveToCamera(1);
            });
        }
    }

    public void OnHighlight()
    {
        _renderer.material = _highlightMaterial;
    }

    public void OnResetHighlight()
    {
        _renderer.material = _originalMaterial;
    }
}
