using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusController : MonoBehaviour, IUnitStatusController {

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

    // Use this for initialization
    void Start () {
        if(m_Unit.GetComponentInChildren<IStatusAffectable>()!=null)
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
                TurnInvisible();
            }
            else
            {
                EndInvisible();
            }
        }
    }

    private void TurnInvisible()
    {
        m_UnitStatusAffectable.SkinnedMeshRenderer.material = m_UnitStatusAffectable.InvisibilityMaterial;
    }
    private void EndInvisible()
    {
        m_UnitStatusAffectable.SkinnedMeshRenderer.material = m_UnitStatusAffectable.NormalMaterial;
    }
    #endregion

    #region SpellImmunity
    private void ControlSpellImmunity(Unit i_Unit, bool i_bSpellImmune)
    {
        if(i_Unit == m_Unit)
        {
            if(i_bSpellImmune)
            {
                ApplySpellImmunity();
            }
            else
            {
                EndSpellImmunity();
            }
        }
    }

    private void ApplySpellImmunity()
    {
        m_UnitStatusAffectable.SpellImmune = true;
        print(m_Unit.gameObject.name + " : Spell Immunity applied");
    }
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
            ApplyStun();
            Invoke("EndStun", i_StunTime);
        }
    }
    private void ApplyStun()
    {
        print("Aplying Stun to " + m_Unit);
        m_UnitStatusAffectable.MovementBlocked = true;
        m_UnitStatusAffectable.AbilityCastBlocked = true;
    }
    private void EndStun()
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
            ApplyKnockback(i_Force, i_ForceMode, i_KnockbackTime);
            Invoke("EndKnockback", i_KnockbackTime);
        }
    }

    private void ApplyKnockback(Vector3 i_Force, ForceMode i_ForceMode, float i_KnockbackTime)
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
        EndStun();

        Rigidbody UnitRigidbody = m_Unit.gameObject.GetComponentInChildren<Rigidbody>();
        if (UnitRigidbody != null)
        {
            UnitRigidbody.isKinematic = true;
        }
    }
    #endregion


}
