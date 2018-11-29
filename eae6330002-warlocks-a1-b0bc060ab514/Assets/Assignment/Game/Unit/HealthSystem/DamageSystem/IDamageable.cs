using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    IDamageDealer LastDamageDealer { get; set; }
    IDamageHandler DamageHandler { get; }
    //void TakeDamage(IDamageInstance i_DamageInstance);
}
