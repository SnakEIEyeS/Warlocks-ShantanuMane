using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour, IGameDataManager {

    [SerializeField]
    private Game m_Game = null;
    [SerializeField]
    private GameEventBus m_GameEventBus = null;
    [SerializeField]
    private KillEventBus m_KillEventBus = null;
    [SerializeField]
    private KillManager m_KillManager = null;
    [SerializeField]
    private GoldEventBus m_GoldEventBus = null;
    [SerializeField]
    private GoldManager m_GoldManager = null;
    
    #region IGameDataManager
    public IGame Game { get { return m_Game; } set { m_Game = value as Game; } }
    public IGameEventBus GameEventBus { get { return m_GameEventBus; } set { m_GameEventBus = value as GameEventBus; } }
    public IKillEventBus KillEventBus { get { return m_KillEventBus; } set { m_KillEventBus = value as KillEventBus; } }
    public IKillManager KillManager { get { return m_KillManager; } }
    public IGoldEventBus GoldEventBus { get { return m_GoldEventBus; } set { m_GoldEventBus = value as GoldEventBus;} }
    public IGoldManager GoldManager { get { return m_GoldManager; } }
    #endregion

    // Use this for initialization
    void Start () {
        m_KillManager.Init(m_Game, this, m_KillEventBus);
        m_GoldManager.Init(m_Game, this, m_GoldEventBus);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
