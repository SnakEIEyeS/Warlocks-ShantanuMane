using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : IDamageDealer {

    private Player m_InstigatorPlayer = null;
    private Unit m_InstigatorUnit = null;
    private IDamageInflicter m_DamageInflicter = null;

    #region IDamageDealer
    public IPlayer InstigatorPlayer { get { return m_InstigatorPlayer; } set { m_InstigatorPlayer = value as Player; } }
    public IUnit InstigatorUnit { get { return m_InstigatorUnit; } set { m_InstigatorUnit = value as Unit; } }
    public IDamageInflicter DamageInflicter { get { return m_DamageInflicter; } set { m_DamageInflicter = value; } }
    #endregion

    public DamageDealer(Player i_Player, Unit i_Unit)
    {
        InstigatorPlayer = i_Player;
        InstigatorUnit = i_Unit;
        IDamageInflicter FoundDamageInflicter = i_Unit.transform.root.GetComponentInChildren<IDamageInflicter>();
        if (FoundDamageInflicter != null)
        {
            DamageInflicter = FoundDamageInflicter;
        }
    }

    public DamageDealer(Player i_Player, Unit i_Unit, IDamageInflicter i_DamageInflicter)
    {
        InstigatorPlayer = i_Player;
        InstigatorUnit = i_Unit;
        DamageInflicter = i_DamageInflicter;
    }
}
