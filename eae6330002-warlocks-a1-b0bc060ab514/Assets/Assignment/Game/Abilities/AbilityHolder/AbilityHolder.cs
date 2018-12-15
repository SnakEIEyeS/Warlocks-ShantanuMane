using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityHolder : IAbilityHolder {

    [SerializeField]
    private GameObject m_Ability = null;
    
    private bool m_bOnCooldown = false;

    #region IAbilityHolder
    public GameObject Ability { get { return m_Ability; } set { m_Ability = value; } }
    public bool OnCooldown { get { return m_bOnCooldown; } set { m_bOnCooldown = value; } }

    public void StartCooldown()
    {
        OnCooldown = true;
        //invoke ResetCooldown
        //Invoke("ResetCooldown", m_Ability.transform.root.GetComponentInChildren<ICastableAbility>().Cooldown);
    }
    public void ResetCooldown() { OnCooldown = false; }
    #endregion

    
}
