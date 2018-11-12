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
    #endregion

    #region LavaDamage
    List<float> LavaDamageResistance { get; }
    List<float> LavaResistanceReduction { get; }
    #endregion

    void TakeDamage(float DamageAmount, DamageType i_DamageType, DamageDealer i_DamageDealer);
}
