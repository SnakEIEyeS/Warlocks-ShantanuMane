using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStatsEvent : IHealthEvent {

    public IHealth Target { get; set; }

    public float StartHealth { get; set; }
    public float EndHealth { get; set; }
    public float HealthChange { get; set; }

    public float StartMaxHealth { get; set; }
    public float EndMaxHealth { get; set; }

    public IHealthSource Source { get; set; }

    public HealthStatsEvent(IHealth target, IHealthStatsInfo stats) {
        Target = target;
        Source = stats.Source;

        StartHealth = target.Current;
        EndHealth = stats.Current;
        HealthChange = EndHealth - StartHealth;

        StartMaxHealth = target.Max;
        EndMaxHealth = stats.Max;
    }
	
}
