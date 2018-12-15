using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour, IGameDataManager {

    public static GameDataManager Instance;

    [SerializeField]
    private Game m_Game = null;
    [SerializeField]
    private GameEventBus m_GameEventBus = null;
    [SerializeField]
    private AbilityFactory m_AbilityFactory = null;
    [SerializeField]
    private AbilityEventBus m_AbilityEventBus = null;
    [SerializeField]
    private StatusEventBus m_StatusEventBus = null;
    [SerializeField]
    private DamageEventBus m_DamageEventBus = null;
    [SerializeField]
    private KillEventBus m_KillEventBus = null;
    [SerializeField]
    private KillManager m_KillManager = null;
    [SerializeField]
    private GoldEventBus m_GoldEventBus = null;
    [SerializeField]
    private GoldManager m_GoldManager = null;
    [SerializeField]
    private Map m_GameMap = null;
    [SerializeField]
    private Island m_Island = null;
    [SerializeField]
    private List<GameObject> m_SpawnPoints = new List<GameObject>();
    [SerializeField]
    private UnitDetailsHUD m_HeadUpDisplay = null;

    #region IGameDataManager
    public IGame Game { get { return m_Game; } set { m_Game = value as Game; } }
    public IGameEventBus GameEventBus { get { return m_GameEventBus; } set { m_GameEventBus = value as GameEventBus; } }
    public AbilityFactory AbilityFactory { get { return m_AbilityFactory; } set { m_AbilityFactory = value; } }
    public IAbilityEventBus AbilityEventBus { get { return m_AbilityEventBus; } set { m_AbilityEventBus = value as AbilityEventBus; } }
    public IStatusEventBus StatusEventBus { get { return m_StatusEventBus; } set { m_StatusEventBus = value as StatusEventBus; } }
    public IDamageEventBus DamageEventBus { get { return m_DamageEventBus; } set { m_DamageEventBus = value as DamageEventBus; } }
    public IKillEventBus KillEventBus { get { return m_KillEventBus; } set { m_KillEventBus = value as KillEventBus; } }
    public IKillManager KillManager { get { return m_KillManager; } }
    public IGoldEventBus GoldEventBus { get { return m_GoldEventBus; } set { m_GoldEventBus = value as GoldEventBus;} }
    public IGoldManager GoldManager { get { return m_GoldManager; } }
    public Map GameMap { get { return m_GameMap; } }
    public Island GameIsland { get { return m_Island; } }
    public List<GameObject> SpawnPoints { get { return m_SpawnPoints; } set { m_SpawnPoints = value; } }
    public UnitDetailsHUD HeadUpDisplay { get { return m_HeadUpDisplay; } }
    #endregion

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Use this for initialization
    void Start () {
        
	}
	
    public void Init()
    {
        m_KillManager.Init(m_Game, this, m_KillEventBus);
        m_GoldManager.Init(m_Game, this, m_GoldEventBus);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
