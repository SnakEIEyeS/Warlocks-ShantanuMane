using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AwardGold : UnityEvent<IPlayer, int>
{ }

[System.Serializable]
public class DeductGold : UnityEvent<IPlayer, int>
{ }
