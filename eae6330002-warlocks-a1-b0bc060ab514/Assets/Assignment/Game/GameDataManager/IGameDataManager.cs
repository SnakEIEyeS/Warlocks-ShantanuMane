using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataManager
{
    IGame Game { get; set; }
    IGameEventBus GameEventBus { get; set; }
    IKillEventBus KillEventBus{ get; set; }
    IKillManager KillManager { get; }
    IGoldEventBus GoldEventBus { get; }
    IGoldManager GoldManager { get; }
}
