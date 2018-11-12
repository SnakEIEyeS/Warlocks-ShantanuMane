﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility {
    //TODO change Instigator to be Player and not PlayerCtrlr
    IPlayerController Instigator { get; set; }
    IUnitController Caster { get; set; }

    //AbilityTargetType TargetType { get; }
    //float Cooldown { get; set; }

    //void AbilityExecute();
    //void AbilityEnd();
}
