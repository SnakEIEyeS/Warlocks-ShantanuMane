using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour, IStore {

    [SerializeField]
    private GameEventBus m_GameEventBus = null;
    [SerializeField]
    private Canvas m_StoreCanvas = null;
    [SerializeField]
    private float m_StoreTime = 5.0f;

    #region IGameEventBus
    public IGameEventBus GameEventBus { get { return m_GameEventBus; } set { m_GameEventBus = value as GameEventBus; } }
    #endregion

    // Use this for initialization
    void Start () {
        m_GameEventBus.OnGamePreRound.AddListener(OpenShop);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenShop(Round i_Round)
    {
        m_StoreCanvas.enabled = true;
        print("OpenShop called");
        Invoke("CloseShop", m_StoreTime);
    }

    private void CloseShop()
    {
        print("CloseShop called");
        m_StoreCanvas.enabled = false;
        m_GameEventBus.OnStoreClose.Invoke(this);
    }
}
