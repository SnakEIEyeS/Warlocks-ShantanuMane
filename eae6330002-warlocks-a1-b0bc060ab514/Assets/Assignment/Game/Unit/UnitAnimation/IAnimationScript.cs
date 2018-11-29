using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationScript
{
    void SetMoving(bool i_bMoving);
    void SetCasting(bool i_bCasting);
    void SetDead(bool i_bDead);
}
