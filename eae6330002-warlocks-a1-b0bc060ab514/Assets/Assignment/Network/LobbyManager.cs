using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks, IAppAware
{
    [SerializeField]
    private App app = null;
    public App App { get { return app; } set { app = value; } }

    [SerializeField]
    private Text m_PlayerCountText = null;

    public byte PlayerCountStored = 0;
    public string DebugText = null;

    [SerializeField]
    private byte m_MainMenuSceneIndex = 1;
    [SerializeField]
    private byte m_WarlocksGameSceneIndex = 4;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerCountStored = PhotonNetwork.CurrentRoom.PlayerCount;
        m_PlayerCountText.text = PlayerCountStored.ToString();

        DebugText = m_PlayerCountText.text;
	}


    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene(m_MainMenuSceneIndex);
        app.Scenes.GoToMainMenu();
    }


    #endregion


    #region Public Methods

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to start game but we are not the master Client");
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        Debug.LogFormat("PhotonNetwork : Loading WarlocksGame, Loader is " + PhotonNetwork.NickName);
        //PhotonNetwork.LoadLevel(m_WarlocksGameSceneIndex);

        photonView.RPC("RPC_LoadGameScene", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_LoadGameScene()
    {
        Debug.LogWarning(PhotonNetwork.NickName + " loading Game from RPCall");
        app.Scenes.LoadGame();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
