using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDetailsHUD : MonoBehaviour {

    [SerializeField]
    private LocalPlayerController m_LocalPlayerController = null;
    private UnitController m_SelectedUnitController;
    private Unit m_SelectedUnit;

    [SerializeField]
    private RawImage m_Portrait = null;
    [SerializeField]
    private Image m_HealthBar = null;
    [SerializeField]
    private Text m_HealthNumbers = null;
    [SerializeField]
    private Image m_ManaBar = null;

    private Health m_UnitHealthRef;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        setUnitDetailsInHUD();

    }

    public void setSelectedUnit(UnitController i_UnitController)
    {
        if(i_UnitController)
        {
            m_SelectedUnitController = i_UnitController;
            m_SelectedUnit = m_SelectedUnitController.getControlledUnit();

            if(m_SelectedUnit)
            {
                setUnitReferences();
            }
        }
    }

    private void setUnitReferences()
    {
        m_UnitHealthRef = m_SelectedUnit.getHealth();
        m_Portrait.texture = m_SelectedUnit.getPortraitCamera().targetTexture;
    }

    private void setUnitDetailsInHUD()
    {
        m_HealthBar.fillAmount = m_UnitHealthRef.Current / m_UnitHealthRef.Max;
        m_HealthNumbers.text = Mathf.CeilToInt(m_UnitHealthRef.Current).ToString() + "/" + Mathf.CeilToInt(m_UnitHealthRef.Max).ToString();
    }
}
