using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageEvent : IDamageEvent {

    public IHealth Target { get; set; }

    public float Amount { get; set; }
    public DamageType DamageType { get; set; }

    public float StartHealth { get; set; }
    public float EndHealth { get; set; }
    public float HealthChange { get; set; }

    public float StartMaxHealth { get; set; }
    public float EndMaxHealth { get; set; }

    public IHealthSource Source { get; set; }
    public IDamageSource DamageSource {
        get { return Source as IDamageSource; }
        set { Source = value; }
    }

    public DamageEvent(IHealth target, IDamageInfo damageInfo) {
        Target = target;
        Source = damageInfo.Source;
        Amount = damageInfo.Amount;
        DamageType = damageInfo.DamageType;
        StartHealth = target.Current;
        EndHealth = target.Current;
        HealthChange = 0;
        StartMaxHealth = target.Max;
        EndMaxHealth = target.Max;
    }

}