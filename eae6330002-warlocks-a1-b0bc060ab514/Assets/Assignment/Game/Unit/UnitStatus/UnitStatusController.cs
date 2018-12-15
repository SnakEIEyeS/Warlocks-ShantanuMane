using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class UnitStatusController : MonoBehaviour, IUnitStatusController {

    [SerializeField]
    private PhotonView m_PhotonView = null;

    [SerializeField]
    private Unit m_Unit = null;
    private IStatusAffectable m_UnitStatusAffectable = null;
    [SerializeField]
    private UnitController m_UnitController = null;
    [SerializeField]
    private StatusEventBus m_StatusEventBus = null;

    #region IUnitStatusController
    public IUnit Unit { get { return m_Unit; } }
    public IUnitController UnitController { get { return m_UnitController; } }
    public IStatusEventBus StatusEventBus { get { return m_StatusEventBus; } set { m_StatusEventBus = value as StatusEventBus; } }
    #endregion

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
        
	}

    public void Init()
    {
        /*PhotonView AttachedPhotonView = GetComponentInChildren<PhotonView>();
        if (AttachedPhotonView)
        {
            m_StatusEventBus = (StatusEventBus)AttachedPhotonView.InstantiationData[4];
        }*/
        m_StatusEventBus = GameDataManager.Instance.StatusEventBus as StatusEventBus;

        if (m_Unit.GetComponentInChildren<IStatusAffectable>() != null)
        {
            m_UnitStatusAffectable = m_Unit.GetComponentInChildren<IStatusAffectable>();
        }
        m_StatusEventBus.InvisibilityEvent.AddListener(ControlInvisibility);
        m_StatusEventBus.SpellImmunityEvent.AddListener(ControlSpellImmunity);
        m_StatusEventBus.StunAttemptEvent.AddListener(StunAttempt);
        m_StatusEventBus.KnockbackAttemptEvent.AddListener(KnockbackAttempt);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Invisibility
    private void ControlInvisibility(Unit i_Unit, bool i_bInvis)
    {
        if(i_Unit == m_Unit)
        {
            if(i_bInvis)
            {
                //TurnInvisible();
                m_PhotonView.RPC("TurnInvisible", RpcTarget.All);
            }
            else
            {
                //EndInvisible();
                m_PhotonView.RPC("EndInvisible", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void TurnInvisible()
    {
        m_UnitStatusAffectable.SkinnedMeshRenderer.material = m_UnitStatusAffectable.InvisibilityMaterial;
        if (!m_PhotonView.IsMine)
        {
            m_Unit.gameObject.GetComponentInChildren<Renderer>().enabled = false;
            m_Unit.gameObject.GetComponentInChildren<UnitOverheadUI>().OverheadUICanvas.enabled = false;
        }
    }

    [PunRPC]
    private void EndInvisible()
    {
        m_UnitStatusAffectable.SkinnedMeshRenderer.material = m_UnitStatusAffectable.NormalMaterial;
        //if (m_PhotonView.IsMine)
        {
            m_Unit.gameObject.GetComponentInChildren<Renderer>().enabled = true;
            m_Unit.gameObject.GetComponentInChildren<UnitOverheadUI>().OverheadUICanvas.enabled = true;
        }
    }
    #endregion

    #region SpellImmunity
    private void ControlSpellImmunity(Unit i_Unit, bool i_bSpellImmune)
    {
        if(i_Unit == m_Unit)
        {
            if(i_bSpellImmune)
            {
                //ApplySpellImmunity();
                m_PhotonView.RPC("ApplySpellImmunity", RpcTarget.All);
            }
            else
            {
                //EndSpellImmunity();
                m_PhotonView.RPC("EndSpellImmunity", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void ApplySpellImmunity()
    {
        m_UnitStatusAffectable.SpellImmune = true;
        print(m_Unit.gameObject.name + " : Spell Immunity applied");
    }

    [PunRPC]
    private void EndSpellImmunity()
    {
        m_UnitStatusAffectable.SpellImmune = false;
        print(m_Unit.gameObject.name + " : Spell Immunity ended");
    }
    #endregion

    #region Stun
    //TODO handle case for multiple status effects on the unit
    private void StunAttempt(Unit i_Unit, float i_StunTime)
    {
        if(i_Unit == m_Unit && !m_UnitStatusAffectable.SpellImmune)
        {
            //ApplyStun();
            m_PhotonView.RPC("ApplyStun", m_Unit.UnitPhotonView.Owner);
            Invoke("EndStun", i_StunTime);
        }
    }

    [PunRPC]
    private void ApplyStun()
    {
        print("Aplying Stun to " + m_Unit);
        m_UnitStatusAffectable.MovementBlocked = true;
        m_UnitStatusAffectable.AbilityCastBlocked = true;
    }

    private void EndStun()
    {
        m_PhotonView.RPC("RPC_EndStun", m_Unit.UnitPhotonView.Owner);
    }
    
    [PunRPC]
    private void RPC_EndStun()
    {
        print("Ending Stun on " + m_Unit);
        m_UnitStatusAffectable.MovementBlocked = false;
        m_UnitStatusAffectable.AbilityCastBlocked = false;
    }
    #endregion

    #region Knockback
    private void KnockbackAttempt(Unit i_Unit, Vector3 i_Force, ForceMode i_ForceMode, float i_KnockbackTime)
    {
        if(i_Unit == m_Unit && !m_UnitStatusAffectable.SpellImmune)
        {
            m_PhotonView.RPC("ApplyKnockback", m_Unit.UnitPhotonView.Owner, i_Force, (byte)i_ForceMode, i_KnockbackTime);
            //ApplyKnockback(i_Force, i_ForceMode, i_KnockbackTime);
            Invoke("EndKnockback", i_KnockbackTime);
        }
    }

    [PunRPC]
    private void ApplyKnockback(Vector3 i_Force, byte i_ForceMode, float i_KnockbackTime)
    {
        ApplyStun();
        
        Rigidbody UnitRigidbody = m_Unit.gameObject.GetComponentInChildren<Rigidbody>();
        if (UnitRigidbody != null)
        {
            UnitRigidbody.isKinematic = false;
            UnitRigidbody.AddForce(i_Force, ForceMode.Impulse);
        }

        //Invoke("EndKnockback", i_KnockbackTime);
    }
    private void EndKnockback()
    {
        m_PhotonView.RPC("RPC_EndKnockBack", m_Unit.UnitPhotonView.Owner);
    }

    [PunRPC]
    private void RPC_EndKnockBack()
    {
        EndStun();

        Rigidbody UnitRigidbody = m_Unit.gameObject.GetComponentInChildren<Rigidbody>();
        if (UnitRigidbody != null)
        {
            UnitRigidbody.isKinematic = true;
        }
    }
    #endregion


}
