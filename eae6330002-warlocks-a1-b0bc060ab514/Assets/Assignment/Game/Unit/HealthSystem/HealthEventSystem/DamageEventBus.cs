using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEventBus : MonoBehaviour, IDamageEventBus
{
    [SerializeField]
    DamageResolver m_DamageResolver = null;
    [SerializeField]
    DamageAttempt m_DamageAttemptEvent = null;
    [SerializeField]
    DamageTaken m_DamageTakenEvent = null;

    #region IDamageEventBus
    public IDamageResolver DamageResolver { get { return m_DamageResolver; } set { m_DamageResolver = value as DamageResolver; } }
    public DamageAttempt DamageAttemptEvent { get { return m_DamageAttemptEvent; } }
    public DamageTaken DamageTakenEvent { get { return m_DamageTakenEvent; } }

    public void DamageAttempt(DamageInstance i_DamageInstance)
    {
        switch(i_DamageInstance.DamageType)
        {
            case DamageType.Physical:
                DamageResolver.HandlePhysicalDamageAttempt(i_DamageInstance);
                break;

            case DamageType.Magical:
                DamageResolver.HandleMagicDamageAttempt(i_DamageInstance);
                break;

            case DamageType.Lava:
                DamageResolver.HandleLavaDamageAttempt(i_DamageInstance);
                break;

            default:
                break;
        }
    }

    public bool DamageAttempt(DamageDealer i_DamageDealer, GameObject i_DamageTarget, float i_DamageAmount, DamageType i_DamageType)
    {
        IDamageable DamageableTarget = i_DamageTarget.transform.root.GetComponentInChildren<IDamageable>();
        if(DamageableTarget != null)
        {
            DamageInstance AttemptDamageInstance = 
                new DamageInstance(i_DamageDealer, DamageableTarget, i_DamageAmount, i_DamageType);

            switch (AttemptDamageInstance.DamageType)
            {
                case DamageType.Physical:
                    DamageResolver.HandlePhysicalDamageAttempt(AttemptDamageInstance);
                    break;

                case DamageType.Magical:
                    DamageResolver.HandleMagicDamageAttempt(AttemptDamageInstance);
                    break;

                case DamageType.Lava:
                    DamageResolver.HandleLavaDamageAttempt(AttemptDamageInstance);
                    break;

                default:
                    break;
            }
            return true;
        }

        return false;
    }
    #endregion


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
