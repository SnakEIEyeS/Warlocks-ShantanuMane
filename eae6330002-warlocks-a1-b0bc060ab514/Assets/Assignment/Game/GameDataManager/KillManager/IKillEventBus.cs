using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillEventBus
{
    DeathEvent DeathEvent { get; }
}
