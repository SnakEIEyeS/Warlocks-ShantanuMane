using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStatisticsController
{
    float MoveSpeed { get; }
    List<float> RawMoveSpeedModifiers { get; }
    List<float> PercentMoveSpeedModifiers { get; }

    bool AddRawMoveSpeedModifier(float i_RawMoveSpeedModifier);
    
    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    bool AddPercentMoveSpeedModifier(float i_PercentMoveSpeedModifier);

    float TurnRate { get; }
    List<float> RawTurnRateModifiers { get; }
    List<float> PercentTurnRateModifiers { get; }

    bool AddRawTurnRateModifier(float i_RawTurnRateModifier);

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    bool AddPercentTurnRateModifier(float i_PercentTurnRateModifier);
    
    float HealthRegen { get; }
    List<float> RawHealthRegenModifiers { get; }
    List<float> PercentHealthRegenModifiers { get; }

    bool AddRawHealthRegenModifier(float i_RawHealthRegenModifier);

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    bool AddPercentHealthRegenModifier(float i_PercentHealthRegenModifier);

}
