using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class AbilityObjectPool : UnityObjectPool<GameObject>
{
    [SerializeField]
    private PhotonView m_PhotonView = null;
    public PhotonView AbilityPoolPhotonView { get { return m_PhotonView; } }

    [SerializeField]
    private int m_InitialSize = 10;
    [SerializeField]
    private bool m_AutoFill = true;
    [SerializeField]
    private int m_MinSize = 1;
    [SerializeField]
    private bool m_CreateIfNecessary = true;

    private int m_AbilityViewIDReturn = -1;

    #region UnityObjectPool
    [SerializeField]
    private List<GameObject> m_GameObjectList = new List<GameObject>();

    public override int InitialSize { get { return m_InitialSize; } set { m_InitialSize = value; } }
    public override bool AutoFill { get { return m_AutoFill; } }
    public override int MinSize { get { return m_MinSize; } }
    public override bool CreateIfNecessary { get { return m_CreateIfNecessary; } }

    public override GameObject Get()
    {
        if (m_GameObjectList.Count > m_MinSize)
        {
            GameObject RetGameObj = m_GameObjectList[0];
            m_GameObjectList.RemoveAt(0);
            return RetGameObj;
        }
        /*else if (CreateIfNecessary)
        {
            return AbilityCreationSequence();
        }*/
        else
        {
            return null;
        }

        //Tell Master I want an Ability
        //Master picks one and sends me the ViewID
        //If valid find object by ViewID and return it
        //If not valid I return null

    }
    public override void Return(GameObject t)
    {
        //print("Return called");
        //m_GameObjectList.Add(t);

        t.transform.position = this.transform.position;
        PhotonView ReturnedAbilityPhotonView = t.GetComponentInChildren<PhotonView>();
        m_PhotonView.RPC("AddAbilityObjToPool", RpcTarget.All, ReturnedAbilityPhotonView.ViewID);

    }
    public override void Resize(int count)
    {
        if (count >= MinSize)
        {
            m_GameObjectList.Capacity = count;
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        /*if (AutoFill)
        {
            Initialize();
        }*/
    }

    public void Init()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetGameObject() { return m_GameObjectList[0]; }
    public override GameObject GetPrefab() { return prefab; }
    private void Initialize()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < InitialSize; i++)
            {
                AbilityCreationSequence();
                //m_GameObjectList.Add(CreateAbility());
            }
        }
    }

    private GameObject AbilityCreationSequence()
    {
        GameObject CreatedAbility = CreateAbility();
        PhotonView CreatedAbilityPhotonView = CreatedAbility.GetComponentInChildren<PhotonView>();
        m_PhotonView.RPC("AddAbilityObjToPool", RpcTarget.All, CreatedAbilityPhotonView.ViewID);

        return CreatedAbility;
    }

    private GameObject CreateAbility()
    {
        object[] AbilityInstantiationData = new object[1];
        AbilityInstantiationData[0] = m_PhotonView.ViewID;
        GameObject CreatedAbility = PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity, 0, AbilityInstantiationData);
        /*IInstantiableAbility InstantiableAbility = CreatedAbility.GetComponent<IInstantiableAbility>();
        if(InstantiableAbility != null)
        {
            InstantiableAbility.AbilityPool = this;
        }*/

        return CreatedAbility;
    }

    [PunRPC]
    public void AddAbilityObjToPool(int i_AbilityPhotonViewID)
    {
        if (m_PhotonView.IsMine)
        {
            m_GameObjectList.Add(CreateAbility());
        }
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject AbilityGO = PhotonView.Find(i_AbilityPhotonViewID).gameObject;
            AbilityGO.transform.position = this.transform.position;
        }
    }

    [PunRPC]
    public void RemoveAbilityObjFromPool(int i_AbilityViewID)
    {
        GameObject AbilityToRemove = PhotonView.Find(i_AbilityViewID).gameObject;
        m_GameObjectList.Remove(AbilityToRemove);
    }

    [PunRPC]
    public void RequestAbility(PhotonMessageInfo photonMessageInfo)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (m_GameObjectList.Count > m_MinSize)
            {
                GameObject AbilityToGive = m_GameObjectList[0];
                PhotonView AbilityPhotonView = AbilityToGive.GetComponentInChildren<PhotonView>();
                m_PhotonView.RPC("", photonMessageInfo.Sender, AbilityPhotonView.ViewID);
                //m_GameObjectList.RemoveAt(0);

                //RPC to remove from pool
                m_PhotonView.RPC("RemoveAbilityObjFromPool", RpcTarget.All, AbilityPhotonView.ViewID);
            }
            else
            {
                //return invalid ViewID
                m_PhotonView.RPC("", photonMessageInfo.Sender, -1);
            }

        }
    }

    [PunRPC]
    public void ReceiveAbility(int i_AbilityPhotonViewID)
    {
        
    }
}
