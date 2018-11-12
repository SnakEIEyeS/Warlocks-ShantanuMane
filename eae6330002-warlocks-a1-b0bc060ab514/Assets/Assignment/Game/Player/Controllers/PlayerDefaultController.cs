using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultController : MonoBehaviour, IInputController {

    private AbilityTargetType m_TargetTypeTemp = AbilityTargetType.Default;

    [SerializeField]
    private UnitDetailsHUD m_UnitDetailsHUD = null;

    [SerializeField]
    private UnitController m_SelectedUnit = null;

    [SerializeField]
    private Map map = null;

    [SerializeField]
    private PlayerController m_ParentPlayerController = null;

    #region IInputController
    public void UpdateInput()
    {
        if (Input.GetButtonDown("SelectHero"))
        {
            SelectHero();
            //TODO Do this better
            m_ParentPlayerController.SetSelectedUnit(m_SelectedUnit);
        }

        //Move command
        if (Input.GetMouseButtonDown(1))
        {
            //print("Right-clicked");
            MoveSelectedUnit();
        }

        //Select command: Left click
        if (Input.GetMouseButtonDown(0))
        {
            //print("Left-clicked");
            GameObject hitGameObject = GetMouseSelection();
            if (hitGameObject && hitGameObject.GetComponentInChildren<UnitController>())
            {
                HandleUnitSelection(hitGameObject.GetComponentInChildren<UnitController>());
            }
        }

        

        if (Input.GetButtonDown("Stop"))
        {
            m_SelectedUnit.StopAll();
        }
    }
    #endregion

    // Use this for initialization
    void Start () {
        SelectHero();
        //TODO Do this better?
        m_ParentPlayerController.SelectedUnit = m_SelectedUnit;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public UnitController getSelectedUnit()
    {
        return m_SelectedUnit;
    }

    public void SelectHero()
    {
        SelectUnit(((Unit)(m_ParentPlayerController.Units[0])).GetComponentInChildren<UnitController>());
    }

    //Get GameObject hit at mouse position
    private GameObject GetMouseSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit))
        {
            return raycastHit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    //For handling left clicks. To be expanded for more functionality like casting spells in the future.
    private void HandleUnitSelection(UnitController i_UnitController)
    {
        SelectUnit(i_UnitController);
        m_ParentPlayerController.SelectedUnit = i_UnitController;
    }

    //Select a unit, for  displaying details in HUD
    private void SelectUnit(UnitController i_UnitController)
    {
        m_SelectedUnit = i_UnitController;
        m_UnitDetailsHUD.setSelectedUnit(m_SelectedUnit);
    }

    //Returns true if currently selected unit is owned by this player
    private bool IsSelectedUnitOwned()
    {
        return m_ParentPlayerController.Units.Contains(m_SelectedUnit.GetComponentInChildren<Unit>());
    }

    //Moves selected unit if it is owned by this player
    private void MoveSelectedUnit()
    {
        if (IsSelectedUnitOwned())
        {
            Vector3 mapPos, mapNormal;
            bool hit = map.GetMapPointFromScreenPoint(Input.mousePosition, out mapPos, out mapNormal);
            if (hit)
            {
                m_SelectedUnit.MoveTo(mapPos);
            }
        }
    }

    public void CastAbilityCommand(int i_AbilityIndex)
    {
        GameObject CurrentCastAbility = m_SelectedUnit.CreateAbility(i_AbilityIndex);

        if (CurrentCastAbility != null)
        {
            AbilityTargetType CurrentTargetType = CurrentCastAbility.GetComponentInChildren<ICastableAbility>().TargetType;

            switch (CurrentTargetType)
            {
                case AbilityTargetType.PointTarget:
                    m_TargetTypeTemp = AbilityTargetType.PointTarget;
                    break;
                case AbilityTargetType.DirectionTarget:
                    m_TargetTypeTemp = AbilityTargetType.DirectionTarget;
                    break;
                case AbilityTargetType.NoTarget:
                    m_SelectedUnit.ExecuteAbility();
                    break;

                default:
                    break;
            }
        }
    }
}
