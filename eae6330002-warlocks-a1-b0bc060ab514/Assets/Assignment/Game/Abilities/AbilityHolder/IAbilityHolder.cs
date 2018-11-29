using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityHolder
{
    GameObject Ability { get; set; }
    bool OnCooldown { get; set; }

    void StartCooldown();
    void ResetCooldown();
}
