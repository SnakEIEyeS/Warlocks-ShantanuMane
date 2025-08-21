using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;

//Parameter list: Round PhotonViewID
[System.Serializable]
public class GamePreRound : UnityEvent<int>
{ }

[System.Serializable]
public class GameStoreClose : UnityEvent<Store>
{ }

//Parameter list: Round PhotonViewID, RoundWinner PhotonViewID
[System.Serializable]
public class GameRoundOutcome : UnityEvent<int, int>
{ }

//Parameter list: Round PhotonViewID
[System.Serializable]
public class GameRoundComplete : UnityEvent<int>
{ }
