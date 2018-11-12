using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour, IInventory
{
    #region IInventory
    [SerializeField]
    private Unit m_InventoryOwner = null;
    public IUnit InventoryOwner { get { return m_InventoryOwner; } }

    [SerializeField]
    private List<Item> m_ItemList = new List<Item>();
    public List<Item> ItemList { get { return m_ItemList; } }

    [SerializeField]
    private int m_InventoryCapacity = 0;
    public int InventoryCapacity { get { return m_InventoryCapacity; } }
    #endregion

    // Use this for initialization
    void Start () {
		foreach(Item InventoryItem in m_ItemList)
        {
            InventoryItem.InitOwner(m_InventoryOwner);
            InventoryItem.OnItemEquip();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
