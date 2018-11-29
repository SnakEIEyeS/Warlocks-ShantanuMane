using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitController : MonoBehaviour, IUnitController {

    [SerializeField]
    private Unit m_ControlledUnit = null;
    private IStatusAffectable m_UnitStatusAffectable = null;
    private GameObject m_AbilityRef = null;
    private AbilityHolder m_AbilityHolderRef = null;

    [SerializeField]
    private Map m_Map = null;

    private Vector3? m_MoveTarget;

    /*[SerializeField]
    private float m_MoveSpeed = 30f;
    [SerializeField]
    private float m_TurnRate = 720f;*/
    
    [SerializeField]
    private float m_MoveTargetTolerance = 0.5f;

    [SerializeField]
    private AbilityFactory m_AbilityFactory = null;
    [SerializeField]
    private AbilityEventBus m_AbilityEventBus = null;

    [SerializeField]
    private WarlockAnimationScript m_WarlockAnimScript = null;

#region IUnitController
    public bool HasReachedDestination { get { return !m_MoveTarget.HasValue; } }
    public void MoveTo(Vector3 targetPosition)
    {
        if(!m_UnitStatusAffectable.MovementBlocked)
        {
            m_MoveTarget = targetPosition;
            m_WarlockAnimScript.SetMoving(true);
        }
    }
    public void StopAll()
    {
        StopAbilityCast();
        StopMoving();
    }
    #endregion

    private void Start()
    {
        m_UnitStatusAffectable = m_ControlledUnit.GetComponentInChildren<IStatusAffectable>();

        m_AbilityEventBus.OnDirectionTargeted.AddListener(AbilityDirectionTargeted);
        m_AbilityEventBus.OnPointTargeted.AddListener(AbilityPointTargeted);
        m_AbilityEventBus.OnTargetingCanceled.AddListener(CancelAbilityTargeting);
    }
    private void Update()
    {
        UpdateTranslation();
        UpdateRotation();
        UpdateMapPosition();
    }

    public Unit getControlledUnit()
    {
        return m_ControlledUnit;
    }

    // this method makes sure the unit is on the ground, not hovering in the air or below the map
    private void UpdateMapPosition()
    {
        Vector3 mapPos, mapNormal;
        m_Map.GetMapPointFromWorldPoint(transform.position, out mapPos, out mapNormal);
        transform.position = mapPos;
        Vector3 right = transform.right;
        Vector3 up = mapNormal;
        Vector3 forward = Vector3.Cross(right, up);
        transform.rotation = Quaternion.LookRotation(forward, up);
    }

    private void UpdateTranslation()
    {
        if (m_MoveTarget.HasValue && !m_UnitStatusAffectable.MovementBlocked)
        {
            //print(m_MoveTarget);
            Vector3 Displacement = m_MoveTarget.Value - transform.position;
            float DistToMoveTarget = Displacement.magnitude;

            if (DistToMoveTarget > m_MoveTargetTolerance)
            {
                Vector3 Direction = Displacement.normalized;
                Vector3 Movement = Direction * m_ControlledUnit.MoveSpeed * Time.deltaTime;

                transform.position += Movement;
            }
            else
            {
                m_MoveTarget = null;
                OnDestinationReached();
            }
        }
    }

    private void UpdateRotation()
    {
        if (m_MoveTarget.HasValue && !m_UnitStatusAffectable.MovementBlocked)
        {
            Vector3 delta = (m_MoveTarget.Value - transform.position);
            Vector3 curDir = transform.forward;
            Vector3 targetDir = delta.normalized;
            float maxTurnPerFrame = m_ControlledUnit.TurnRate * Time.deltaTime;
            float angleLeft = Vector3.Angle(curDir, targetDir);
            float turnPerFrame = Mathf.Min(angleLeft, maxTurnPerFrame);
            Quaternion rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(targetDir, Vector3.up),
                turnPerFrame
            );
            transform.rotation = rotation;
        }
    }

    protected virtual void OnDestinationReached()
    {
        StopMoving();
    }

    public void StopMoving()
    {
        m_MoveTarget = null;
        m_WarlockAnimScript.SetMoving(false);
    }

     //Currently for regaining control after coming out of stunned state
    public void RegainControl()
    {
        Rigidbody rigidbody = GetComponentInChildren<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;
        //m_ControlledUnit.setCanMove(true);
    }

    public GameObject CreateAbility(int i_AbilityIndex)
    {
        if(!m_UnitStatusAffectable.AbilityCastBlocked)
        {
            /*List<GameObject> GOAbilityList = m_ControlledUnit.getAbilityList();
            if (GOAbilityList[i_AbilityIndex])
            {
                IObjectPool<GameObject> AbilityPool = m_AbilityFactory.Get(GOAbilityList[i_AbilityIndex]);
                GameObject AbilityGO = AbilityPool.Get();
                IAbility AbilityRef = AbilityGO.GetComponentInChildren<IAbility>();
                if (AbilityRef != null)
                {
                    //TODO AbilityRef.Instigator = need to get IPlayerController
                    m_AbilityRef = AbilityGO;

                    AbilityRef.Caster = this;

                    return m_AbilityRef;
                }
            }*/

            List<AbilityHolder> AbilityHolderList = m_ControlledUnit.getAbilityHolderList();
            if (AbilityHolderList[i_AbilityIndex]!=null && !AbilityHolderList[i_AbilityIndex].OnCooldown)
            {
                IObjectPool<GameObject> AbilityPool = m_AbilityFactory.Get(AbilityHolderList[i_AbilityIndex].Ability);
                GameObject AbilityGO = AbilityPool.Get();
                IAbility AbilityRef = AbilityGO.GetComponentInChildren<IAbility>();
                if (AbilityRef != null)
                {
                    //TODO AbilityRef.Instigator = need to get IPlayerController
                    m_AbilityRef = AbilityGO;
                    m_AbilityHolderRef = AbilityHolderList[i_AbilityIndex];
                    AbilityRef.Caster = this;

                    return m_AbilityRef;
                }
            }
        }
        
        return null;
    }

    public void CastAbility()
    {
        StopMoving();
        m_WarlockAnimScript.SetCasting(true);
        Invoke("ExecuteAbility", m_AbilityRef.GetComponentInChildren<ICastableAbility>().CastTime);
    }
    private void StopAbilityCast()
    {
        //TODO Think if AbilityRef should be kept indefinitely
        CancelInvoke("ExecuteAbility");
        m_WarlockAnimScript.SetCasting(false);
        CancelCurrentAbility();
    }
    public void ExecuteAbility()
    {
        if(m_AbilityRef)
        {
            m_AbilityRef.GetComponentInChildren<ICastableAbility>().AbilityExecute();
            //if (m_AbilityHolderRef.Ability.GetComponentInChildren<ICastableAbility>() != null)
            {
               // m_AbilityHolderRef.StartCooldown();
            }
            m_AbilityEventBus.OnCastComplete.Invoke(this);
            m_WarlockAnimScript.SetCasting(false);
            m_AbilityRef = null;
            m_AbilityHolderRef = null;
        }
    }
    private void CancelAbilityTargeting(UnitController i_UnitController)
    {
        if(i_UnitController == this)
        {
            if(m_AbilityRef)
            {
                CancelCurrentAbility();
            }
        }
    }
    private void CancelCurrentAbility()
    {
        if(m_AbilityRef)
        {
            m_AbilityRef.GetComponentInChildren<ICastableAbility>().AbilityEnd();
            m_AbilityRef = null;
        }
        if (m_AbilityHolderRef != null)
        {
            m_AbilityHolderRef = null;
        }
    }

    private void AbilityDirectionTargeted(UnitController i_UnitController, Vector3 i_Direction)
    {
        if(i_UnitController == this)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(i_Direction, Vector3.up),
                360);
        }
    }
    private void AbilityPointTargeted(UnitController i_UnitController, Vector3 i_Point)
    {
        if (i_UnitController == this)
        {
            Vector3 CastDirection = i_Point - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(CastDirection, Vector3.up),
                360);
        }
    }

}
