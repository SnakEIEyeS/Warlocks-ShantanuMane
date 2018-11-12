using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusAffectable
{
    #region Invisibility
    SkinnedMeshRenderer SkinnedMeshRenderer { get; }
    Material NormalMaterial { get; set; }
    Material InvisibilityMaterial { get; set; }
    #endregion

    #region SpellImmunity
    bool SpellImmune { get; set; }
    #endregion

    #region Movement
    bool MovementBlocked { get; set; }
    #endregion

    #region AbilityCast
    bool AbilityCastBlocked { get; set; }
    #endregion

}
