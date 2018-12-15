using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class TeleportAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility
{
    [SerializeField]
    private PhotonView m_PhotonView = null;
    
    private PlayerController m_InstigatorController = null;
    private UnitController m_Caster = null;

    private AbilityTargetType m_TargetType = AbilityTargetType.PointTarget;
    private float m_CastTime = 0.5f;
    private float m_Cooldown = 5f;

    private float m_MaxDistance = 15.0f;
    private Vector3 m_TeleportPoint = Vector3.zero;

    private IObjectPool<GameObject> m_AbilityPool = null;

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
        //Teleport();
        m_PhotonView.RPC("Teleport", m_Caster.getControlledUnit().UnitPhotonView.Owner);
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

    void Awake()
    {
        GameObject CasterGO = PhotonView.Find((int)m_PhotonView.InstantiationData[0]).gameObject;
        m_Caster = CasterGO.GetComponentInChildren<UnitController>();
    }

    // Use this for initialization
    void Start () {
        AbilityEventBus abilityEventBus = (AbilityEventBus)FindObjectOfType<AbilityEventBus>();

        //TODO You're not supposed to do this. There needs to be a cast animation that triggers the ability
        abilityEventBus.OnPointTargeted.AddListener(SetTeleportLocation);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetTeleportLocation(UnitController i_UnitController, Vector3 i_TargetPoint)
    {
        if (m_Caster == i_UnitController)
        {
            /*Vector3 CasterPosition = m_Caster.getControlledUnit().transform.position;
            if ((i_TargetPoint - CasterPosition).sqrMagnitude <= m_MaxDistance * m_MaxDistance)
            {
                m_TeleportPoint = i_TargetPoint;
            }
            else
            {
                m_TeleportPoint = CasterPosition + (i_TargetPoint - CasterPosition).normalized * m_MaxDistance;
            }*/
            m_PhotonView.RPC("RPC_SetTeleportLocation", PhotonNetwork.MasterClient, i_TargetPoint);
        }
    }

    [PunRPC]
    private void RPC_SetTeleportLocation(Vector3 i_TargetPoint)
    {
        //Vector3 CasterPosition = m_Caster.getControlledUnit().transform.position;
        Vector3 CasterPosition = m_Caster.getControlledUnit().UnitPhotonView.transform.position;
        if ((i_TargetPoint - CasterPosition).sqrMagnitude <= m_MaxDistance * m_MaxDistance)
        {
            m_TeleportPoint = i_TargetPoint;
        }
        else
        {
            m_TeleportPoint = CasterPosition + (i_TargetPoint - CasterPosition).normalized * m_MaxDistance;
        }
    }

    [PunRPC]
    void Teleport()
    {
        m_Caster.getControlledUnit().transform.position = m_TeleportPoint;

        AbilityEnd();
    }
}
