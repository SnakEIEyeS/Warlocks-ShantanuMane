using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataManager
{
    IGame Game { get; set; }
    IGameEventBus GameEventBus { get; set; }
    AbilityFactory AbilityFactory { get; set; }
    IAbilityEventBus AbilityEventBus { get; set; }
    IStatusEventBus StatusEventBus { get; set; }
    IDamageEventBus DamageEventBus { get; set; }
    IKillEventBus KillEventBus{ get; set; }
    IKillManager KillManager { get; }
    IGoldEventBus GoldEventBus { get; }
    IGoldManager GoldManager { get; }
    Map GameMap { get; }
    Island GameIsland { get; }
    List<GameObject> SpawnPoints { get; set; }
    UnitDetailsHUD HeadUpDisplay { get; }
}
