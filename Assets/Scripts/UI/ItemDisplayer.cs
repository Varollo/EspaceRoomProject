using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayer : MonoBehaviour
{
    [SerializeField] private UIShowHide _showHide;
    [SerializeField] private Image _displayImage;
    [SerializeField] private Text _displayText;
    [SerializeField] private AudioController _audioController;

    public void DisplayItem(InventoryItem item)
    {
        _audioController.Play("Item");
        _displayImage.sprite = item.ItemSprite;
        _displayText.text = item.Name;
        _showHide.Show();
    }
}
