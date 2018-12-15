using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DirectionTargeted : UnityEvent<UnitController, Vector3>
{ }

[System.Serializable]
public class PointTargeted : UnityEvent<UnitController, Vector3>
{ }

[System.Serializable]
public class TargetingCanceled : UnityEvent<UnitController>
{ }

[System.Serializable]
public class CastComplete : UnityEvent<UnitController>
{ }
