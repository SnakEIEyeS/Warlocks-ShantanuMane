using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class GoldManager : MonoBehaviour, IGoldManager {

    //[SerializeField]
    [SerializeField]
    private GameEventBus m_GameEventBus = null;
    private IGoldEventBus m_GoldEventBus = null;
    private Dictionary<IPlayer, int> m_GoldScoreboard = new Dictionary<IPlayer, int>();

    private IGame m_Game = null;
    private IGameDataManager m_GameDataManager = null;

    [SerializeField]
    private int m_StartingGold = 10;
    [SerializeField]
    private int m_RoundWinnerGold = 4;
    [SerializeField]
    private int m_RoundCompleteGold = 5;
    

    #region IGoldManager
    public IGame Game { get { return m_Game; } set { m_Game = value; } }
    public IGameDataManager GameDataManager { get { return m_GameDataManager; } set { m_GameDataManager = value; } }
    public IGoldEventBus GoldEventBus { get { return m_GoldEventBus; } set { m_GoldEventBus = value; } }
    public Dictionary<IPlayer, int> GoldScoreboard { get { return m_GoldScoreboard; } }

    public void Init(IGame i_Game, IGameDataManager i_GameDataManager, IGoldEventBus i_GoldEventBus)
    {
        m_Game = i_Game;
        m_GameDataManager = i_GameDataManager;
        m_GoldEventBus = i_GoldEventBus;
        InitGoldScoreboard();
    }
    #endregion
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitGoldScoreboard()
    {
        foreach(Player player in m_Game.PlayersInGame)
        {
            m_GoldScoreboard.Add(player, m_StartingGold);
        }

        //m_KillEventBus.DeathEvent.AddListener()
        m_GoldEventBus.AwardGold.AddListener(AwardPlayerGold);
        m_GoldEventBus.DeductGold.AddListener(DeductPlayerGold);

        m_GameEventBus.OnGameRoundOutcome.AddListener(AwardRoundWinner);
        m_GameEventBus.OnGameRoundComplete.AddListener(AwardRoundCompleteGold);
    }

    private void AwardPlayerGold(IPlayer i_Player, int i_GoldAmount)
    {
        m_GoldScoreboard[i_Player] += i_GoldAmount;
    }

    private void DeductPlayerGold(IPlayer i_Player, int i_GoldAmount)
    {
        m_GoldScoreboard[i_Player] -= i_GoldAmount;
    }

    private void AwardRoundWinner(int i_RoundPhotonViewID, int i_WinnerPhotonViewID)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            IPlayer RoundWinnerPlayer = PhotonView.Find(i_WinnerPhotonViewID).gameObject.GetComponentInChildren<IPlayer>();
            AwardPlayerGold(RoundWinnerPlayer, m_RoundWinnerGold);
        }
        
    }

    private void AwardRoundCompleteGold(int i_RoundPhotonViewID)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in m_Game.PlayersInGame)
            {
                AwardPlayerGold(player, m_RoundCompleteGold);
            }
        }
    }
}
