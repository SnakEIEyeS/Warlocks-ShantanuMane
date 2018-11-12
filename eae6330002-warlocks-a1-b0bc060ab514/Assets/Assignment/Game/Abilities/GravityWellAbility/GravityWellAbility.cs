using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWellAbility : MonoBehaviour, IAbility, ICastableAbility, IInstantiableAbility, IStatusEffectAbility
{

    private PlayerController m_InstigatorController = null;
    private UnitController m_Caster = null;

    private AbilityTargetType m_TargetType = AbilityTargetType.PointTarget;
    private float m_CastTime = 0.6f;
    private float m_Cooldown = 12.0f;

    private Vector3 m_WellLocation = Vector3.zero;
    private float m_SetupTime = 1.0f;
    private float m_Duration = 3.0f;
    private float m_InnerRadius = 10.0f;
    private float m_InnerForce = 250.0f;
    private float m_OuterRadius = 30.0f;
    private float m_OuterForce = 170.0f;
    private bool m_bWellCreated = false;

    private List<UnitController> m_CaughtUnitControllers = new List<UnitController>();

    private IObjectPool<GameObject> m_AbilityPool = null;
     private StatusEventBus m_StatusEventBus = null;

    #region IAbility
    public IPlayerController Instigator
    {
        get { return m_InstigatorController; }
        set { m_InstigatorController = value as PlayerController; }
    }
    public IUnitController Caster
    {
        get { return m_Caster; }
        set { m_Caster = value as UnitController; }
    }
    #endregion

    #region ICastableAbility
    public AbilityTargetType TargetType { get { return m_TargetType; } }
    public float CastTime { get { return m_CastTime; } }
    public float Cooldown
    {
        get { return m_Cooldown; }
        set { m_Cooldown = value; }
    }
    public void AbilityExecute()
    {
        Invoke("CreateGravityWell", m_SetupTime);
    }
    public void AbilityEnd()
    {
        m_Caster = null;
        m_InstigatorController = null;

        m_AbilityPool.Return(this.gameObject);
    }
    #endregion

    #region IInstantiableAbility
    public IObjectPool<GameObject> AbilityPool { get { return m_AbilityPool; } set { m_AbilityPool = value; } }
    #endregion

    #region IStatusEffectAbility
    public IStatusEventBus StatusEventBus { get { return m_StatusEventBus; } set { m_StatusEventBus = value as StatusEventBus; } }
    #endregion

    // Use this for initialization
    void Start()
    {
        AbilityEventBus abilityEventBus = (AbilityEventBus)FindObjectOfType<AbilityEventBus>();
        abilityEventBus.OnPointTargeted.AddListener(SetWellLocation);

        StatusEventBus = FindObjectOfType<StatusEventBus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bWellCreated)
        {
            PullCaughtUnits();
        }
    }

    private void PullCaughtUnits()
    {
        //print("Pull called");
        foreach (UnitController caughtUnitCtrlr in m_CaughtUnitControllers)
        {
            Rigidbody caughtRgdBdy = caughtUnitCtrlr.transform.root.GetComponentInChildren<Rigidbody>();
            if (caughtRgdBdy)
            {
                caughtRgdBdy.isKinematic = false;
                //print(caughtRgdBdy.gameObject.name);
                ApplyPullForce(caughtRgdBdy);
            }
        }
    }

    private void ApplyPullForce(Rigidbody caughtRgdBdy)
    {
        Vector3 PositionDifference = m_WellLocation - caughtRgdBdy.transform.position;
        if (PositionDifference.sqrMagnitude <= m_InnerRadius * m_InnerRadius)
        {
            caughtRgdBdy.AddForce(PositionDifference.normalized * m_InnerForce, ForceMode.Force);
        }
        else
        {
            caughtRgdBdy.AddForce(PositionDifference.normalized * m_OuterForce, ForceMode.Force);
        }
    }

    private void SetWellLocation(UnitController i_UnitController, Vector3 i_TargetPoint)
    {
        if (m_Caster == i_UnitController)
        {
            m_WellLocation = i_TargetPoint;
        }
    }

    void CreateGravityWell()
    {
        DrawDebugLines();
        Collider[] CaughtColliders = Physics.OverlapSphere(m_WellLocation, m_OuterRadius);
        foreach (Collider caughtColl in CaughtColliders)
        {
            UnitController caughtCollUnitCtrlr = caughtColl.transform.root.GetComponentInChildren<UnitController>();
            if (caughtCollUnitCtrlr != null)
            {
                m_CaughtUnitControllers.Add(caughtCollUnitCtrlr);

                m_StatusEventBus.StunAttemptEvent.Invoke(caughtCollUnitCtrlr.getControlledUnit(), m_Duration);
            }
        }

        m_bWellCreated = true;

        Invoke("EndGravityWell", m_Duration);
    }

    private void DrawDebugLines()
    {
        Debug.DrawRay(m_WellLocation, Vector3.forward * m_OuterRadius, Color.black, 3.0f, false);
        Debug.DrawRay(m_WellLocation, -Vector3.forward * m_OuterRadius, Color.black, 3.0f, false);
        Debug.DrawRay(m_WellLocation, Vector3.right * m_OuterRadius, Color.black, 3.0f, false);
        Debug.DrawRay(m_WellLocation, -Vector3.right * m_OuterRadius, Color.black, 3.0f, false);
    }

    void EndGravityWell()
    {
        m_bWellCreated = false;
        foreach(UnitController caughtUnitCtrlr in m_CaughtUnitControllers)
        {
            caughtUnitCtrlr.RegainControl();
        }
        m_CaughtUnitControllers.Clear();
        AbilityEnd();
    }
}
