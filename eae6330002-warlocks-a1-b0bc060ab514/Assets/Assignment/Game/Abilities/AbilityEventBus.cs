using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityEventBus : MonoBehaviour, IAbilityEventBus
{
    [SerializeField]
    private DirectionTargeted m_OnDirectionTargeted = null;
    [SerializeField]
    private PointTargeted m_OnPointTargeted = null;
    [SerializeField]
    private TargetingCanceled m_OnTargetingCanceled = null;
    [SerializeField]
    private CastComplete m_OnCastComplete = null;

    #region IAbilityEventBus
    public DirectionTargeted OnDirectionTargeted { get { return m_OnDirectionTargeted; } }
    public PointTargeted OnPointTargeted { get { return m_OnPointTargeted; } }
    public TargetingCanceled OnTargetingCanceled { get { return m_OnTargetingCanceled; } }
    public CastComplete OnCastComplete { get { return m_OnCastComplete; } }
    #endregion
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
