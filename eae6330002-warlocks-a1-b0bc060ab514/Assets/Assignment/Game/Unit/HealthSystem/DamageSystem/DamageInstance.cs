using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInstance : IDamageInstance {

    private DamageDealer m_DamageDealer = null;
    private float m_DamageAmount = 0.0f;
    private DamageType m_DamageType = DamageType.Normal;
    private IDamageable m_Damageable = null;

    #region IDamageInstance
    public IDamageDealer DamageDealer { get { return m_DamageDealer; } set { m_DamageDealer = value as DamageDealer; } }
    public float DamageAmount { get { return m_DamageAmount; } set { m_DamageAmount = value; } }
    public DamageType DamageType { get { return m_DamageType; } set { m_DamageType = value; } }
    public IDamageable Damageable { get { return m_Damageable; } set { m_Damageable = value; } }
    #endregion

    public DamageInstance(DamageDealer i_DamageDealer, float i_DamageAmount)
    {
        DamageDealer = i_DamageDealer;
        DamageAmount = i_DamageAmount;
    }

    public DamageInstance(DamageDealer i_DamageDealer, float i_DamageAmount, DamageType i_DamageType)
    {
        DamageDealer = i_DamageDealer;
        DamageAmount = i_DamageAmount;
        DamageType = i_DamageType;
    }

    public DamageInstance(DamageDealer i_DamageDealer, IDamageable i_Damageable, float i_DamageAmount, DamageType i_DamageType)
    {
        DamageDealer = i_DamageDealer;
        DamageAmount = i_DamageAmount;
        DamageType = i_DamageType;
        Damageable = i_Damageable;
    }
}
