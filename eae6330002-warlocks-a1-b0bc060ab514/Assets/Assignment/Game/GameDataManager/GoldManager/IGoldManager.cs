using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoldManager
{
    IGame Game { get; set; }
    IGameDataManager GameDataManager { get; set; }
    IGoldEventBus GoldEventBus { get; set; }
    Dictionary<IPlayer, int> GoldScoreboard { get; }

    void Init(IGame i_Game, IGameDataManager i_GameDataManager, IGoldEventBus i_GoldEventBus);
}
