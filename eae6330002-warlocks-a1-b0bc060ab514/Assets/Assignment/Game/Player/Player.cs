using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;

//Only need to send PhotonView ID. No need to serialize.
public class Player : MonoBehaviour, IPlayer {

    [SerializeField]
    private PhotonView m_PhotonView = null;
    public PhotonView PlayerPhotonView { get { return m_PhotonView; } }

    [SerializeField]
    private Round m_RoundInstance = null;

    [SerializeField]
    private String PlayerName = null;

    private bool bIsLocalPlayer = false;
    private int m_Wins = 0;

    [SerializeField]
    private Color m_Color = Color.white;

#region IPlayer
    public Color Color { get { return m_Color; } set { m_Color = value; } }
    public bool IsLocal { get { return bIsLocalPlayer; } set { bIsLocalPlayer = value; } }
    public string Name { get { return PlayerName; } set { PlayerName = value; } }
    #endregion

    private void Awake()
    {
        
    }
    void Start()
    {
        if(GetComponent<LocalPlayerController>())
        {
            IsLocal = true;
        }

        //RegisterAlivePlayer();
    }

    public void Init()
    {
        Name = (string)m_PhotonView.InstantiationData[0];

        PhotonView RoundPhotonView = PhotonView.Find((int)m_PhotonView.InstantiationData[1]);
        m_RoundInstance = RoundPhotonView.gameObject.GetComponentInChildren<Round>();

        if (PhotonNetwork.IsMasterClient)
        {
            RegisterPlayerInGame();
        }
        /*else
        {
            PlayerPhotonView.RPC("RegisterPlayerInGame", RpcTarget.MasterClient);
        }*/
    }

    void RegisterAlivePlayer()
    {
        //Round RoundInstance = FindObjectOfType<Round>();
        //RoundInstance.AddAlivePlayer(this);
    }

    public Round getRoundInstance()
    {
        return m_RoundInstance;
    }

    public int getWins()
    { return m_Wins; }
    public void setWins(int i_Wins)
    { m_Wins = i_Wins; }


    #region Photon related

    [PunRPC]
    private void RegisterPlayerInGame()
    {
        Game.GameInstance.PlayersInGame.Add(this);
    }

    #endregion

}
