using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDamageHandler : MonoBehaviour, IDamageHandler
{
    [SerializeField]
    private Unit m_UnitOwner = null;

    private DamageEventBus m_DamageEventBus = null;
    public IDamageEventBus DamageEventBus { get { return m_DamageEventBus; } set { m_DamageEventBus = value as DamageEventBus; } }

    // Use this for initialization
	void Start () {
        m_DamageEventBus = FindObjectOfType<DamageEventBus>();

        m_UnitOwner = this.transform.root.GetComponentInChildren<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region PhysicalDamage
    #endregion

    #region MagicalDamage
    private float m_MagicDamageAmplification = 0.0f;
    public float MagicDamageAmplification { get { return m_MagicDamageAmplification; } set { m_MagicDamageAmplification = value; } }

    private List<float> m_MagicDamageResistance = new List<float>();
    public List<float> MagicDamageResistance { get { return m_MagicDamageResistance; } }

    private List<float> m_MagicResistanceReduction = new List<float>();
    public List<float> MagicResistanceReduction { get { return m_MagicResistanceReduction; } }
    #endregion

    #region LavaDamage
    private List<float> m_LavaDamageResistance = new List<float>();
    public List<float> LavaDamageResistance { get { return m_LavaDamageResistance; } }

    private List<float> m_LavaResistanceReduction = new List<float>();
    public List<float> LavaResistanceReduction { get { return m_LavaResistanceReduction; } }
    #endregion

    public void TakeDamage(float i_DamageAmount, DamageType i_DamageType, DamageDealer i_DamageDealer)
    {
        switch(i_DamageType)
        {
            case DamageType.Physical:
                break;
            case DamageType.Magical:
                if(!m_UnitOwner.SpellImmune)
                {
                    print((i_DamageDealer.InstigatorUnit as MonoBehaviour).gameObject.name + " hit " + m_UnitOwner.gameObject.name 
                        + " for " + i_DamageAmount + " Magic Damage");
                    DamageEventBus.DamageTakenEvent.Invoke(i_DamageDealer, m_UnitOwner, i_DamageAmount, i_DamageType);
                    m_UnitOwner.LastDamageDealer = i_DamageDealer;
                }
                break;
            case DamageType.Lava:
                break;
            default:
                break;
        }
    }


}
