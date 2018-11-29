using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoldEventBus
{
    AwardGold AwardGold { get; }
    DeductGold DeductGold { get; }
}
