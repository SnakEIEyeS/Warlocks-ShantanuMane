using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class FireballAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility, 
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
    private float m_Cooldown = 5f;

    private float m_Speed = 20.0f;
    private float m_Duration = 3.0f;
    private float m_Size = 2.0f;
    private float m_CollisionDamage = 5.0f;
    private float m_CollisionForce = 150.0f;
    private float m_CollisionKnockbackTime = 0.2f;
    private bool m_bFireballShooting = false;

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
        //ShootFireball();
        m_PhotonView.RPC("ShootFireball", PhotonNetwork.MasterClient);
    }
    public void AbilityEnd()
    {
        m_bFireballShooting = false;
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
    void Start () {
        AbilityEventBus abilityEventBus = (AbilityEventBus)FindObjectOfType<AbilityEventBus>();
        abilityEventBus.OnDirectionTargeted.AddListener(SetDirection);

        StatusEventBus = FindObjectOfType<StatusEventBus>();
        m_DamageEventBus = FindObjectOfType<DamageEventBus>();

        m_MeshPhotonView.TransferOwnership(PhotonNetwork.MasterClient);
    }
	
	// Update is called once per frame
	void Update () {
		if(m_bFireballShooting && m_PhotonView.IsMine)
        {
            MoveProjectile();
        }
	}

    private void SetDirection(UnitController i_UnitController, Vector3 i_Direction)
    {
        if (m_Caster == i_UnitController)
        {
            //this.transform.root.forward = i_Direction;

            AbilityEventBus abilityEventBus = (AbilityEventBus)FindObjectOfType<AbilityEventBus>();
            abilityEventBus.OnDirectionTargeted.RemoveListener(SetDirection);

            m_PhotonView.RPC("RPC_SetFireballDirection", PhotonNetwork.MasterClient, i_Direction);
        }
    }

    [PunRPC]
    private void RPC_SetFireballDirection(Vector3 i_Direction)
    {
        this.transform.root.forward = i_Direction;
    }

    private void MoveProjectile()
    {
        this.transform.root.position += transform.root.forward * m_Speed * Time.deltaTime;
        //Debug.Log(gameObject.transform.position);
    }

    [PunRPC]
    private void ShootFireball()
    {
        Vector3 CasterPosition = m_Caster.getControlledUnit().transform.position;
        this.transform.root.position = new Vector3(CasterPosition.x, CasterPosition.y + 5.0f, CasterPosition.z);
        m_SphereCollider.radius = m_Size / 2.0f;
        m_AbilityMesh.transform.localScale = new Vector3(m_Size, m_Size, m_Size);

        m_bFireballShooting = true;
        Invoke("EndAbility", m_Duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_bFireballShooting)
        {
            if(other.transform.root.gameObject != m_Caster.transform.root.gameObject)
            {
                print("Fireball hit: " + other.transform.root.name);
                /*Health otherHealth = other.transform.root.GetComponentInChildren<Health>();
                if(otherHealth)
                {
                    //otherHealth.TakeDamage(m_CollisionDamage);
                    otherHealth.TakeDamage(new DamageInstance(
                    new DamageDealer(m_Caster.getControlledUnit().Owner, m_Caster.getControlledUnit()),
                    m_CollisionDamage)
                    );
                }*/

                m_DamageEventBus.DamageAttempt(
                    new DamageDealer(m_Caster.getControlledUnit().Owner, m_Caster.getControlledUnit()),
                    other.transform.root.gameObject,
                    m_CollisionDamage,
                    m_AbilityDamageType
                    );

                Unit hitUnit = other.transform.root.GetComponentInChildren<Unit>();
                if (hitUnit != null)
                {
                    m_StatusEventBus.KnockbackAttemptEvent.Invoke(
                        hitUnit, this.transform.root.forward * m_CollisionForce, ForceMode.Impulse, m_CollisionKnockbackTime
                        );
                }

                EndAbility();
                
            }
        }
    }

    private void EndAbility()
    {
        print("Destroying: " + transform.root.name);
        AbilityEnd();
    }
}
