using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpellShieldAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility, IStatusEffectAbility
{
    [SerializeField]
    private PhotonView m_PhotonView = null;

    private PlayerController m_InstigatorController = null;
    private UnitController m_Caster = null;

    private AbilityTargetType m_TargetType = AbilityTargetType.NoTarget;
    private float m_CastTime = 0.5f;
    private float m_Cooldown = 20.0f;

    [SerializeField]
    private SphereCollider m_ShieldSphereCollider = null;
    private float m_SpellShieldDuration = 5.0f;
    private bool m_bSpellShieldActive = false;
    

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
        //SpellShield();
        m_PhotonView.RPC("SpellShield", PhotonNetwork.MasterClient);
    }
    public void AbilityEnd()
    {
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

    void Awake()
    {
        GameObject CasterGO = PhotonView.Find((int)m_PhotonView.InstantiationData[0]).gameObject;
        m_Caster = CasterGO.GetComponentInChildren<UnitController>();
    }

    // Use this for initialization
    void Start()
    {
        StatusEventBus = FindObjectOfType<StatusEventBus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bSpellShieldActive && m_PhotonView.IsMine)
        {
            this.transform.root.position = m_Caster.getControlledUnit().transform.position;
        }
    }

    [PunRPC]
    private void SpellShield()
    {
        IStatusAffectable CasterStatusAffectable = 
            m_Caster.getControlledUnit().transform.root.gameObject.GetComponentInChildren<IStatusAffectable>();
        if(CasterStatusAffectable != null)
        {
            m_StatusEventBus = GameDataManager.Instance.StatusEventBus as StatusEventBus;
            m_StatusEventBus.SpellImmunityEvent.Invoke(m_Caster.getControlledUnit(), true);
        }
        this.transform.position = m_Caster.getControlledUnit().transform.position;
        m_bSpellShieldActive = true;
        m_ShieldSphereCollider.enabled = true;

        Invoke("EndSpellShield", m_SpellShieldDuration);

    }

    private void EndSpellShield()
    {
        IStatusAffectable CasterStatusAffectable =
            m_Caster.getControlledUnit().transform.root.gameObject.GetComponentInChildren<IStatusAffectable>();
        if (CasterStatusAffectable != null)
        {
            m_StatusEventBus = GameDataManager.Instance.StatusEventBus as StatusEventBus;
            m_StatusEventBus.SpellImmunityEvent.Invoke(m_Caster.getControlledUnit(), false);
        }
        //this.transform.position = (m_AbilityPool as MonoBehaviour).transform.position;
        m_bSpellShieldActive = false;
        m_ShieldSphereCollider.enabled = false;

        AbilityEnd();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_bSpellShieldActive)
        {
            IAbility otherAbility = other.transform.root.GetComponentInChildren<IAbility>();
            if (otherAbility != null)
            {
                if(other.transform.root.GetComponentInChildren<IMovingAbility>() != null )
                    //&& otherAbility.Caster != this.Caster && otherAbility.Instigator != this.Instigator)
                {
                    //other.transform.root.forward = -other.transform.root.forward;
                    //print(other.transform.root.gameObject.name + " triggered by spell shield");

                    PhotonView OtherAbilityPhotonView = other.transform.root.gameObject.GetComponentInChildren<PhotonView>();
                    m_PhotonView.RPC("ReflectAbility", OtherAbilityPhotonView.Owner, OtherAbilityPhotonView.ViewID);
                }
                
            }
        }
    }

    [PunRPC]
    private void ReflectAbility(int i_OtherAbilityViewID)
    {
        GameObject OtherAbilityGO = PhotonView.Find(i_OtherAbilityViewID).gameObject;
        OtherAbilityGO.transform.forward = -(OtherAbilityGO.transform.forward);
        print(OtherAbilityGO.name + " triggered by spell shield");
    }

}
