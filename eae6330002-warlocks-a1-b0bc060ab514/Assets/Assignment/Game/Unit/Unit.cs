using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Unit : MonoBehaviour, IUnit, IUnitStatistics, IInventoryHolder,
    IDamageable, IDamageInflicter, IStatusAffectable {

    #region Unit
    [SerializeField]
    private string m_UnitName = null;
    [SerializeField]
    private Player m_Owner = null;
    [SerializeField]
    private UnitController m_UnitController = null;
    [SerializeField]
    private UnitHealthHandler m_UnitHealthHandler = null;
    public string Name { get { return m_UnitName; } set { m_UnitName = value; } }
    public Player Owner { get { return m_Owner; } set { m_Owner = value; } }
    public IUnitHealthHandler UnitHealthHandler { get { return m_UnitHealthHandler; } set { m_UnitHealthHandler = value as UnitHealthHandler; } }
    public IHealth Health { get { return UnitHealthHandler.Health; } }
    public IUnitController Controller { get { return m_UnitController; } set { m_UnitController = (UnitController)value; } }
    #endregion

    #region UnitStatistics
    [SerializeField]
    private UnitStatisticsController m_UnitStatisticsController = null;

    [SerializeField]
    private float m_MoveSpeedBase = 30f;
    //TODO figure out how set should work
    public float MoveSpeed
    {
        get
        {
            if (m_UnitStatisticsController) { return m_UnitStatisticsController.MoveSpeed; }
            else { print("Not Stats Controller: " + transform.root.name); return m_MoveSpeedBase; }
        }
        set { m_MoveSpeedBase = value; }
    }

    [SerializeField]
    private float m_TurnRateBase = 270f;//TODO figure out how set should work
    public float TurnRate
    {
        get
        {
            if (m_UnitStatisticsController) { return m_UnitStatisticsController.TurnRate; }
            else { print("Not Stats Controller: " + transform.root.name); return m_TurnRateBase; }
        }
        set { m_TurnRateBase = value; }
    }

    [SerializeField]
    private float m_HealthRegenBase = 0.0f;
    public float HealthRegen
    {
        get
        {
            if(m_UnitStatisticsController) { return m_UnitStatisticsController.HealthRegen; }
            else { print("Not Stats Controller: " + transform.root.name); return m_HealthRegenBase; }
        }
        set { m_HealthRegenBase = value; }
    }
    #endregion

    #region IInventoryHolder
    [SerializeField]
    private UnitInventory m_UnitInventory = null;
    public IInventory Inventory { get { return m_UnitInventory; } }
    #endregion

    [SerializeField]
    private Camera m_PortraitCamera = null;

    //[SerializeField]
    //private List<UnityEngine.Object> m_AbilityList = new List<UnityEngine.Object>();
    [SerializeField]
    private List<GameObject> m_AbilityList = new List<GameObject>();
    [SerializeField]
    private List<AbilityHolder> m_AbilityHolderList = new List<AbilityHolder>();

    [SerializeField]
    private SkinnedMeshRenderer m_MeshRenderer = null;
    private Material m_NormalMaterial = null;
    [SerializeField]
    private MeshRenderer m_MinimapMarkerCubeMesh = null;
    [SerializeField]
    private MeshRenderer m_MinimapMarkerSphereMesh = null;

    //[SerializeField]
    //private GameObject m_UnitSkeleton = null;
    [SerializeField]
    private CapsuleCollider m_CapsuleCollider = null;
    [SerializeField]
    private GameObject m_DeathRagdoll = null;

    [SerializeField]
    private GameObject m_SpawnPoint = null;

    private Round m_CurrentRound = null;

    //private bool bCanMove = true;

    #region IDamageable, IDamageInflicter
    private DamageDealer m_LastDamageDealer = null;
    public IDamageDealer LastDamageDealer { get { return m_LastDamageDealer; } set { m_LastDamageDealer = value as DamageDealer; } }

    public IDamageHandler DamageHandler { get { return m_UnitHealthHandler.DamageHandler; } }

    /*public void TakeDamage(IDamageInstance i_DamageInstance)
    {
        if (!IsDead)
        {
            if (i_DamageInstance.DamageDealer != null)
            {
                DamageDealer = i_DamageInstance.DamageDealer;
            }
            Current -= i_DamageInstance.DamageAmount;

            if (IsDead)
            {
                RelayDeath();
                FindObjectOfType<KillEventBus>().DeathEvent.Invoke(this, m_LastDamageDealer);
            }
        }
    }*/
    #endregion

    #region StatusEffects
    [SerializeField]
    private Material m_InvisMaterial = null;

    //Ability related effects
    private bool m_bSpellImmune = false;
    private bool m_bMovementBlocked = false;
    private bool m_bAbilityCastBlocked = false;
    #endregion

    #region IStatusAffectable
    public SkinnedMeshRenderer SkinnedMeshRenderer { get { return m_MeshRenderer; } }
    public Material NormalMaterial { get { return m_NormalMaterial; } set { m_NormalMaterial = value; } }
    public Material InvisibilityMaterial { get { return m_InvisMaterial; } set { m_InvisMaterial = value; } }
    public bool SpellImmune { get { return m_bSpellImmune; } set { m_bSpellImmune = value; } }
    public bool MovementBlocked { get { return m_bMovementBlocked; } set { m_bMovementBlocked = value; } }
    public bool AbilityCastBlocked { get { return m_bAbilityCastBlocked; } set { m_bAbilityCastBlocked = value; } }
    #endregion

    void Start()
    {
        if(m_Owner)
        {
            setUnitColor(m_Owner.Color);
            setMinimapMarkerColor(m_Owner.Color);
        }
        NormalMaterial = m_MeshRenderer.material;

        m_UnitStatisticsController.Init(this, m_MoveSpeedBase, m_TurnRateBase);
    }

    void Update()
    {
        
    }

    private void setUnitColor(Color i_Color)
    {
        m_MeshRenderer.material.SetColor("_Color", i_Color);
        //m_MeshRenderer.materials[0].SetColor("_Color", i_Color);
        m_DeathRagdoll.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", i_Color);
    }

    private void setMinimapMarkerColor(Color i_Color)
    {
        m_MinimapMarkerCubeMesh.material.SetColor("_Color", i_Color);
        m_MinimapMarkerSphereMesh.material.SetColor("_Color", i_Color);
    }

    public Health getHealth()
    {
        return m_UnitHealthHandler.Health as Health;
    }

    public Camera getPortraitCamera()
    {
        return m_PortraitCamera;
    }

    //public List<UnityEngine.Object> getAbilityList()
    //{ return m_AbilityList; }
    public List<GameObject> getAbilityList()
    { return m_AbilityList; }
    public List<AbilityHolder> getAbilityHolderList()
    { return m_AbilityHolderList; }

    //Spawn self at appropriate location after death
    public void SpawnSelf()
    {
        gameObject.transform.position = m_SpawnPoint.transform.position;

        //setCanMove(true);
        m_UnitHealthHandler.enabled = true;

        //TODO make HealEventBus and trigger a HealEvent
        //m_UnitHealthHandler.Health.Current = m_UnitHealthHandler.Health.Max;

        gameObject.GetComponentInChildren<UnitOverheadUI>().enabled = true;
        //GetComponentInChildren<Ragdoll>().getAliveParent().gameObject.SetActive(true);
        //GetComponentInChildren<Ragdoll>().AlignRagdoll();
        //m_DeathRagdoll.SetActive(false);
        m_CapsuleCollider.enabled = true;

        //Unblock Unit Action
        m_bMovementBlocked = false;
        m_bAbilityCastBlocked = false;
    }

    //Handle unit death
    public void UnitDeath()
    {
        m_CapsuleCollider.enabled = false;
        //m_UnitSkeleton.SetActive(true);
        //m_DeathRagdoll.SetActive(true);
        //GetComponentInChildren<Ragdoll>().AlignRagdoll();
        //GetComponentInChildren<Ragdoll>().getAliveParent().gameObject.SetActive(false);
        //gameObject.GetComponentInChildren<UnitOverheadUI>().enabled = false;
        m_UnitHealthHandler.enabled = false;
        //setCanMove(false);
        m_UnitController.StopAll();

        //Block Unit Action
        m_bMovementBlocked = true;
        m_bAbilityCastBlocked = true;

        m_Owner.getRoundInstance().RegisterUnitDeath(this);
    }

    //Currently used for setting a stunned state
    /*public bool getCanMove()
    { return bCanMove; }
    public void setCanMove(bool i_CanMove)
    { bCanMove = i_CanMove; }*/
}
