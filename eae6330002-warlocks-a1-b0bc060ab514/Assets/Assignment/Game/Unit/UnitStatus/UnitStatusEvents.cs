using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitInvisibility : UnityEvent<Unit, bool>
{ }

[System.Serializable]
public class UnitSpellImmunity : UnityEvent<Unit, bool>
{ }

[System.Serializable]
public class UnitStunAttempt : UnityEvent<Unit, float>
{ }

[System.Serializable]
public class UnitStunApply : UnityEvent<Unit>
{ }

[System.Serializable]
public class UnitStunEnd : UnityEvent<Unit>
{ }

[System.Serializable]
public class UnitKnockbackAttempt : UnityEvent<Unit, Vector3, ForceMode, float>
{ }

[System.Serializable]
public class UnitKnockbackApply : UnityEvent<Unit>
{ }

[System.Serializable]
public class UnitKnockbackEnd : UnityEvent<Unit>
{ }
