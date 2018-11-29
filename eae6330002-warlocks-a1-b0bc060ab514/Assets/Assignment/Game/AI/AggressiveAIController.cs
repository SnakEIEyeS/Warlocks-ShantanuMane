using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveAIController : AIPlayerController {

    [SerializeField]
    private float m_LostHealthFleeExponent = 5.0f;
    [SerializeField]
    private float m_LostHealthFleeWeight = 3.0f;
    [SerializeField]
    private float m_LostHealthFightExponent = 0.4f;
    [SerializeField]
    private float m_LostHealthFightWeight = 4.0f;
    [SerializeField]
    private float m_AbilityCooldownFightWeight = 2.0f;

    [SerializeField]
    private float m_ThreatRadius = 10.0f;
    private int m_UnitThreatCount = 0;
    [SerializeField]
    private float m_UnitThreatExponent = 3.0f;
    [SerializeField]
    private float m_UnitThreatWeight = 1.5f;

    [SerializeField]
    private Unit m_FightTarget = null;
    [SerializeField]
    private float m_FightRange = 10.0f;

	// Use this for initialization
	void Start ()
    {
        base.Start();
        //print("Aggressive print");

        //Invoke("TestMove", 1.0f);
        //Invoke("TestCast", 1.3f);

    }
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
	}

    protected override void PlayRound()
    {
        //Calculates Fight and Flee utilities
        base.PlayRound();
        
    }

    protected override void MakeDecision()
    {
        //base.MakeDecision();
        CalculateFightUtility();
        CalculateFleeUtility();
        if (m_FightUtility >= m_FleeUtility)
        {
            //Fight!
            print(transform.gameObject + " wants to Fight!");
            m_CurrentAction = AIAction.Fight;
            if(m_FightTarget == null)
            {
                SelectFightTarget();
            }
        }
        else
        {
            //Flee
            print(transform.gameObject + " wants to Flee!");
            m_CurrentAction = AIAction.Flee;
        }
    }

    protected override void PerformAction()
    {
        //base.PerformAction();
        switch(m_CurrentAction)
        {
            case AIAction.Fight:
                Fight();
                break;

            case AIAction.Flee:
                Flee();
                break;

            default:
                break;
        }
    }

    protected override void CalculateFightUtility()
    {
        //print("Aggro fight util");
        Unit controlledUnit = SelectedUnit.getControlledUnit();

        int abilityCooldownCount = 0;
        foreach (int AbilityIndex in m_UsableAbilityIndexes)
        {
            if(controlledUnit.getAbilityHolderList()[AbilityIndex]!=null 
                && controlledUnit.getAbilityHolderList()[AbilityIndex].OnCooldown)
            {
                abilityCooldownCount++;
            }
        }
        float abilityCooldownFactor = 
            ((m_UsableAbilityIndexes.Count - abilityCooldownCount) / m_UsableAbilityIndexes.Count) * m_AbilityCooldownFightWeight;

        float maxHealth = controlledUnit.Health.Max;
        float lostHealth = maxHealth - controlledUnit.Health.Current;
        float healthFactor = Mathf.Pow((float)(lostHealth / maxHealth), m_LostHealthFightExponent) * m_LostHealthFightWeight;

        m_FightUtility = (abilityCooldownFactor + healthFactor) / (m_AbilityCooldownFightWeight + m_LostHealthFightWeight);
    }

    protected override void CalculateFleeUtility()
    {
        //print("Aggro flee util");
        Unit controlledUnit = SelectedUnit.getControlledUnit();

        float maxHealth = controlledUnit.Health.Max;
        float lostHealth = maxHealth - controlledUnit.Health.Current;
        float healthFactor = Mathf.Pow((float)(lostHealth / maxHealth), m_LostHealthFleeExponent) * m_LostHealthFleeWeight;

        m_UnitThreatCount = 0;
        Collider[] caughtColliders = Physics.OverlapSphere(controlledUnit.transform.position, m_ThreatRadius);
        foreach(Collider colliderInThreatRadius in caughtColliders)
        {
            if(colliderInThreatRadius.transform.root.GetComponentInChildren<IUnit>() != null)
            {
                m_UnitThreatCount++;
            }
        }

        float unitThreatFactor = Mathf.Pow((float)(m_UnitThreatCount / m_Round.AliveUnits.Count), m_UnitThreatExponent) * m_UnitThreatWeight;

        m_FleeUtility = (float)((healthFactor + unitThreatFactor) / (m_LostHealthFleeWeight + m_UnitThreatWeight));

    }

    protected override void Fight()
    {
        //print(transform.gameObject.name + " performing fight action");
        //if(m_AIPlayerState == PlayerStates.Default)
        {
            Unit controlledUnit = SelectedUnit.getControlledUnit();

            //Target not in range
            if ((m_FightTarget.transform.position - controlledUnit.transform.position).magnitude
                > m_FightRange)
            {
                if (IsAbilityReady((int)AIAbilityIndex.Teleport))
                //if(false)
                {
                    //Teleport
                }
                else
                {
                    //Move to target
                    //print("I want to move to target");
                    MoveCommand(GetMapPoint((m_FightTarget.transform.position - controlledUnit.transform.position).normalized * 2.0f));
                    //print(GetMapPoint((m_FightTarget.transform.position - controlledUnit.transform.position).normalized * 2.0f));
                }
            }
            
            else
            {
                //Look for Blast opportunity
                if (BlastOpportunity(controlledUnit))
                {
                    //Cast Blast
                }
                else if (IsAbilityReady((int)AIAbilityIndex.Meteor))
                {
                    //Cast Meteor, save up Blast
                }
                else if(IsAbilityReady((int)AIAbilityIndex.Blast))
                {
                    //Cast Blast as a last resort
                }
            }
            
        }
    }

    private void SelectFightTarget()
    {
        m_FightTarget = null;

        int targetIndex = (int)Random.Range(0.0f, m_Round.AliveUnits.Count - 1);
        while (m_FightTarget == null)
        {
            Unit aliveUnit = m_Round.AliveUnits[targetIndex] as Unit;
            if (aliveUnit && aliveUnit != SelectedUnit.getControlledUnit())
            {
                m_FightTarget = m_Round.AliveUnits[targetIndex] as Unit;
            }
            else
            {
                targetIndex = (targetIndex + 1) % m_Round.AliveUnits.Count;
            }
        }
    }

    private bool IsAbilityReady(int i_AbilityIndex)
    {
        List<AbilityHolder> unitAbilityHolderList = SelectedUnit.getControlledUnit().getAbilityHolderList();

        if (unitAbilityHolderList[i_AbilityIndex] != null && !unitAbilityHolderList[i_AbilityIndex].OnCooldown)
        {
            return true;
        }

        return false;
    }

    private bool BlastOpportunity(Unit controlledUnit)
    {
        if(IsAbilityReady((int)AIAbilityIndex.Blast))
        {
            int blastPotentialTargets = 0;
            Collider[] blastPotentialColliders = Physics.OverlapSphere(controlledUnit.transform.position, m_ThreatRadius);
            foreach (Collider blastCollider in blastPotentialColliders)
            {
                if (blastCollider.transform.root.GetComponentInChildren<IUnit>() != null)
                {
                    blastPotentialTargets++;
                }
            }

            //if multiple targets possibly within radius
            if (blastPotentialTargets > 1)
            {
                //Cast Blast
                return true;
            }
        }
        
        return false;
    }

    protected override void Flee()
    {
        m_FightTarget = null;
    }



    void TestMove()
    {
        MoveCommand(GetMapPoint(SelectedUnit.getControlledUnit().transform.position - new Vector3(10.0f, 0.0f, 10.0f)));
    }
    void TestCast()
    {
        AbilityTargetType testTargetType = CastAbilityCommand(1);
        switch(testTargetType)
        {
            case AbilityTargetType.DirectionTarget:
                TestDirectionCast();
                break;

            case AbilityTargetType.PointTarget:
                TestPointCast();
                break;

            case AbilityTargetType.NoTarget:
                break;

            default:
                break;
        }
    }
    void TestPointCast()
    {
        print("PointCast");
        OrderCastPoint(GetMapPoint(SelectedUnit.getControlledUnit().transform.position - new Vector3(10.0f, 0.0f, 10.0f)));
    }
    void TestDirectionCast()
    {
        print("DirectionCast");
        OrderCastDirection(SelectedUnit.getControlledUnit().transform.forward);
    }
}
