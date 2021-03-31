using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _slots;
    [SerializeField] private UIShowHide _inventoryHudShowHide;

    public void ShowHud()
    {
        _inventoryHudShowHide.Show();
    }

    public void HideHud()
    {
        _inventoryHudShowHide.Hide();
    }

    public bool AddItem(InventoryItem item)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsEmpty)
            {
                slot.Item = item;
                return true;
            }
        }

        return false;
    }

    public bool Contains(InventoryItem item)
    {

        foreach (InventorySlot slot in _slots)
        {
            if (!slot.IsEmpty)
            {
                if (slot.Item.Compare(item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public InventoryItem RemoveItem(InventoryItem item)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (!slot.IsEmpty)
            {
                if (slot.Item.Compare(item))
                {
                    InventoryItem pop = slot.Item;
                    slot.Clear();
                    return pop;
                }
            }
        }

        return null;
    }
}