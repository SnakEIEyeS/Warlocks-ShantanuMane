using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillManager
{
    IGame Game { get; set; }
    IGameDataManager GameDataManager { get; set; }
    IKillEventBus KillEventBus { get; set; }

    //Dictionary<Player, int> KillScoreboard { get; }
    List<PlayerKillData> KillScoreList { get; }

    void Init(IGame i_Game, IGameDataManager i_GameDataManager, IKillEventBus i_KillEventBus);
}
