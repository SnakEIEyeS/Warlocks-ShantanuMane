using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility, 
    IStatusEffectAbility, IDamagingAbility
{

    private PlayerController m_InstigatorController = null;
    private UnitController m_Caster = null;

    private AbilityTargetType m_TargetType = AbilityTargetType.NoTarget;
    private float m_CastTime = 1.0f;
    private float m_Cooldown = 5f;

    private float m_InnerRadius = 5.0f;
    private float m_OuterRadius = 20.0f;

    private float m_InnerDamage = 20.0f;
    private float m_OuterDamage = 5.0f;

    private float m_InnerForce = 20.0f;
    private float m_OuterForce = 100.0f;

    private float m_KnockbackDuration = 1.0f;

    [SerializeField]
    private LayerMask m_BlastLayerMask = -1;

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
        Blast();
    }
    public void AbilityEnd()
    {
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

    #region IDamagingAbility
    private DamageType m_AbilityDamageType = DamageType.Magical;
    public DamageType AbilityDamageType { get { return m_AbilityDamageType; } set { m_AbilityDamageType = value; } }

    private DamageEventBus m_DamageEventBus = null;
    public IDamageEventBus DamageEventBus { get { return m_DamageEventBus; } set { m_DamageEventBus = value as DamageEventBus; } }
    #endregion

    // Use this for initialization
    void Start()
    {
        StatusEventBus = FindObjectOfType<StatusEventBus>();
        m_DamageEventBus = FindObjectOfType<DamageEventBus>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Blast()
    {
        DrawDebugRadiusLines();

        Collider[] CollidersInBlast = Physics.OverlapSphere(m_Caster.getControlledUnit().transform.position, m_OuterRadius, m_BlastLayerMask.value, QueryTriggerInteraction.Collide);

        foreach (Collider CaughtCollider in CollidersInBlast)
        {
            UnitController hitUnitController = CaughtCollider.transform.root.GetComponentInChildren<UnitController>();
            if (hitUnitController && hitUnitController != m_Caster)
            {
                print("Blast: " + CaughtCollider.transform.root.name);

                Unit hitUnit = CaughtCollider.transform.root.GetComponentInChildren<Unit>();
                if(hitUnit)
                {
                    Vector3 ForceDirection = (hitUnitController.transform.position - m_Caster.getControlledUnit().transform.position).normalized;
                    m_StatusEventBus.KnockbackAttemptEvent.Invoke(
                        hitUnit, new Vector3(ForceDirection.x * m_OuterForce, 0f, ForceDirection.z * m_OuterForce), ForceMode.Impulse, m_KnockbackDuration
                        ); 
                }

                if (m_DamageEventBus)
                {
                    bool DamageAttemptSuccess = m_DamageEventBus.DamageAttempt(
                            new DamageDealer(m_Caster.getControlledUnit().Owner, m_Caster.getControlledUnit()),
                            CaughtCollider.transform.root.gameObject,
                            m_InnerDamage,
                            DamageType.Magical
                            );
                }


                /*Health hitHealth = CaughtCollider.transform.root.GetComponentInChildren<Health>();
                if (hitHealth)
                {
                    //hitHealth.TakeDamage(m_InnerDamage);
                    hitHealth.TakeDamage(new DamageInstance(
                        new DamageDealer(m_Caster.getControlledUnit().Owner, m_Caster.getControlledUnit()),
                        m_InnerDamage)
                        );
                }*/
            }

        }

        AbilityEnd();
    }

    private void DrawDebugRadiusLines()
    {
        Debug.DrawRay(m_Caster.transform.position, m_Caster.transform.forward * m_OuterRadius, Color.cyan, 3.0f, false);
        Debug.DrawRay(m_Caster.transform.position, m_Caster.transform.forward * m_InnerRadius, Color.red, 3.0f, false);
        Debug.DrawRay(m_Caster.transform.position, -(m_Caster.transform.forward * m_OuterRadius), Color.cyan, 3.0f, false);
        Debug.DrawRay(m_Caster.transform.position, -(m_Caster.transform.forward * m_InnerRadius), Color.red, 3.0f, false);
        Debug.DrawRay(m_Caster.transform.position, m_Caster.transform.right * m_OuterRadius, Color.cyan, 3.0f, false);
        Debug.DrawRay(m_Caster.transform.position, m_Caster.transform.right * m_InnerRadius, Color.red, 3.0f, false);
        Debug.DrawRay(m_Caster.transform.position, -(m_Caster.transform.right * m_OuterRadius), Color.cyan, 3.0f, false);
        Debug.DrawRay(m_Caster.transform.position, -(m_Caster.transform.right * m_InnerRadius), Color.red, 3.0f, false);
    }
}
