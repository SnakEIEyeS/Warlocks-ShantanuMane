using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageInstance
{
    IDamageDealer DamageDealer { get; set; }
    float DamageAmount { get; set; }
    DamageType DamageType { get; set; }
    IDamageable Damageable { get; set; }

}
