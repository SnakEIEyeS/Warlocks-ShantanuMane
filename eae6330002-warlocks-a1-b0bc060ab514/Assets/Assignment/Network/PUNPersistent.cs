using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


public enum WarlocksEventCode
{
    PlayerLoadedGame = 1,
    AllPlayersLoadComplete = 2,
    Default = 0
}


public class PUNPersistent : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public static PUNPersistent Instance;

    [SerializeField]
    private App app = null;

    private int m_PlayersInGameScene = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);   
    }

    public override void OnDisable()
    {
        //PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        
    }

    private void OnSceneFinishedLoading(Scene i_Scene, LoadSceneMode i_LoadMode)
    {
        //TODO check if Scenes script notifies of this multiple times within a load
        if (i_Scene.name == app.Scenes.GameSceneName)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                HandlePlayerLoadedGameScene();
            }
            else
            {
                NotifyPlayerLoadGameComplete();
            }
            Debug.LogWarning(PhotonNetwork.NickName + " loaded Warlocks scene");
            //PlayerManager.PlayerInstance.CreatePlayerInWorld();
        }
    }

    private void MasterPlayerLoadedGame()
    {

    }

    private void NotifyPlayerLoadGameComplete()
    {
        //photonView.RPC("HandlePlayerLoadedGameScene", RpcTarget.MasterClient);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            CachingOption = EventCaching.DoNotCache,
            InterestGroup = 0,
            Receivers = ReceiverGroup.MasterClient
        };

        PhotonNetwork.RaiseEvent((byte)WarlocksEventCode.PlayerLoadedGame, null, 
            raiseEventOptions, new ExitGames.Client.Photon.SendOptions { Reliability = true });
    }

    [PunRPC]
    private void HandlePlayerLoadedGameScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            m_PlayersInGameScene++;
            if (m_PlayersInGameScene == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                //Start the Game
                AllPlayersLoadedGame();
            }
            //Debug.LogWarning("Player loaded game. PlayersInGameScene = " + m_PlayersInGameScene);
        }
    }

    private void AllPlayersLoadedGame()
    {
        //Debug.LogWarning("All players loaded game " + PhotonNetwork.CurrentRoom.PlayerCount.ToString());
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            CachingOption = EventCaching.DoNotCache,
            InterestGroup = 0,
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent((byte)WarlocksEventCode.AllPlayersLoadComplete, null, 
            raiseEventOptions, new ExitGames.Client.Photon.SendOptions { Reliability = true });
    }

    private void PrepareForGame()
    {
        /*if(PlayerManager.PlayerInstance)
        {
            PlayerManager.PlayerInstance.CreatePlayerInWorld();
        }*/

        if(PhotonNetwork.IsMasterClient)
        {
            Warlocks.GameManager.Instance.BeginGameSession();
        }
    }

    #region IOnPunCallback

    public void OnEvent(ExitGames.Client.Photon.EventData i_PhotonEvent)
    {
        byte eventCode = i_PhotonEvent.Code;

        switch(eventCode)
        {
            case (byte)WarlocksEventCode.PlayerLoadedGame:
                HandlePlayerLoadedGameScene();
                break;

            case (byte)WarlocksEventCode.AllPlayersLoadComplete:
                PrepareForGame();
                break;

            default:
                break;
        }
    }

    #endregion


    #region MonoBehaviourPunCallbacks
    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        m_PlayersInGameScene = 0;
    }
    #endregion
}
