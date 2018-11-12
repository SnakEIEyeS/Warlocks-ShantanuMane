using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitOverheadUI : MonoBehaviour {

    [SerializeField]
    private Unit m_Unit = null;

    [SerializeField]
    private Canvas UnitInfoCanvas = null;
    [SerializeField]
    private Image m_HealthBar = null;
    private Health m_UnitHealthRef = null;

    private GameObject m_MainCamera = null;

    // Use this for initialization
    void Start ()
    {
        m_MainCamera = GameObject.FindWithTag("MainCamera");

        Text PlayerName = UnitInfoCanvas.GetComponentInChildren<Text>();
        PlayerName.text = m_Unit.Owner.Name;

        m_UnitHealthRef = m_Unit.getHealth();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (UnitInfoCanvas)
        {
            UnitInfoCanvas.transform.rotation = m_MainCamera.transform.rotation;
        }

        if (m_HealthBar)
        {
            m_HealthBar.fillAmount = m_UnitHealthRef.Current / m_UnitHealthRef.Max;
        }
    }
}
