using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInstantiableAbility {

    IObjectPool<GameObject> AbilityPool { get; set; }

}
