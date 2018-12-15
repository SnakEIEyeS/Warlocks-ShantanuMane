using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Game : MonoBehaviour, IGame {

    public static Game GameInstance;

    [SerializeField]
    private PhotonView m_GamePhotonView = null;
    public PhotonView GamePhotonView { get { return m_GamePhotonView; } }

    [SerializeField]
    private App app = null;

    [SerializeField]
    private GameDataManager m_GameDataManager = null;
    [SerializeField]
    private GameEventBus m_GameEventBus = null;

    [SerializeField]
    private Round m_RoundHandler = null;

    [SerializeField]
    private float m_RoundsToVictory = 2;

    [SerializeField]
    private List<Player> m_PlayersInGame = new List<Player>();
    [SerializeField]
    private List<Unit> m_UnitsInGame = new List<Unit>();

    [SerializeField]
    private Canvas m_GameUI = null;

    #region IGame
    public Round RoundInstance { get { return m_RoundHandler; } }
    public List<Player> PlayersInGame { get { return m_PlayersInGame; } }
    public List<Unit> UnitsInGame { get { return m_UnitsInGame; } }
    public IGameDataManager GameDataManager { get { return m_GameDataManager; } set { m_GameDataManager = value as GameDataManager; } }
    public IGameEventBus GameEventBus { get { return m_GameEventBus; } }
    #endregion

    private void Awake()
    {
        GameInstance = this;
    }

    private void Start()
    {
        m_GameEventBus.OnStoreClose.AddListener(OnStoreCloseStartRound);

        if(PhotonNetwork.IsMasterClient)
        {
            m_RoundHandler.OnPhaseChanged.AddListener(OnRoundPhaseChange);
        }
        
        //OnPreRound(m_RoundHandler);
        //m_RoundHandler.StartRound();

        //This is the one that was used and worked
        //m_RoundHandler.RestartRound();
    }

    private void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            Warlocks.GameManager.Instance.LeaveRoom();
        }
    }

    public void StartGame()
    {
        m_RoundHandler.RestartRound();
    }

    void OnRoundPhaseChange(Round i_Round)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            switch (i_Round.Phase)
            {
                case RoundPhase.PreRound:
                    OnPreRound(i_Round.RoundPhotonView.ViewID);
                    m_GamePhotonView.RPC("OnPreRound", RpcTarget.Others, i_Round.RoundPhotonView.ViewID);
                    break;
                case RoundPhase.InProgress:
                    OnRoundStart();
                    m_GamePhotonView.RPC("OnRoundStart", RpcTarget.Others);
                    break;
                case RoundPhase.Celebration:
                    OnRoundOutcome(i_Round.RoundPhotonView.ViewID, ((Player)i_Round.Winner).PlayerPhotonView.ViewID);
                    m_GamePhotonView.RPC("OnRoundOutcome", RpcTarget.Others, 
                        i_Round.RoundPhotonView.ViewID, ((Player)i_Round.Winner).PlayerPhotonView.ViewID);
                    break;
                case RoundPhase.Completed:
                    OnRoundCompleted(i_Round.RoundPhotonView.ViewID);
                    m_GamePhotonView.RPC("OnRoundCompleted", RpcTarget.Others, i_Round.RoundPhotonView.ViewID);
                    break;
            }
        }
        
    }

    [PunRPC]
    private void OnPreRound(int i_RoundPhotonViewID)
    {
        GameEventBus.OnGamePreRound.Invoke(i_RoundPhotonViewID);
        PrepUnits();
    }
    //Tell all units in the game to handle their spawning
    public void PrepUnits()
    {
        foreach(Unit unit in m_UnitsInGame)
        {
            unit.SpawnSelf();
        }
    }

    private void OnStoreCloseStartRound(Store i_Store)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            m_RoundHandler.StartRound();
        }
    }

    [PunRPC]
    private void OnRoundStart()
    {
        //m_GameUI.enabled = false;
    }

    [PunRPC]
    private void OnRoundOutcome(int i_RoundPhotonViewID, int i_RoundWinnerPhotonViewID)
    {
        m_GameEventBus.OnGameRoundOutcome.Invoke(i_RoundPhotonViewID, i_RoundWinnerPhotonViewID);
       // UpdatePostRoundUI();
        //m_GameUI.enabled = true;
    }
    public void UpdatePostRoundUI()
    {
        if (m_GameUI.GetComponentInChildren<GameUI>())
        {
            m_GameUI.GetComponentInChildren<GameUI>().UpdatePlayerRecords();
        }
    }

    [PunRPC]
    private void OnRoundCompleted(int i_RoundPhotonViewID)
    {
        
        if(PhotonNetwork.IsMasterClient)
        {
            if (!CheckGameEnd())
            {
                Debug.LogWarning("Master says Round is complete!");
                Round RoundHandler = PhotonView.Find(i_RoundPhotonViewID).gameObject.GetComponentInChildren<Round>();
                RoundHandler.RestartRound();
            }
        }
        m_GameEventBus.OnGameRoundComplete.Invoke(i_RoundPhotonViewID);

    }

    //Check to see if a Player has won the game
    public bool CheckGameEnd()
    {
        foreach(Player GamePlayer in m_PlayersInGame)
        {
            if(GamePlayer.getWins() >= m_RoundsToVictory)
            {
                //Invoke("CloseGame", 10f);
                return true;
            }
        }
        return false;
    }

    private void CloseGame()
    {
        app.Scenes.GoToMainMenu();
    }

    public List<Player> getPlayersInGame()
    {
        return m_PlayersInGame;
    }

}
