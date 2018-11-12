using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : MonoBehaviour, IAbility, IMovingAbility {

    [SerializeField]
    private SphereCollider m_SphereCollider = null;
    [SerializeField]
    private PlayerController m_InstigatorController = null;
    [SerializeField]
    private UnitController m_Caster = null;

    #region IAbility
    public IPlayerController Instigator
    {
        get { return m_InstigatorController; }
        set { m_InstigatorController = value as PlayerController; }
    }
    public IUnitController Caster
    {
        get { return m_Caster; }
        set { m_Caster = value as UnitController; }
    }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += this.transform.forward * 10 * Time.deltaTime;
	}


}
