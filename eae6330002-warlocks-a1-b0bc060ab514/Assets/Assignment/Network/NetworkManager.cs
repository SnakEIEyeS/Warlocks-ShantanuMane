using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks, IAppAware
{
    public static NetworkManager Instance;

    #region IAppAware
    [SerializeField]
    private App app = null;
    public App App { get { return app; } set { app = value; } }
    #endregion

    private int m_PlayersInGameScene = 0;

    private void Awake()
    {
        Instance = this;

    }

    // Use this for initialization
    void Start () {
        //DontDestroyOnLoad(gameObject);
        //SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /*private void OnSceneFinishedLoading(Scene i_Scene, LoadSceneMode i_LoadMode)
    {
        if(i_Scene.name == app.Scenes.GameSceneName)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PlayerLoadedGameScene();
            }
            else
            {
                NotifyPlayerLoadGameComplete();
            }
            Debug.Log(PhotonNetwork.NickName + " loaded Warlocks scene");
            PlayerManager.PlayerInstance.CreatePlayerInWorld();
        }
    }

    private void MasterPlayerLoadedGame()
    {

    }

    private void NotifyPlayerLoadGameComplete()
    {
        photonView.RPC("PlayerLoadedGameScene", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void PlayerLoadedGameScene()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            m_PlayersInGameScene++;
            if(m_PlayersInGameScene >= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                //Start the Game
                Com.MyCompany.MyGame.GameManager.Instance.AllPlayersLoadedGame();
            }
        }
    }*/


    #region MonoBehaviourPunCallbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        //m_PlayersInGameScene = 0;
    }

    #endregion

    public void OnDestroy()
    {
        //print("They're destroying NetworkManager");
    }
}
