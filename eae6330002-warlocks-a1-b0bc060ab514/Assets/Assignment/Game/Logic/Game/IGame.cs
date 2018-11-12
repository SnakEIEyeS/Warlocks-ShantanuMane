using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
    List<Player> PlayersInGame { get; }
    List<Unit> UnitsInGame { get; }

    IGameDataManager GameDataManager { get; set; }
    IGameEventBus GameEventBus { get; }
}
