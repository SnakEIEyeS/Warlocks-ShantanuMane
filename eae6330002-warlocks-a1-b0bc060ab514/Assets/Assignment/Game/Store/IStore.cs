using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStore
{
    IGameEventBus GameEventBus { get; }
}
