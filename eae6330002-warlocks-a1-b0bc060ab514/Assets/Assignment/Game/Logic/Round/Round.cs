using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;


public class Round : MonoBehaviour, IRound<RoundPhase,RoundEvent> {

    [SerializeField]
    private PhotonView m_RoundPhotonView = null;
    public PhotonView RoundPhotonView { get { return m_RoundPhotonView; } }
    [SerializeField]
    private Game m_Game = null;
    [SerializeField]
    private Canvas m_GameUI = null;

    [SerializeField]
    private RoundTimer m_RoundTimer = null;

    [SerializeField]
    private List<IPlayer> m_Players = new List<IPlayer>();
    private List<IUnit> m_Units = new List<IUnit>();

    //Sync over Network. Only need to send PhotonViewID
    private Player m_RoundWinner = null;

    [SerializeField]
    private GameObject m_ObstaclePrefab = null;
    private List<GameObject> ObstacleList = new List<GameObject>();
    private int m_ObstacleCount = 5;

    //TODO eliminate the need for this by using RoundPhase
    private bool bRoundEnded = false;
    //Sync over Network. Can be cast to byte
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
        if(PhotonNetwork.IsMasterClient)
        {
            m_RoundTimer.StartRoundTimer();
            SpawnObstacles();
            bRoundEnded = false;
            print("Round Started");

            m_Phase = RoundPhase.InProgress;
            m_OnPhaseChanged.Invoke(this);
        }
    }

    #endregion

    void Start()
    {
    }

    void Update()
    {
        /*if(Input.GetKeyDown("r"))
        {
            print("r pressed");
            EndRound();
        }*/

        if(PhotonNetwork.IsMasterClient)
        {
            //Check if Round should End
            if (m_Phase == RoundPhase.InProgress && !bRoundEnded && AliveUnits.Count <= 1)
            {
                EndRound();
            }
        }
        
    }

    public void EndRound()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            print("Round ended");
            bRoundEnded = true;
            m_RoundTimer.EndTimer();

            DetermineWinner();
            //StartCoroutine(Celebration_Coroutine());
            

            RoundCleanup();
            Celebration_Coroutine();
        }
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

    /*IEnumerator Celebration_Coroutine()
    {
        //if(PhotonNetwork.IsMasterClient)
        {
            m_Phase = RoundPhase.Celebration;
            m_OnPhaseChanged.Invoke(this);

            Debug.LogWarning("Reached pre yield");
            yield return new WaitForSeconds(5f);
            Debug.LogWarning("Reached post yield");
            m_Phase = RoundPhase.Completed;
            m_OnPhaseChanged.Invoke(this);
        }
    }*/
    private void  Celebration_Coroutine()
    {
        Debug.LogWarning("Reached celebration. Master is " + PhotonNetwork.MasterClient.NickName);
        
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("Reached pre yield");
            Invoke("EndCelebration", 5f);
            m_Phase = RoundPhase.Celebration;
            m_OnPhaseChanged.Invoke(this);

            
            //yield return new WaitForSeconds(5f);
            
        }
    }

    private void EndCelebration()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("Reached post yield");
            m_Phase = RoundPhase.Completed;
            m_OnPhaseChanged.Invoke(this);
        }    
    }

    //TODO Network Destroy obstacles
    private void RoundCleanup()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            AliveUnits.Clear();
            AlivePlayers.Clear();

            foreach (GameObject Obstacle in ObstacleList)
            {
                PhotonNetwork.Destroy(Obstacle);
                //Destroy(Obstacle);
            }
            ObstacleList.Clear();
        }
    }

    
    public void RestartRound()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            print("Round restarting");
            m_RoundTimer.Reset();
            ResetRound();

            m_Phase = RoundPhase.PreRound;
            m_OnPhaseChanged.Invoke(this);

            //Invoke("StartRound", 10.0f);
            //StartRound();
        }

    }

    private void ResetRound()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            m_RoundWinner = null;

            AddAllAlivePlayers();
            AddAllAliveUnits();
        }
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

    //Handled on Master because Damage is also handled on Master
    public void RegisterUnitDeath(IUnit i_DyingUnit)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            AliveUnits.Remove(i_DyingUnit);
        }
    }

    //TODO Network Instantiate Obstacles
    private void SpawnObstacles()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            float SpawnRadius =
            (m_RoundTimer.getIsland().gameObject.GetComponentInChildren<MeshRenderer>().transform.localScale.z) / 2f;
            Vector3 IslandLocation = m_RoundTimer.getIsland().gameObject.transform.position;


            for (int i = 0; i < m_ObstacleCount; i++)
            {
                Vector2 SpawnPoint = UnityEngine.Random.insideUnitCircle * SpawnRadius;
                GameObject SpawnedObstacle = PhotonNetwork.Instantiate(
                    m_ObstaclePrefab.name, new Vector3(SpawnPoint.x, 0f, SpawnPoint.y) + IslandLocation, Quaternion.identity
                    );
                ObstacleList.Add(SpawnedObstacle);
            }
        }
        
    }
    
}
