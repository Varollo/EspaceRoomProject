using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string Name;
    public Sprite ItemSprite;

    public bool Compare(InventoryItem item) => Name == item.Name;
}
