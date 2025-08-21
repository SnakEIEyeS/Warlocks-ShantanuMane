using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldEventBus : MonoBehaviour, IGoldEventBus {

    [SerializeField]
    private AwardGold m_AwardGoldEvent = null;
    [SerializeField]
    private DeductGold m_DeductGoldEvent = null;

    #region IGoldEventBus
    public AwardGold AwardGold { get { return m_AwardGoldEvent; } }
    public DeductGold DeductGold { get { return m_DeductGoldEvent; } }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
