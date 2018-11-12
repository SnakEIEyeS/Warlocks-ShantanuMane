using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsItem : Item
{
    [SerializeField]
    private float m_RawMoveSpeedBuff = 9.0f;
    [SerializeField]
    private float m_RawTurnRateBuff = 80.0f;

    #region Item
    public override bool OnItemEquip()
    {
        IUnitStatisticsController OwnerUnitStatController = 
            (ItemOwner as MonoBehaviour).transform.root.GetComponentInChildren<IUnitStatisticsController>();
        if(OwnerUnitStatController != null)
        {
            OwnerUnitStatController.AddRawMoveSpeedModifier(m_RawMoveSpeedBuff);
            OwnerUnitStatController.AddRawTurnRateModifier(m_RawTurnRateBuff);

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
