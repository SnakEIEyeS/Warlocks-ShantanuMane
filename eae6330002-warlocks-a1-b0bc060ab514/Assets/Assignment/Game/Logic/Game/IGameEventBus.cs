using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEventBus
{
    GamePreRound OnGamePreRound { get; }
    GameStoreClose OnStoreClose { get; }
    GameRoundOutcome OnGameRoundOutcome { get; }
    GameRoundComplete OnGameRoundComplete { get; }
}
