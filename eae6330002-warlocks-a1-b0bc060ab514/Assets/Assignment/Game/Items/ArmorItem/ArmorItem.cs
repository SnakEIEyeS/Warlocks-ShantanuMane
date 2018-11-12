using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : Item
{
    [SerializeField]
    private float m_MagicDamageResistance = 0.1f;
    [SerializeField]
    private float m_LavaDamageResistance = 0.1f;

    #region Item
    public override bool OnItemEquip()
    {
        IDamageHandler OwnerDamageHandler =
            (ItemOwner as MonoBehaviour).transform.root.GetComponentInChildren<IDamageHandler>();
        if (OwnerDamageHandler != null)
        {
            OwnerDamageHandler.AddMagicDamageResistance(m_MagicDamageResistance);
            OwnerDamageHandler.AddLavaDamageResistance(m_LavaDamageResistance);

            return true;
        }
        return false;
    }
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
