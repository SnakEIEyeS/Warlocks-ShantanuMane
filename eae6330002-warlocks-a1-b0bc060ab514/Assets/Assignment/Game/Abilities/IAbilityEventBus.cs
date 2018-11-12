using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityEventBus
{

    DirectionTargeted OnDirectionTargeted { get; }
    PointTargeted OnPointTargeted { get; }
    TargetingCanceled OnTargetingCanceled { get; }
    CastComplete OnCastComplete { get; }

}
