using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStatistics
{
    float MoveSpeed { get; set; }
    float TurnRate { get; set; }
    float HealthRegen { get; set; }
}
