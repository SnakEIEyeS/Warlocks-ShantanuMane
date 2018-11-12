using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility, IStatusEffectAbility
{

    private PlayerController m_InstigatorController = null;
    private UnitController m_Caster = null;

    private Vector3 m_CastDirection = new Vector3(0.0f, 0.0f, 0.0f);
    private float m_MaxReach = 35f;
    private float m_PushForce = 500f;
    private float m_StunDuration = 1f;
    private AbilityTargetType m_TargetType = AbilityTargetType.DirectionTarget;
    private float m_CastTime = 0.5f;
    private float m_Cooldown = 5f;

    private IObjectPool<GameObject> m_AbilityPool = null;
    private StatusEventBus m_StatusEventBus = null;


    #region IAbility
    public IPlayerController Instigator
    {
        get { return m_InstigatorController; }
        set { m_InstigatorController = value as PlayerController; }
    }
    public IUnitController Caster
    {
        get { return m_Caster; }
        set { m_Caster = value as UnitController; }
    }
    #endregion

    #region ICastableAbility
    public AbilityTargetType TargetType { get { return m_TargetType; } }
    public float CastTime { get { return m_CastTime; } }
    public float Cooldown
    {
        get { return m_Cooldown; }
        set { m_Cooldown = value; }
    }
    public void AbilityExecute()
    {
        Push();
    }
    public void AbilityEnd()
    {
        //m_Caster.ReturnAbility(this.gameObject);
        m_Caster = null;
        m_InstigatorController = null;
        m_AbilityPool.Return(this.gameObject);
    }
    #endregion

    #region IInstantiableAbility
    public IObjectPool<GameObject> AbilityPool { get { return m_AbilityPool; } set { m_AbilityPool = value; } }
    #endregion

    #region IStatusEffectAbility
    public IStatusEventBus StatusEventBus { get { return m_StatusEventBus; } set { m_StatusEventBus = value as StatusEventBus; } }
    #endregion


    // Use this for initialization
    void Start () {
        AbilityEventBus abilityEventBus = (AbilityEventBus)FindObjectOfType<AbilityEventBus>();
        abilityEventBus.OnDirectionTargeted.AddListener(SetCastDirection);

        StatusEventBus = FindObjectOfType<StatusEventBus>();
	}
	
	// Update is called once per frame
	void Update () {
        //print(m_Caster.gameObject.name);
	}

    private void SetCastDirection(UnitController i_UnitController, Vector3 i_Direction)
    {
        if (m_Caster && m_Caster == i_UnitController)
        {
            m_CastDirection = i_Direction;
        }
    }

    private void Push()
    {
        print("PUSH!");

        Vector3 CasterLocation = m_Caster.getControlledUnit().transform.position;
        //Vector3 rayStart = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Vector3 rayStart = new Vector3(CasterLocation.x, CasterLocation.y + 1f, CasterLocation.z);
        //Ray ray = new Ray(rayStart, transform.forward);
        //Debug.DrawRay(rayStart, transform.forward*m_MaxReach, Color.blue, 2.0f);
        Ray ray = new Ray(rayStart, m_CastDirection);
        Debug.DrawRay(rayStart, m_CastDirection * m_MaxReach, Color.blue, 2.0f);
        //TODO change to spherecast
        //TODO change raycast to use layermask?
        RaycastHit[] hits = Physics.RaycastAll(ray, m_MaxReach);

        foreach (RaycastHit hit in hits)
        {
            UnitController hitUnitController = hit.transform.gameObject.GetComponentInChildren<UnitController>();
            if (hit.transform.gameObject == m_InstigatorController ||
                hitUnitController != null && hitUnitController == m_Caster)
            {
                print("Hit myself!");
            }
            else
            {
                print(hit.transform.gameObject.name);

                /*Rigidbody hitRigidbody = hit.transform.gameObject.GetComponentInChildren<Rigidbody>();
                if (hitRigidbody != null)
                {
                    if (hitUnitController != null)
                    {
                        hitRigidbody.isKinematic = false;
                        //hitRigidbody.AddForce(transform.forward.x * m_PushForce, 0f, transform.forward.z * m_PushForce, ForceMode.Impulse);
                        hitRigidbody.AddForce(m_CastDirection.x * m_PushForce, 0f, m_CastDirection.z * m_PushForce, ForceMode.Impulse);
                        hitUnitController.getControlledUnit().setCanMove(false);
                        hitUnitController.Invoke("RegainControl", m_StunDuration);
                    }
                }*/

                Unit hitUnit = hit.transform.root.GetComponentInChildren<Unit>();
                if (hitUnit != null)
                {
                    m_StatusEventBus.KnockbackAttemptEvent.Invoke(
                        hitUnit, new Vector3(m_CastDirection.x * m_PushForce, 0f, m_CastDirection.z * m_PushForce), ForceMode.Impulse, m_StunDuration
                        );
                }
            }
        }

        AbilityEnd();


    }

}
