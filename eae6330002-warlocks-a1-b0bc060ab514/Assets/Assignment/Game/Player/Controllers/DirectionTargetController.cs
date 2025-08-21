using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionTargetController : MonoBehaviour, IInputController {

    [SerializeField]
    private UnitController m_SelectedUnit = null;
    [SerializeField]
    private Map map = null;
    [SerializeField]
    private AbilityEventBus m_AbilityEventBus = null;

    #region IInputController
    public void UpdateInput()
    {
        Vector3 mapPos, mapNormal;
        bool hit = map.GetMapPointFromScreenPoint(Input.mousePosition, out mapPos, out mapNormal);
        if (hit)
        {
            DrawDirectionArrow(mapPos);

            if (Input.GetMouseButtonDown(0))
            {
                HandleDirectionSelected(mapPos);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_AbilityEventBus.OnTargetingCanceled.Invoke(m_SelectedUnit);
            Debug.Log("Targeting Canceled");
        }


    }
#endregion

    public void Init(IUnitController i_UnitController, Map i_Map, IAbilityEventBus i_AbilityEventBus)
    {
        m_SelectedUnit = i_UnitController as UnitController;
        map = i_Map;
        m_AbilityEventBus = i_AbilityEventBus as AbilityEventBus;
    }

    // Use this for initialization
    void Start () {
        //m_AbilityEventBus.OnDirectionTargeted.AddListener(WriteDirection);
        //m_AbilityEventBus.OnTargetingCanceled.AddListener(WriteCanceled);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void HandleDirectionSelected(Vector3 i_MouseTargetPos)
    {
        Vector3 UnitPosition = m_SelectedUnit.getControlledUnit().transform.position;
        Vector3 CastDirection = new Vector3((i_MouseTargetPos - UnitPosition).x, 0.0f, (i_MouseTargetPos - UnitPosition).z).normalized;
        m_AbilityEventBus.OnDirectionTargeted.Invoke(m_SelectedUnit, CastDirection);
        print("Cast Direction: " + CastDirection);
    }

    void DrawDirectionArrow(Vector3 i_MouseTargetPos)
    {
        Vector3 UnitPosition = m_SelectedUnit.getControlledUnit().transform.position;
        Vector3 ArrowEndPosition = (i_MouseTargetPos - UnitPosition).normalized * 15.0f;
        Debug.DrawRay(UnitPosition, ArrowEndPosition, Color.black, Time.deltaTime, false);
    }

    /*void WriteDirection(UnitController i_UnitController, Vector3 i_Direction)
    {
        print("Cast Direction: " + i_Direction);
    }
    void WriteCanceled(UnitController i_UnitController)
    {
        print(i_UnitController);
    }*/

    public void SetSelectedUnit(IUnitController i_UnitController)
    {
        m_SelectedUnit = i_UnitController as UnitController;
    }
}
