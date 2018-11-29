using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    IUnit ItemOwner { get; set; }
    bool OnItemEquip();
}
