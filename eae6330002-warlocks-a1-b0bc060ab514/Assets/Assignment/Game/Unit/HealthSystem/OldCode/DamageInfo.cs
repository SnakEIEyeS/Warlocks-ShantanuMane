using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageInfo : IDamageInfo {

    public IDamageSource Source { get; set; }
    public float Amount { get; set; }
    public DamageType DamageType { get; set; }

    public DamageInfo(IDamageSource source, float amount, DamageType damageType) {
        Source = source;
        Amount = amount;
        DamageType = damageType;
    }
}
