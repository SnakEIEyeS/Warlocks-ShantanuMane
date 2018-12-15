using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class StealthAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility, IStatusEffectAbility
{
    [SerializeField]
    private PhotonView m_PhotonView = null;

    private PlayerController m_InstigatorController = null;
    private UnitController m_Caster = null;

    private AbilityTargetType m_TargetType = AbilityTargetType.NoTarget;
    private float m_CastTime = 0.0f;
    private float m_Cooldown = 20.0f;

    private float m_FadeTime = 1.0f;
    private float m_Duration = 5.0f;

    private IObjectPool<GameObject> m_AbilityPool = null;
    private IStatusEventBus m_StatusEventBus = null;

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
        print(m_Caster.getControlledUnit() + " Stealth!");
        Invoke("TriggerInvisibility", m_FadeTime);
    }
    public void AbilityEnd()
    {
        StopListeningForInvisBreak();
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
    public IStatusEventBus StatusEventBus { get { return m_StatusEventBus; } set { m_StatusEventBus = value; } }
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
        
    }

    private void TriggerInvisibility()
    {
        ListenForInvisBreakActions();

        /*if (m_StatusEventBus == null)
        {
            m_StatusEventBus = FindObjectOfType<StatusEventBus>();
        }
        m_StatusEventBus.InvisibilityEvent.Invoke(m_Caster.getControlledUnit(), true);

        Invoke("EndInvisibility", m_Duration);*/

        m_PhotonView.RPC("RPC_TriggerInvisibility", PhotonNetwork.MasterClient);
    }

    [PunRPC]
    private void RPC_TriggerInvisibility()
    {
        m_StatusEventBus = GameDataManager.Instance.StatusEventBus as StatusEventBus;
        m_StatusEventBus.InvisibilityEvent.Invoke(m_Caster.getControlledUnit(), true);

        Invoke("EndInvisibility", m_Duration);
    }

    private void BreakInvisibility(UnitController i_UnitController)
    {
        if(i_UnitController == m_Caster)
        {
            EndInvisibility();
        }
    }

    private void EndInvisibility()
    {
        /*if (m_StatusEventBus == null)
        {
            m_StatusEventBus = FindObjectOfType<StatusEventBus>();
        }
        m_StatusEventBus.InvisibilityEvent.Invoke(m_Caster.getControlledUnit(), false);
        CancelInvoke("EndInvisibility");

        AbilityEnd();*/
        m_PhotonView.RPC("RPC_EndInvisibility", PhotonNetwork.MasterClient);
    }

    [PunRPC]
    private void RPC_EndInvisibility()
    {
        m_StatusEventBus = GameDataManager.Instance.StatusEventBus as StatusEventBus;
        m_StatusEventBus.InvisibilityEvent.Invoke(m_Caster.getControlledUnit(), false);
        CancelInvoke("EndInvisibility");

        AbilityEnd();
    }

    private void ListenForInvisBreakActions()
    {
        AbilityEventBus abilityEventBus = GameDataManager.Instance.AbilityEventBus as AbilityEventBus;
        abilityEventBus.OnCastComplete.AddListener(BreakInvisibility);
    }

    private void StopListeningForInvisBreak()
    {
        AbilityEventBus abilityEventBus = GameDataManager.Instance.AbilityEventBus as AbilityEventBus;
        abilityEventBus.OnCastComplete.RemoveListener(BreakInvisibility);
    }

    private void EndAbility()
    {
        AbilityEnd();
    }
}
