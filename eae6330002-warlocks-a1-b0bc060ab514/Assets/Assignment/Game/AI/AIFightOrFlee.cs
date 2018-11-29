using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AIFightOrFlee
{
    float CalculateFightUtility();
    float CalculateFleeUtility();

    void Fight();
    void Flee();
}
