using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIAbilityIndex
{
    Teleport = 1,
    Blast = 3,
    Meteor = 5,
    Default = 9999
}

public enum AIAction
{
    Fight = 1,
    Flee = 2,
    Default = 0
}

public abstract class AIPlayerController : PlayerController {

    //[SerializeField]
    public Game m_Game = null;
    //[SerializeField]
    public Round m_Round = null;
    //[SerializeField]
    public Map m_Map = null;
    //[SerializeField]
    public Island m_Island = null;

    //[SerializeField]
    public UnitController m_SelectedUnitController = null;
    public PlayerStates m_AIPlayerState = PlayerStates.Default;

    [SerializeField]
    private AbilityEventBus m_AbilityEventBus = null;
    public List<int> m_UsableAbilityIndexes = new List<int>(){
        (int)AIAbilityIndex.Teleport,
        (int)AIAbilityIndex.Blast,
        (int)AIAbilityIndex.Meteor
    };

    private bool m_bAwaitingCastDirection = false;
    private bool m_bAwaitingCastPoint = false;

    public AIAction m_CurrentAction = AIAction.Default;
    public float m_DecisionCooldownTime = 1.0f;
    protected bool m_bDecisionOnCooldown = false;
    protected float m_FightUtility = 0.0f;
    protected float m_FleeUtility = 0.0f;


    protected void Start()
    {
        print("Parent AICtrlr print: " + gameObject.name);

        SetSelectedUnit(m_SelectedUnitController);
        m_AbilityEventBus.OnCastComplete.AddListener(CastComplete);
    }

    protected void Update()
    {
        switch(m_Round.Phase)
        {
            case RoundPhase.PreRound:
                break;

            case RoundPhase.InProgress:
                //Play
                PlayRound();
                break;

            case RoundPhase.Celebration:
                break;

            case RoundPhase.Completed:
                break;
        }
    }

    protected virtual void PlayRound()
    {
        if(!m_bDecisionOnCooldown)
        {
            MakeDecision();
            SetDecisionCooldown();
        }
        else
        {
            PerformAction();
        }
    }

    protected virtual void MakeDecision() {}
    protected virtual void PerformAction() {}

    protected virtual void CalculateFightUtility() {}
    protected virtual void CalculateFleeUtility() {}

    protected virtual void Fight() { }
    protected virtual void Flee() { }

    private void SetDecisionCooldown()
    {
        m_bDecisionOnCooldown = true;
        Invoke("ResetDecisionCooldown", m_DecisionCooldownTime);
    }

    private void ResetDecisionCooldown()
    {
        m_bDecisionOnCooldown = false;
    }

    public Vector3 GetMapPoint(Vector3 i_WorldPoint)
    {
        Vector3 ReturnMapPoint;
        Vector3 ReturnMapNormal;
        m_Map.GetMapPointFromWorldPoint(i_WorldPoint, out ReturnMapPoint, out ReturnMapNormal);

        return ReturnMapPoint;
    }

    public Vector3 GetHorizontalDirFromWorldPoint(Vector3 i_WorldPoint)
    {
        Vector3 RawDirection = (i_WorldPoint - SelectedUnit.getControlledUnit().transform.position).normalized;

        return new Vector3(RawDirection.x, 0.0f, RawDirection.z);
    }


    public void MoveCommand(Vector3 i_MoveLocation)
    {
        if(m_AIPlayerState == PlayerStates.Casting)
        {
            CancelCast();
        }
        SelectedUnit.StopAll();
        SelectedUnit.MoveTo(i_MoveLocation);
    }

    public AbilityTargetType CastAbilityCommand(int i_AbilityIndex)
    {
        GameObject CurrentCastAbility = m_SelectedUnitController.CreateAbility(i_AbilityIndex);

        if (CurrentCastAbility != null)
        {
            AbilityTargetType CurrentTargetType = CurrentCastAbility.GetComponentInChildren<ICastableAbility>().TargetType;

            switch (CurrentTargetType)
            {
                case AbilityTargetType.PointTarget:
                    m_bAwaitingCastPoint = true;
                    m_AIPlayerState = PlayerStates.Targeting;
                    return CurrentTargetType;

                case AbilityTargetType.DirectionTarget:
                    m_bAwaitingCastDirection = true;
                    m_AIPlayerState = PlayerStates.Targeting;
                    return CurrentTargetType;

                case AbilityTargetType.NoTarget:
                    SelectedUnit.CastAbility();
                    m_AIPlayerState = PlayerStates.Casting;
                    return CurrentTargetType;

                default:
                    return AbilityTargetType.Default;
            }
        }

        return AbilityTargetType.Default;
    }

    public bool OrderCastDirection(Vector3 i_CastDirection)
    {
        if(m_AIPlayerState == PlayerStates.Targeting && m_bAwaitingCastDirection)
        {
            //SelectedUnit.StopAll();
            m_AbilityEventBus.OnDirectionTargeted.Invoke(SelectedUnit, i_CastDirection);
            m_SelectedUnitController.CastAbility();
            m_bAwaitingCastDirection = false;
            m_AIPlayerState = PlayerStates.Casting;
            return true;
        }

        return false;
    }

    public bool OrderCastPoint(Vector3 i_CastPoint)
    {
        if(m_AIPlayerState == PlayerStates.Targeting && m_bAwaitingCastPoint)
        {
            //SelectedUnit.StopAll();
            m_AbilityEventBus.OnPointTargeted.Invoke(SelectedUnit, i_CastPoint);
            m_SelectedUnitController.CastAbility();
            m_bAwaitingCastPoint = false;
            m_AIPlayerState = PlayerStates.Casting;
            return true;
        }

        return false;
    }

    void CancelCast()
    {
        m_AIPlayerState = PlayerStates.Default;
        m_bAwaitingCastDirection = false;
        m_bAwaitingCastPoint = false;
    }

    void CastComplete(UnitController i_UnitController)
    {
        if(i_UnitController == SelectedUnit)
        {
            m_AIPlayerState = PlayerStates.Default;
        }
    }


}
