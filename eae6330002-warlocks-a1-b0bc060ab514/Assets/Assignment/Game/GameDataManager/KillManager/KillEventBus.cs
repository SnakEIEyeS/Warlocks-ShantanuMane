using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEventBus : MonoBehaviour, IKillEventBus {

    [SerializeField]
    private DeathEvent m_DeathEvent = null;

    #region IKillEventBus
    public DeathEvent DeathEvent { get { return m_DeathEvent; } }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
