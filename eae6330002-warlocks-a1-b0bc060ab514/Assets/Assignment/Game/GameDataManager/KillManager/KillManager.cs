using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillManager : MonoBehaviour, IKillManager {

    //[SerializeField]
    private IKillEventBus m_KillEventBus = null;
    //[SerializeField]
    //private Dictionary<Player, int> m_KillScoreboard = new Dictionary<Player, int>();
    [SerializeField]
    private List<PlayerKillData> m_KillScoreList = new List<PlayerKillData>();

    //[SerializeField]
    private IGame m_Game = null;
    private IGameDataManager m_GameDataManger = null;

    [SerializeField]
    private int m_KillGold = 2;

    #region IKillManager
    public IGame Game { get { return m_Game; } set { m_Game = value; } }
    public IGameDataManager GameDataManager { get { return m_GameDataManger; } set { m_GameDataManger = value; } }
    public IKillEventBus KillEventBus { get { return m_KillEventBus; } set { m_KillEventBus = value; } }

    //public Dictionary<Player, int> KillScoreboard { get { return m_KillScoreboard; } }
    public List<PlayerKillData> KillScoreList { get { return m_KillScoreList; } }

    public void Init(IGame i_Game, IGameDataManager i_GameDataManager, IKillEventBus i_KillEventBus)
    {
        m_Game = i_Game;
        m_GameDataManger = i_GameDataManager;
        m_KillEventBus = i_KillEventBus;
        InitKillScoreboard();
    }
    #endregion

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitKillScoreboard()
    {
        foreach(Player player in m_Game.PlayersInGame)
        {
            //m_KillScoreboard.Add(player, 0);
            m_KillScoreList.Add(new PlayerKillData(player, 0));
        }

        m_KillEventBus.DeathEvent.AddListener(RegisterKill);
    }

    private void RegisterKill(IHealth i_Health, IDamageDealer i_DamageDealer)
    {
        //m_KillScoreboard[i_DamageDealer.InstigatorPlayer as Player]++;
        int KillingPlayerIndex = m_KillScoreList.FindIndex(x => x.m_Player == i_DamageDealer.InstigatorPlayer as Player);
        m_KillScoreList[KillingPlayerIndex].m_Kills++;

        FindObjectOfType<GoldEventBus>().AwardGold.Invoke(i_DamageDealer.InstigatorPlayer, m_KillGold);
    }
}
