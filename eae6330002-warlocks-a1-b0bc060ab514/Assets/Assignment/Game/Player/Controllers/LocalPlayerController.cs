using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerController : PlayerController, IInputController {

    //[SerializeField]
    //private Transform m_TargetMarker = null;
    

    [SerializeField]
    private UnitDetailsHUD m_UnitDetailsHUD = null;

    //[SerializeField]
    //private UnitController m_SelectedUnit = null;

    [SerializeField]
    private Map map = null;

    [SerializeField]
    private Stack<IInputController> m_InputControllers = new Stack<IInputController>();

    #region AllInputControllers
    [SerializeField]
    private PlayerDefaultController m_DefaultController = null;
    [SerializeField]
    private DirectionTargetController m_DirectionTargetController = null;
    [SerializeField]
    private PointTargetController m_PointTargetController = null;
    [SerializeField]
    private CastController m_CastController = null;
    #endregion

    #region AbilityRelated
    [SerializeField]
    private AbilityEventBus m_AbilityEventBus = null;

    private IInputController m_CurrentTargetingController = null;
    #endregion

    #region IInputController
    public void UpdateInput()
    {
        
    }
#endregion

    private void Start()
    {
        //SelectUnit(((Unit)(Units[0])).GetComponentInChildren<UnitController>());
        /*m_InputControllers.Push(m_DefaultController);
        m_DefaultController.SelectHero();
        m_InputControllers.Pop();
        m_InputControllers.Push(m_DirectionTargetController);*/
        PlayerState = PlayerStates.Default;
        m_AbilityEventBus.OnDirectionTargeted.AddListener(AbilityGroundTargeted);
        m_AbilityEventBus.OnPointTargeted.AddListener(AbilityGroundTargeted);
        m_AbilityEventBus.OnTargetingCanceled.AddListener(AbilityTargetingCanceled);
        m_AbilityEventBus.OnCastComplete.AddListener(AbilityCastComplete);
    }

    private void Update()
    {

        //m_InputControllers.Peek().UpdateInput();
        /*if(PlayerState != PlayerStates.Casting)
        {
            UpdateRegularInput();
        }*/

        switch (PlayerState)
        {
            case PlayerStates.Default:
                UpdateRegularInput();
                m_DefaultController.UpdateInput();
                break;

            case PlayerStates.Targeting:
                m_CurrentTargetingController.UpdateInput();
                break;

            case PlayerStates.Casting:
                m_CastController.UpdateInput();
                break;

            default:
                break;
        }
    }

    private void UpdateRegularInput()
    {
        //Cast 1st ability
        if (Input.GetButtonUp("Ability1"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(0);
            }
        }
        if (Input.GetButtonUp("Ability2"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(1);
            }
        }
        if (Input.GetButtonUp("Ability3"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(2);
            }
        }
        if (Input.GetButtonUp("Ability4"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(3);
            }
        }
        if (Input.GetButtonUp("Ability5"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(4);
            }
        }
        if (Input.GetButtonUp("Ability6"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(5);
            }
        }
        if (Input.GetButtonUp("Ability7"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(6);
            }
        }
        if (Input.GetButtonUp("Ability8"))
        {
            if (IsSelectedUnitOwned())
            {
                CastAbilityCommand(7);
            }
        }

        if (Input.GetButtonDown("Stop"))
        {
            SelectedUnit.StopAll();
        }
    }

    public UnitController getSelectedUnit()
    {
        return SelectedUnit;
    }

    //Get GameObject hit at mouse position
    /*private GameObject GetMouseSelection()
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
    }*/

    //For handling left clicks. To be expanded for more functionality like casting spells in the future.
    /*private void HandleUnitSelection(UnitController i_UnitController)
    {
        SelectUnit(i_UnitController);
    }*/

    //Select a unit, for  displaying details in HUD
    public void SelectUnit(UnitController i_UnitController)
    {
        SelectedUnit = i_UnitController;
        m_UnitDetailsHUD.setSelectedUnit(SelectedUnit);
    }

    //Returns true if currently selected unit is owned by this player
    private bool IsSelectedUnitOwned()
    {
        return Units.Contains(SelectedUnit.getControlledUnit());
    }

    public void CastAbilityCommand(int i_AbilityIndex)
    {
        GameObject CurrentCastAbility = SelectedUnit.CreateAbility(i_AbilityIndex);

        if(CurrentCastAbility != null)
        {
            AbilityTargetType CurrentTargetType = CurrentCastAbility.GetComponentInChildren<ICastableAbility>().TargetType;

            switch(CurrentTargetType)
            {
                case AbilityTargetType.PointTarget:
                    m_CurrentTargetingController = m_PointTargetController;
                    SetActiveTargetingController();
                    break;

                case AbilityTargetType.DirectionTarget:
                    m_CurrentTargetingController = m_DirectionTargetController;
                    SetActiveTargetingController();
                    break;

                case AbilityTargetType.NoTarget:
                    SetActiveCastController();
                    break;

                default:
                    break;
            }
        }
    }

    private void AbilityGroundTargeted(UnitController i_UnitController, Vector3 i_Direction)
    {
        if(i_UnitController == SelectedUnit && IsSelectedUnitOwned())
        {
            SetActiveCastController();
        }
    }
    private void AbilityTargetingCanceled(UnitController i_UnitController)
    {
        if(i_UnitController == SelectedUnit && IsSelectedUnitOwned())
        {
            SetActiveDefaultController();
        }
    }
    private void AbilityCastComplete(UnitController i_UnitController)
    {
        if(i_UnitController == SelectedUnit && IsSelectedUnitOwned())
        {
            SetActiveDefaultController();
        }
    }

    private void SetActiveDefaultController()
    {
        PlayerState = PlayerStates.Default;
    }
    private void SetActiveTargetingController()
    {
        PlayerState = PlayerStates.Targeting;
    }
    private void SetActiveCastController()
    {
        PlayerState = PlayerStates.Casting;
        m_CastController.RelayAbilityCast(SelectedUnit);
    }

}
