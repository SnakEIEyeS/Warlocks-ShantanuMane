using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastableAbility
{
    AbilityTargetType TargetType { get; }
    float CastTime { get; }
    float Cooldown { get; set; }

    void AbilityExecute();
    void AbilityEnd();
}
