using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour, IGame {

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
    public List<Player> PlayersInGame { get { return m_PlayersInGame; } }
    public List<Unit> UnitsInGame { get { return m_UnitsInGame; } }
    public IGameDataManager GameDataManager { get { return m_GameDataManager; } set { m_GameDataManager = value as GameDataManager; } }
    public IGameEventBus GameEventBus { get { return m_GameEventBus; } }
    #endregion

    private void Start()
    {
        m_GameEventBus.OnStoreClose.AddListener(OnStoreCloseStartRound);
        m_RoundHandler.OnPhaseChanged.AddListener(OnRoundPhaseChange);
        //OnPreRound(m_RoundHandler);
        //m_RoundHandler.StartRound();
        m_RoundHandler.RestartRound();
    }

    private void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            app.Scenes.GoToMainMenu();
        }
    }

    void OnRoundPhaseChange(Round round)
    {
        switch (round.Phase)
        {
            case RoundPhase.PreRound:
               OnPreRound(round);
                break;
            case RoundPhase.InProgress:
                OnRoundStart();
                break;
            case RoundPhase.Celebration:
                OnRoundOutcome(round);
                break;
            case RoundPhase.Completed:
                OnRoundCompleted(round);
                break;
        }
    }

    private void OnPreRound(Round i_Round)
    {
        GameEventBus.OnGamePreRound.Invoke(i_Round);
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
        m_RoundHandler.StartRound();
    }

    private void OnRoundStart()
    {
        //m_GameUI.enabled = false;
    }

    private void OnRoundOutcome(Round i_Round)
    {
        m_GameEventBus.OnGameRoundOutcome.Invoke(i_Round);
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

    private void OnRoundCompleted(Round i_Round)
    {
        m_GameEventBus.OnGameRoundComplete.Invoke(i_Round);
        if(!CheckGameEnd())
        {
            i_Round.RestartRound();
        }
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
