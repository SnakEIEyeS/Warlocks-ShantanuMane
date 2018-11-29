using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IPlayerController {

    [SerializeField]
    private Player m_Player = null;

    [SerializeField]
    private List<Unit> m_OwnedUnits = new List<Unit>();

    private UnitController m_SelectedUnit = null;
    public UnitController SelectedUnit { get { return m_SelectedUnit; } set { m_SelectedUnit = value; } }

    private PlayerStates m_PlayerState = PlayerStates.Default;
    public PlayerStates PlayerState { get { return m_PlayerState; } set { m_PlayerState = value; } }

#region IPlayerController
    public IPlayer Player { get { return m_Player; } }
    public List<IUnit> Units { get { return m_OwnedUnits.ConvertAll<IUnit>((u) => u as IUnit); } }
#endregion

    //TODO Do this better - events?
    public void SetSelectedUnit(UnitController i_UnitController)
    {
        m_SelectedUnit = i_UnitController;
    }

}
