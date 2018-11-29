using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEventBus
{
    UnitInvisibility InvisibilityEvent { get; }
    UnitSpellImmunity SpellImmunityEvent { get; }
    UnitStunAttempt StunAttemptEvent { get; }
    UnitStunApply StunApplyEvent { get; }
    UnitStunEnd StunEndEvent { get; }
    UnitKnockbackAttempt KnockbackAttemptEvent { get; }
    UnitKnockbackApply KnockbackApplyEvent { get; }
    UnitKnockbackEnd KnockbackEndEvent { get; }
}
