using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventBus : MonoBehaviour, IGameEventBus {

    [SerializeField]
    private GamePreRound m_GamePreRound = null;
    [SerializeField]
    private GameStoreClose m_GameStoreClose = null;
    [SerializeField]
    private GameRoundOutcome m_GameRoundOutcome = null;
    [SerializeField]
    private GameRoundComplete m_GameRoundComplete = null;

    #region IGameEventBus
    public GamePreRound OnGamePreRound { get { return m_GamePreRound; } }
    public GameStoreClose OnStoreClose { get { return m_GameStoreClose; } }
    public GameRoundOutcome OnGameRoundOutcome { get { return m_GameRoundOutcome; } }
    public GameRoundComplete OnGameRoundComplete { get { return m_GameRoundComplete; } }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
