using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagingAbility
{
    DamageType AbilityDamageType { get; set; }
    IDamageEventBus DamageEventBus { get; set; }
}
