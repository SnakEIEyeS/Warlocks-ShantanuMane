using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    [SerializeField]
    private Game m_Game = null;
    [SerializeField]
    private GameEventBus m_GameEventBus = null;
    [SerializeField]
    private Canvas m_ScoreScreen = null;

    [SerializeField]
    private Text m_Player1Record = null;
    [SerializeField]
    private Text m_Player2Record = null;

    public IGameEventBus GameEventBus { get { return m_GameEventBus; } set { m_GameEventBus = value as GameEventBus; } }

    // Use this for initialization
    void Start () {
        m_GameEventBus.OnGamePreRound.AddListener(HideScoreScreen);
        m_GameEventBus.OnGameRoundOutcome.AddListener(ShowScoreScreen);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowScoreScreen(Round i_Round)
    {
        UpdatePlayerRecords();
        m_ScoreScreen.enabled = true;
    }
    public void HideScoreScreen(Round i_Round)
    {
        print("HideScoreScreen called");
        UpdatePlayerRecords();
        m_ScoreScreen.enabled = true;
        m_ScoreScreen.enabled = false;
    }

    public void UpdatePlayerRecords()
    {
        List<Player> GamePlayers = m_Game.getPlayersInGame();
        m_Player1Record.text = GamePlayers[0].Name + " - Wins: " + GamePlayers[0].getWins();
        m_Player2Record.text = GamePlayers[1].Name + " - Wins: " + GamePlayers[1].getWins();
    }
}
