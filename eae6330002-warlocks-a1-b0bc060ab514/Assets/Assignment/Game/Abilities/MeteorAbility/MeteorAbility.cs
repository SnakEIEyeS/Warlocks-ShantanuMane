using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
public class MeteorAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility, 
    IStatusEffectAbility, IMovingAbility, IDamagingAbility
{
    [SerializeField]
    private PhotonView m_PhotonView = null;
    [SerializeField]
    private PhotonView m_MeshPhotonView = null;

    private PlayerController m_InstigatorController = null;
    private UnitController m_Caster = null;

    private AbilityTargetType m_TargetType = AbilityTargetType.DirectionTarget;
    private float m_CastTime = 1.0f;
    private float m_Cooldown = 15.0f;

    private Vector3 m_StartPosition = Vector3.zero;
    private Vector3 m_ImpactPoint = Vector3.zero;
    private float m_ImpactDamage = 10.0f;
    private float m_RollDamage = 5.0f;
    private float m_FallTime = 1.0f;
    private float m_FallHtAndDist = 20.0f;
    private float m_FallSpeed = 0.0f;
    private float m_ImpactForce = 10.0f;
    private float m_ImpactRange = 10.0f;
    private float m_ImpackKnockbackDuration = 0.5f;
    private float m_RollSpeed = 10.0f;
    private float m_RollDuration = 2.0f;

    private bool m_bMeteorMoving = false;
    private bool m_bMeteorFalling = false;
    private bool m_bMeteorRolling = false;

    [SerializeField]
    private SphereCollider m_SphereCollider = null;
    [SerializeField]
    private GameObject m_AbilityMesh = null;

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
        m_PhotonView.RPC("SummonMeteor", PhotonNetwork.MasterClient);
        //SummonMeteor();
    }
    public void AbilityEnd()
    {
        m_bMeteorMoving = false;
        m_bMeteorFalling = false;
        m_bMeteorRolling = false;

        m_Caster = null;
        m_InstigatorController = null;

        //m_AbilityPool.Return(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
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

    void Awake()
    {
        GameObject CasterGO = PhotonView.Find((int)m_PhotonView.InstantiationData[0]).gameObject;
        m_Caster = CasterGO.GetComponentInChildren<UnitController>();
        
    }

    // Use this for initialization
    void Start()
    {
        AbilityEventBus abilityEventBus = (AbilityEventBus)FindObjectOfType<AbilityEventBus>();
        abilityEventBus.OnDirectionTargeted.AddListener(SetMeteorSummonVars);

        StatusEventBus = FindObjectOfType<StatusEventBus>();
        m_DamageEventBus = FindObjectOfType<DamageEventBus>();

        m_FallSpeed = m_FallHtAndDist / m_FallTime;

        m_MeshPhotonView.TransferOwnership(PhotonNetwork.MasterClient);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bMeteorMoving && m_PhotonView.IsMine)
        {
            MoveMeteor();
        }
    }

    private void MoveMeteor()
    {
        if (m_bMeteorFalling)
        {
            this.transform.root.position -= transform.root.up * m_FallSpeed * Time.deltaTime;
            this.transform.root.position += transform.root.forward * m_FallSpeed * Time.deltaTime;
        }
        else if (m_bMeteorRolling)
        {
            this.transform.root.position += transform.root.forward * m_RollSpeed * Time.deltaTime;
        }

        //Debug.Log(gameObject.transform.position);
    }

    private void SetMeteorSummonVars(UnitController i_UnitController, Vector3 i_Direction)
    {
        if (m_Caster == i_UnitController)
        {
            //this.transform.root.forward = i_Direction;
            //m_StartPosition = m_Caster.getControlledUnit().transform.position + new Vector3(0.0f, m_FallHtAndDist, 0.0f);

            AbilityEventBus abilityEventBus = (AbilityEventBus)FindObjectOfType<AbilityEventBus>();
            abilityEventBus.OnDirectionTargeted.RemoveListener(SetMeteorSummonVars);
            m_PhotonView.RPC("RPC_SetMeteorSummonVars", RpcTarget.MasterClient, i_Direction);
        }
    }

    [PunRPC]
    private void RPC_SetMeteorSummonVars(Vector3 i_Direction)
    {
        this.transform.root.forward = i_Direction;
        m_StartPosition = m_Caster.getControlledUnit().transform.position + new Vector3(0.0f, m_FallHtAndDist, 0.0f);
    }

    [PunRPC]
    private void SummonMeteor()
    {
        this.transform.root.position = m_StartPosition;
        m_SphereCollider.radius = m_ImpactRange / 2.0f;
        m_AbilityMesh.transform.localScale = new Vector3(m_ImpactRange, m_ImpactRange, m_ImpactRange);
        m_ImpactPoint = new Vector3(0.0f, -m_SphereCollider.radius, 0.0f);

        m_bMeteorMoving = true;
        m_bMeteorFalling = true;
        Invoke("MeteorImpact", m_FallTime);
    }

    private void MeteorImpact()
    {
        Collider[] MeteorImpactCaught = Physics.OverlapSphere(this.gameObject.transform.position, m_ImpactRange);
        foreach(Collider impactCaught in MeteorImpactCaught)
        {
            ApplyImpactForce(impactCaught);
            ApplyImpactDamage(impactCaught);
        }

        MeteorRoll();
    }
    private void ApplyImpactForce(Collider i_ImpactCaughtCollider)
    {
        /*Rigidbody otherRigidBody = i_ImpactCaughtCollider.transform.root.GetComponentInChildren<Rigidbody>();
        if (otherRigidBody)
        {
            UnitController otherUnitController = i_ImpactCaughtCollider.transform.root.GetComponentInChildren<UnitController>();
            if (otherUnitController)
            {
                otherRigidBody.isKinematic = false;
                otherUnitController.getControlledUnit().setCanMove(false);
                otherUnitController.Invoke("RegainControl", 0.75f);
            }

            otherRigidBody.AddForce(otherRigidBody.transform.position - m_ImpactPoint * m_ImpactForce, ForceMode.Impulse);
        }*/
        Unit hitUnit = i_ImpactCaughtCollider.transform.root.GetComponentInChildren<Unit>();
        if (hitUnit != null)
        {
            m_StatusEventBus.KnockbackAttemptEvent.Invoke(
                hitUnit, hitUnit.transform.position - m_ImpactPoint * m_ImpactForce, ForceMode.Impulse, m_ImpackKnockbackDuration
                );
        }
    }
    private void ApplyImpactDamage(Collider i_ImpactCaughtCollider)
    {
        if (i_ImpactCaughtCollider.transform.root != m_Caster.transform.root)
        {
            print("Meteor impact damaging: " + i_ImpactCaughtCollider.transform.root.name);

            m_DamageEventBus.DamageAttempt(
                new DamageDealer(m_Caster.getControlledUnit().Owner, m_Caster.getControlledUnit()),
                i_ImpactCaughtCollider.transform.root.gameObject,
                m_ImpactDamage,
                m_AbilityDamageType
                );
        }
    }

    private void MeteorRoll()
    {
        m_bMeteorFalling = false;
        m_bMeteorRolling = true;
        Invoke("EndMeteor", m_RollDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_bMeteorRolling)
        {
            if (other.transform.root != m_Caster.transform.root)
            {
                print("Meteor rolled over: " + other.transform.root.name);

                m_DamageEventBus.DamageAttempt(
                new DamageDealer(m_Caster.getControlledUnit().Owner, m_Caster.getControlledUnit()),
                other.transform.root.gameObject,
                m_RollDamage,
                m_AbilityDamageType
                );
            }
        }
    }

    private void EndMeteor()
    {
        print("Ending meteor: " + transform.root.name);
        AbilityEnd();
    }
}
