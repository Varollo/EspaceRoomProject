using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;

    public InventoryItem Item
    {
        get => _item;
        set
        {
            if (value == null)
            {
                _itemImage.sprite = null;
                _itemImage.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                _itemImage.sprite = value.ItemSprite;
                _itemImage.color = new Color(1f, 1f, 1f, 1f);
            }

            _item = value;
        }
    }

    private InventoryItem _item;

    public void Clear() => Item = null;
    public bool IsEmpty => Item == null;

    public InventoryItem PopItem()
    {
        InventoryItem item = Item;
        Item = null;
        return item;
    }
}
