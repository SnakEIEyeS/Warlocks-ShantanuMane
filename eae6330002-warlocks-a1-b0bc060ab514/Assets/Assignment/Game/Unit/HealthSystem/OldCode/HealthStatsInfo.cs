using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealthStatsInfo : IHealthStatsInfo {

    public IHealthSource Source { get; set; }

    public float Current { get; set; }
    public float Max { get; set; }

    public HealthStatsInfo(IHealthSource source, float current, float max) {
        Source = source;
        Current = current;
        Max = max;
    }

    public HealthStatsInfo(IHealthSource source, IHealth original, float newHealth) {
        Source = source;
        Current = newHealth;
        Max = original.Max;
    }

}
