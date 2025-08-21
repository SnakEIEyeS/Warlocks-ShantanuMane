using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager PlayerInstance;

    [SerializeField]
    private PhotonView m_PhotonView = null;

    #region GameRelated
    [SerializeField]
    private GameObject m_PlayerPrefab = null;
    [SerializeField]
    private GameObject m_UnitPrefab = null;

    #endregion

    private void Awake()
    {
        if(PlayerInstance == null)
        {
            PlayerInstance = this;
        }
    }
    // Use this for initialization
    void Start () {
        //CreatePlayerInWorld();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreatePlayerInWorld(Photon.Realtime.Player i_PhotonPlayer)
    {
        //Unit needs SpawnPoint. UnitCtrlr needs Map, AbilityFactory, AbilityEventBus. UnitStatusCtrlr needs StatusEventBus.
        //Pass int as SpawnPointIndex. Everything else can be handled locally.
        object[] UnitInstantiationData = new object[1];
        //TODO pick SpawnPoint
        int RandomSpawnPointIndex = Random.Range(0, GameDataManager.Instance.SpawnPoints.Count);
        UnitInstantiationData[0] = RandomSpawnPointIndex;
        GameObject CreatedUnitGO = PhotonNetwork.Instantiate(
            m_UnitPrefab.name, 
            Vector3.zero, Quaternion.identity, 0, 
            UnitInstantiationData
            );

        PhotonView UnitGOPhotonView = CreatedUnitGO.GetComponentInChildren<PhotonView>();
        UnitGOPhotonView.TransferOwnership(i_PhotonPlayer);
        UnitController CreatedUnitController = CreatedUnitGO.GetComponentInChildren<UnitController>();
        CreatedUnitController.AnimationPhotonView.TransferOwnership(i_PhotonPlayer);

        
        //For Player & For PlayerController
        //Player & playerCtrlr need PlayerName, Unit, RoundInstance, UnitCtrlr, HUD, Map, AbilityEventBus
        //Pass PlayerName & PhotonViewIDs of Unit's GO, RoundInstance. Everything else can be handled locally.
        object[] PlayerInstantiationData = new object[3];
        PlayerInstantiationData[0] = i_PhotonPlayer.NickName;
        PlayerInstantiationData[1] = Game.GameInstance.RoundInstance.gameObject.GetComponentInChildren<PhotonView>().ViewID;
        PlayerInstantiationData[2] = UnitGOPhotonView.ViewID;
        GameObject CreatedPlayerGO = PhotonNetwork.Instantiate(
            m_PlayerPrefab.name, 
            Vector3.zero, Quaternion.identity, 0, 
            PlayerInstantiationData
            );

        PhotonView PlayerGOPhotonView = CreatedPlayerGO.GetComponentInChildren<PhotonView>();
        PlayerGOPhotonView.TransferOwnership(i_PhotonPlayer);

        Debug.LogError("Creating manifestation for: " + i_PhotonPlayer.NickName);

        //Initialize on Master
        //InitializePlayerEntities(UnitGOPhotonView.ViewID, PlayerGOPhotonView.ViewID);

        //if(i_PhotonPlayer != PhotonNetwork.MasterClient)
        {
            //Make RPC call to init both Unit & Player
            Warlocks.GameManager.Instance.photonView.RPC(
                "InitializePlayerEntities", RpcTarget.All,
                UnitGOPhotonView.ViewID,
                PlayerGOPhotonView.ViewID
                );
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

    private void OnDestroy()
    {
        //print("They're destroying PlayerManager");
    }
}
