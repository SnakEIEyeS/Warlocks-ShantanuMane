using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastController : MonoBehaviour, IInputController {

    private PlayerController m_ParentPlayerController = null;

    #region IInputController
    public void UpdateInput()
    {
        
    }
    #endregion

    // Use this for initialization
    void Start () {
        SetParentPlayerController();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetParentPlayerController()
    {
        m_ParentPlayerController = transform.root.GetComponentInChildren<PlayerController>();
    }

    public void RelayAbilityCast(UnitController i_UnitController)
    {
        i_UnitController.CastAbility();
    }
}
