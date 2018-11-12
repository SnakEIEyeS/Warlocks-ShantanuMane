using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    #region IItem
    [SerializeField]
    private Unit m_UnitOwner = null;
    public IUnit ItemOwner { get { return m_UnitOwner; } set { m_UnitOwner = value as Unit; } }
    public virtual bool OnItemEquip()
    {
        return true;
    }
    #endregion

    public void InitOwner(IUnit i_Unit)
    {
        ItemOwner = i_Unit;
    }

    // Use this for initialization
    private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		
	}
}
