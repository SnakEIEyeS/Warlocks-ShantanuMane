using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Warlocks
{
    public class GameManager : MonoBehaviourPunCallbacks, IAppAware
    {

        public static GameManager Instance;
        //[Tooltip("The prefab to use for representing the player")]
        //public GameObject playerPrefab;

        [SerializeField]
        private App app = null;
        public App App { get { return app; } set { app = value; } }

        [SerializeField]
        private GameObject m_PlayerManagerPrefab = null;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            
            //SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }

        void Start()
        {
            

            /*if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }*/
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

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }


        #endregion


        #region Public Methods

        public void AllPlayersLoadedGame()
        {
            Game.GameInstance.StartGame();
        }

        public void BeginGameSession()
        {
            PhotonView Attached = GetComponentInChildren<PhotonView>();
            //Attached.RPC("CreatePlayerForSession", RpcTarget.All);
            CreatePlayerForSession();
            GameDataManager.Instance.Init();
            Game.GameInstance.StartGame();
        }

        [PunRPC]
        private void CreatePlayerForSession()
        {
            GameObject CreatedGO = Instantiate(m_PlayerManagerPrefab, Vector3.zero, Quaternion.identity);
            PlayerManager CreatedPlayerManager = CreatedGO.GetComponentInChildren<PlayerManager>();

            if (CreatedPlayerManager)
            {
                //CreatedPlayerManager.CreatePlayerInWorld();
                foreach (Photon.Realtime.Player RoomPlayer in PhotonNetwork.PlayerList)
                {
                    
                    CreatedPlayerManager.CreatePlayerInWorld(RoomPlayer);
                }
            }
        }

        [PunRPC]
        private void InitializePlayerEntities(int i_UnitPhotonViewID, int i_PlayerPhotonViewID)
        {
            Unit GrantedUnit = PhotonView.Find(i_UnitPhotonViewID).gameObject.GetComponentInChildren<Unit>();
            GrantedUnit.Init();
            UnitController GrantedUnitCtrlr = GrantedUnit.gameObject.GetComponentInChildren<UnitController>();
            GrantedUnitCtrlr.Init();
            UnitStatusController GrantedStatusCtrlr = GrantedUnit.gameObject.GetComponentInChildren<UnitStatusController>();
            GrantedStatusCtrlr.Init();

            Player GrantedPlayer = PhotonView.Find(i_PlayerPhotonViewID).gameObject.GetComponentInChildren<Player>();
            GrantedPlayer.Init();
            LocalPlayerController GrantedLocalPC = GrantedPlayer.gameObject.GetComponentInChildren<LocalPlayerController>();
            GrantedLocalPC.Init();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion


        #region Private Methods


        


        #endregion
    }
}