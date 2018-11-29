using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageResolver : MonoBehaviour, IDamageResolver {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region IDamageResolver
    public void HandlePhysicalDamageAttempt(DamageInstance i_DamageInstance)
    { }

    public void HandleMagicDamageAttempt(DamageInstance i_DamageInstance)
    {
        float FinalMagicDamage = i_DamageInstance.DamageAmount * (1+ i_DamageInstance.DamageDealer.DamageInflicter.DamageHandler.MagicDamageAmplification);

        foreach(float MagicResistance in i_DamageInstance.Damageable.DamageHandler.MagicDamageResistance)
        {
            FinalMagicDamage *= (1 - MagicResistance);
        }
        foreach(float MagicResistanceReduction in i_DamageInstance.Damageable.DamageHandler.MagicResistanceReduction)
        {
            FinalMagicDamage *= (1 + MagicResistanceReduction);
        }

        i_DamageInstance.Damageable.DamageHandler.TakeDamage(FinalMagicDamage, DamageType.Magical, i_DamageInstance.DamageDealer as DamageDealer);
    }

    public void HandleLavaDamageAttempt(DamageInstance i_DamageInstance)
    {
        float FinalLavaDamage = i_DamageInstance.DamageAmount;

        foreach (float LavaResistance in i_DamageInstance.Damageable.DamageHandler.LavaDamageResistance)
        {
            FinalLavaDamage *= (1 - LavaResistance);
        }
        foreach (float LavaResistanceReduction in i_DamageInstance.Damageable.DamageHandler.LavaResistanceReduction)
        {
            FinalLavaDamage *= (1 + LavaResistanceReduction);
        }

        i_DamageInstance.Damageable.DamageHandler.TakeDamage(FinalLavaDamage, DamageType.Lava, i_DamageInstance.DamageDealer as DamageDealer);
    }
    #endregion
}
