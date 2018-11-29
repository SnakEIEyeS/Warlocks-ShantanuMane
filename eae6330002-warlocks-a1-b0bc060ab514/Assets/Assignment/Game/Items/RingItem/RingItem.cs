using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingItem : Item
{
    [SerializeField]
    private float m_HealthRegenBuff = 1.0f;

    #region Item
    public override bool OnItemEquip()
    {
        IUnitStatisticsController OwnerUnitStatController =
            (ItemOwner as MonoBehaviour).transform.root.GetComponentInChildren<IUnitStatisticsController>();
        if (OwnerUnitStatController != null)
        {
            OwnerUnitStatController.AddRawHealthRegenModifier(m_HealthRegenBuff);

            return true;
        }
        return false;
    }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
