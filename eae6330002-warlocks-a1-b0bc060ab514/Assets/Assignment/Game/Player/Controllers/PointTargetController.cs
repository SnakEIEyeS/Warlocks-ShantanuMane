﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTargetController : MonoBehaviour, IInputController {

    [SerializeField]
    private UnitController m_SelectedUnit = null;
    [SerializeField]
    private Map map = null;
    [SerializeField]
    private AbilityEventBus m_AbilityEventBus = null;

    #region IInputController
    public void UpdateInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mapPos, mapNormal;
            bool hit = map.GetMapPointFromScreenPoint(Input.mousePosition, out mapPos, out mapNormal);
            if (hit)
            {
                m_AbilityEventBus.OnPointTargeted.Invoke(m_SelectedUnit, mapPos);
                print("Target Point: " + mapPos);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_AbilityEventBus.OnTargetingCanceled.Invoke(m_SelectedUnit);
            Debug.Log("Targeting Canceled");
        }
    }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
