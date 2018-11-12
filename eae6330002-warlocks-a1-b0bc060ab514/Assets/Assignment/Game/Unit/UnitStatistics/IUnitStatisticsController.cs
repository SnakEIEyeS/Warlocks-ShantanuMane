using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStatisticsController
{
    float MoveSpeed { get; }
    List<float> RawMoveSpeedModifiers { get; }
    List<float> PercentMoveSpeedModifiers { get; }

    float TurnRate { get; }
    List<float> RawTurnRateModifiers { get; }
    List<float> PercentTurnRateModifiers { get; }
    
    float HealthRegen { get; }
    List<float> RawHealthRegenModifiers { get; }
    List<float> PercentHealthRegenModifiers { get; }

}
