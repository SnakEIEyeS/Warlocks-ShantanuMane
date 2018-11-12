using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageAttempt : UnityEvent<DamageInstance>
{ }

[System.Serializable]
public class DamageTaken : UnityEvent<IDamageDealer, IDamageable, float, DamageType>
{ }
