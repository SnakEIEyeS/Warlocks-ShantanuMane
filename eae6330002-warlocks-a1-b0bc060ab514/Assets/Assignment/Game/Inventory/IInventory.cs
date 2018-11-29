using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    IUnit InventoryOwner { get; }
    List<Item> ItemList { get; }
    int InventoryCapacity { get; }
}
