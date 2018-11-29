using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEventBus : MonoBehaviour, IStatusEventBus
{

    [SerializeField]
    private UnitInvisibility m_InvisibilityEvent = null;
    [SerializeField]
    private UnitSpellImmunity m_SpellImmunityEvent = null;
    [SerializeField]
    private UnitStunAttempt m_StunAttemptEvent = null;
    [SerializeField]
    private UnitStunApply m_StunApplyEvent = null;
    [SerializeField]
    private UnitStunEnd m_StunEndEvent = null;
    [SerializeField]
    private UnitKnockbackAttempt m_KnockbackAttemptEvent = null;
    [SerializeField]
    private UnitKnockbackApply m_KnockbackApplyEvent = null;
    [SerializeField]
    private UnitKnockbackEnd m_KnockbackEndEvent = null;

    #region IStatusEventBus
    public UnitInvisibility InvisibilityEvent { get { return m_InvisibilityEvent; } }
    public UnitSpellImmunity SpellImmunityEvent { get { return m_SpellImmunityEvent; } }
    public UnitStunAttempt StunAttemptEvent { get { return m_StunAttemptEvent; } }
    public UnitStunApply StunApplyEvent { get { return m_StunApplyEvent; } }
    public UnitStunEnd StunEndEvent { get { return m_StunEndEvent; } }
    public UnitKnockbackAttempt KnockbackAttemptEvent { get { return m_KnockbackAttemptEvent; } }
    public UnitKnockbackApply KnockbackApplyEvent { get { return m_KnockbackApplyEvent; } }
    public UnitKnockbackEnd KnockbackEndEvent { get { return m_KnockbackEndEvent; } }
    #endregion

}
