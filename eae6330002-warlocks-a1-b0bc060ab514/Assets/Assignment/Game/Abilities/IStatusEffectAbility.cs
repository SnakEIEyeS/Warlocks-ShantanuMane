using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffectAbility
{
    IStatusEventBus StatusEventBus { get; set; }
}
