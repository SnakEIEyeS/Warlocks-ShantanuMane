using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageHandler
{
    IDamageEventBus DamageEventBus { get; set; }
    #region PhysicalDamage
    #endregion

    #region MagicalDamage
    //TODO should this also be a list of amplifiers?
    float MagicDamageAmplification { get; set; }
    List<float> MagicDamageResistance { get; }
    List<float> MagicResistanceReduction { get; }

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    bool AddMagicDamageResistance(float i_PercentMagicDamageResistance);

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    bool AddMagicResistanceReduction(float i_PercentMagicResistanceReduction);
    #endregion

    #region LavaDamage
    List<float> LavaDamageResistance { get; }
    List<float> LavaResistanceReduction { get; }

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    bool AddLavaDamageResistance(float i_PercentLavaDamageResistance);

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    bool AddLavaResistanceReduction(float i_PercentLavaResistanceReduction);
    #endregion

    void TakeDamage(float DamageAmount, DamageType i_DamageType, DamageDealer i_DamageDealer);
}
