using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageResolver
{
    void HandlePhysicalDamageAttempt(DamageInstance i_DamageInstance);
    void HandleMagicDamageAttempt(DamageInstance i_DamageInstance);
    void HandleLavaDamageAttempt(DamageInstance i_DamageInstance);
}
