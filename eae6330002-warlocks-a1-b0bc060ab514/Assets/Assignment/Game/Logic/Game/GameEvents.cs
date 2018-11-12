using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GamePreRound : UnityEvent<Round>
{ }

[System.Serializable]
public class GameStoreClose : UnityEvent<Store>
{ }

[System.Serializable]
public class GameRoundOutcome : UnityEvent<Round>
{ }

[System.Serializable]
public class GameRoundComplete : UnityEvent<Round>
{ }
