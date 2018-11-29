using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IPlayer {

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

    void Start()
    {
        if(GetComponent<LocalPlayerController>())
        {
            IsLocal = true;
        }

        RegisterAlivePlayer();
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

}
