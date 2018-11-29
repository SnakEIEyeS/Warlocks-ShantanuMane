using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealer
{
    IPlayer InstigatorPlayer { get; set; }
    IUnit InstigatorUnit { get; set; }
    IDamageInflicter DamageInflicter { get; set; }
}
