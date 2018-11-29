using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthHandler
{
    //TODO make this templated?
    GameObject Owner { get; set; }
    IHealth Health { get; set; }
    IDamageEventBus DamageEventBus { get; set; }
}
