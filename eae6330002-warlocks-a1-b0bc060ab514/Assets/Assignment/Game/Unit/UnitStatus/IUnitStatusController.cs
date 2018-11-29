using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStatusController
{
    IUnit Unit { get; }
    IUnitController UnitController { get; }
    IStatusEventBus StatusEventBus { get; set; }
}
