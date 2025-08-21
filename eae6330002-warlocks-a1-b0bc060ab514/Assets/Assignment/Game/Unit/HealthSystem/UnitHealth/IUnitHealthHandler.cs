using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitHealthHandler : IHealthHandler
{
    IDamageHandler DamageHandler { get; set; }
}
