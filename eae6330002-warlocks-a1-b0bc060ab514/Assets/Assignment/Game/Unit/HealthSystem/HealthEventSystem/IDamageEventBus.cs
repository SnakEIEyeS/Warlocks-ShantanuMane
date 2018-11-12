using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageEventBus
{
    IDamageResolver DamageResolver { get; set; }
    DamageAttempt DamageAttemptEvent { get; }
    DamageTaken DamageTakenEvent { get; }

    void DamageAttempt(DamageInstance i_DamageInstance);
    bool DamageAttempt(DamageDealer i_DamageDealer, GameObject i_DamageTarget, float i_DamageAmount, DamageType i_DamageType);
}
