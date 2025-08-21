using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoldData
{
    public Player m_Player;
    public int m_Gold;

    public PlayerGoldData(Player i_Player, int i_Gold)
    {
        m_Player = i_Player;
        m_Gold = i_Gold;
    }
}

[System.Serializable]
public class PlayerKillData
{
    public Player m_Player;
    public int m_Kills;

    public PlayerKillData(Player i_Player, int i_Kills)
    {
        m_Player = i_Player;
        m_Kills = i_Kills;
    }

    public void SetKills(int i_Kills)
    {
        m_Kills = i_Kills;
    }
}
