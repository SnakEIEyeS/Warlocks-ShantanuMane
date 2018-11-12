using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Round : MonoBehaviour, IRound<RoundPhase,RoundEvent> {

    [SerializeField]
    private Game m_Game = null;
    [SerializeField]
    private Canvas m_GameUI = null;

    [SerializeField]
    private RoundTimer m_RoundTimer = null;

    [SerializeField]
    private List<IPlayer> m_Players = new List<IPlayer>();

    private List<IUnit> m_Units = new List<IUnit>();
    private Player m_RoundWinner = null;

    [SerializeField]
    private GameObject m_ObstaclePrefab = null;
    private List<GameObject> ObstacleList = new List<GameObject>();
    private int m_ObstacleCount = 5;

    //TODO eliminate the need for this by using RoundPhase
    private bool bRoundEnded = false;
    [SerializeField]
    private RoundPhase m_Phase = RoundPhase.PreRound;

    [SerializeField]
    private RoundEvent m_OnPhaseChanged = null;

    #region IRound
    public RoundPhase Phase { get { return m_Phase; } }
    public RoundEvent OnPhaseChanged { get { return m_OnPhaseChanged; } }
    public IPlayer Winner { get { return m_RoundWinner; } set { m_RoundWinner = (Player)value; } }
    public List<IUnit> AliveUnits { get { return m_Units; } }
    public List<IPlayer> AlivePlayers { get { return m_Players; } }

    public void StartRound()
    {
        m_RoundTimer.StartRoundTimer();
        SpawnObstacles();
        bRoundEnded = false;
        print("Round Started");

        m_Phase = RoundPhase.InProgress;
        m_OnPhaseChanged.Invoke(this);

    }

    #endregion

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            print("r pressed");
            EndRound();
        }

        if (!bRoundEnded && AliveUnits.Count <= 1)
        {
            EndRound();
        }
    }

    public void EndRound()
    {
        print("Round ended");
        bRoundEnded = true;
        m_RoundTimer.EndTimer();
        
        DetermineWinner();
        StartCoroutine(Celebration_Coroutine());

        RoundCleanup();

    }

    private void DetermineWinner()
    {
        if (AliveUnits[0] != null)
        {
            m_RoundWinner = AliveUnits[0].Owner;
            m_RoundWinner.setWins(m_RoundWinner.getWins() + 1);
            print(m_RoundWinner.Name + " - Wins: " + m_RoundWinner.getWins());
        }
    }

    IEnumerator Celebration_Coroutine()
    {
        m_Phase = RoundPhase.Celebration;
        m_OnPhaseChanged.Invoke(this);

        yield return new WaitForSeconds(5f);
        m_Phase = RoundPhase.Completed;
        m_OnPhaseChanged.Invoke(this);
    }

    private void RoundCleanup()
    {
        AliveUnits.Clear();
        AlivePlayers.Clear();

        foreach (GameObject Obstacle in ObstacleList)
        {
            Destroy(Obstacle);
        }
        ObstacleList.Clear();
    }

    
    public void RestartRound()
    {
        print("Round restarting");
        m_RoundTimer.Reset();
        ResetRound();

        m_Phase = RoundPhase.PreRound;
        m_OnPhaseChanged.Invoke(this);
        
        //Invoke("StartRound", 10.0f);
        //StartRound();
    }

    private void ResetRound()
    {
        m_RoundWinner = null;

        AddAllAlivePlayers();
        AddAllAliveUnits();
    }

    public void AddAllAlivePlayers()
    {
        print("AddAllAlivePlayers called");
        Player[] ArrAlivePlayers = FindObjectsOfType<Player>();
        foreach(IPlayer AlivePlayer in ArrAlivePlayers)
        {
            AlivePlayers.Add(AlivePlayer);
        }
    }

    public void AddAllAliveUnits()
    {
        print("AddAllAliveUnits called");
        Unit[] ArrAliveUnits = FindObjectsOfType<Unit>();
        foreach(IUnit AliveUnit in ArrAliveUnits)
        {
            AliveUnits.Add(AliveUnit);
        }
    }

    public void RegisterUnitDeath(IUnit i_DyingUnit)
    {
        AliveUnits.Remove(i_DyingUnit);
        
    }

    private void SpawnObstacles()
    {
        float SpawnRadius = 
            (m_RoundTimer.getIsland().gameObject.GetComponentInChildren<MeshRenderer>().transform.localScale.z) / 2f;
        Vector3 IslandLocation = m_RoundTimer.getIsland().gameObject.transform.position;


        for (int i = 0; i < m_ObstacleCount; i++)
        {
            Vector2 SpawnPoint = UnityEngine.Random.insideUnitCircle * SpawnRadius;
            GameObject SpawnedObstacle = (GameObject)Instantiate(m_ObstaclePrefab, new Vector3(SpawnPoint.x, 0f, SpawnPoint.y) + IslandLocation, Quaternion.identity);
            ObstacleList.Add(SpawnedObstacle);
        }
    }
    
}
