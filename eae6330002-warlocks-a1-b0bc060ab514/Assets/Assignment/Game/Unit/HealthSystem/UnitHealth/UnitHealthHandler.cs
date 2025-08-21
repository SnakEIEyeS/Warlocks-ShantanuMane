using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class UnitHealthHandler : MonoBehaviour, IPunObservable, IUnitHealthHandler
{
    [SerializeField]
    private PhotonView m_PhotonView = null;

    [SerializeField]
    private GameObject m_Owner = null;
    [SerializeField]
    private Health m_Health = null;
    private DamageEventBus m_DamageEventBus = null;
    [SerializeField]
    private UnitDamageHandler m_DamageHandler = null;

    #region IUnitHealthHandler
    public GameObject Owner { get { return m_Owner; } set { m_Owner = value; } }
    public IHealth Health { get { return m_Health; } set { m_Health = value as Health; } }
    public IDamageEventBus DamageEventBus { get { return m_DamageEventBus; } set { m_DamageEventBus = value as DamageEventBus; } }
    public IDamageHandler DamageHandler { get { return m_DamageHandler; } set { m_DamageHandler = value as UnitDamageHandler; } }
    #endregion


    // Use this for initialization
    void Start () {
        m_DamageEventBus = FindObjectOfType<DamageEventBus>();
        if(m_DamageEventBus)
        {
            //m_DamageEventBus.DamageAttemptEvent.AddListener(HandleDamageAttempt);
            m_DamageEventBus.DamageTakenEvent.AddListener(HandleDamageTaken);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void HandleDamageTaken(IDamageDealer i_DamageDealer, IDamageable i_Damageable, float i_DamageAmount, DamageType i_DamageType)
    {
        if(i_Damageable.Equals(m_Owner) || (i_Damageable as MonoBehaviour).gameObject == m_Owner)
        {
            //m_Health.Current -= i_DamageAmount;
            if (m_PhotonView.IsMine)
            {
                TakeDamage(i_DamageAmount);
            }
            else
            {
                m_PhotonView.RPC("TakeDamage", m_PhotonView.Owner, i_DamageAmount);
            }
        }
    }

    [PunRPC]
    private void TakeDamage(float i_DamageAmount)
    {
        m_Health.Current -= i_DamageAmount;
        if(m_Health.Current < 0.0f)
        {
            m_Health.Current = 0.0f;
        }
    }

    public void RefillHealthOnSpawn()
    {
        //m_Health.Current = m_Health.Max;
        if(m_PhotonView.IsMine)
        {
            Heal(m_Health.Max);
        }
        else
        {
            m_PhotonView.RPC("Heal", m_PhotonView.Owner, m_Health.Max);
        }
    }

    [PunRPC]
    private void Heal(float i_HealAmount)
    {
        m_Health.Current += i_HealAmount;
        if(m_Health.Current > m_Health.Max)
        {
            m_Health.Current = m_Health.Max;
        }
    }

    /*public void HandleDamageAttempt(DamageInstance i_DamageInstance)
    {
        switch (i_DamageInstance.DamageType)
        {
            case DamageType.Magical:
                HandleMagicDamageAttempt(i_DamageInstance);
                break;

            case DamageType.Lava:
                HandleLavaDamageAttempt(i_DamageInstance);
                break;
        }
    }
    private void HandleMagicDamageAttempt(DamageInstance i_DamageInstance)
    { }

    private void HandleLavaDamageAttempt(DamageInstance i_DamageInstance)
    { }*/


    #region IPunObservable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(m_Health.Current);
        }
        else
        {
            m_Health.Current = (float)stream.ReceiveNext();
        }
    }

    #endregion
}
