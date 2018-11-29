using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatisticsController : MonoBehaviour, IUnitStatisticsController {

    private Unit m_Unit = null;


    #region MoveSpeed
    private float m_MoveSpeed = 0.0f;
    private List<float> m_RawMoveSpeedModifiers = new List<float>();
    private List<float> m_PercentMoveSpeedModifiers = new List<float>();
    public float MoveSpeed { get { return FinalMoveSpeed(); } }
    public List<float> RawMoveSpeedModifiers { get { return m_RawMoveSpeedModifiers; } }
    public List<float> PercentMoveSpeedModifiers { get { return m_PercentMoveSpeedModifiers; } }

    private float FinalMoveSpeed()
    {
        float FinalMoveSpeed = (m_MoveSpeed + AggregateRawMoveSpeedModifiers()) * AggregatePercentMoveSpeedModifiers();
        return FinalMoveSpeed;
    }

    private float AggregateRawMoveSpeedModifiers()
    {
        float FinalRawMoveSpeedMod = 0.0f;
        foreach(float RawMSMod in RawMoveSpeedModifiers)
        {
            FinalRawMoveSpeedMod += RawMSMod;
        }

        return FinalRawMoveSpeedMod;
    }

    private float AggregatePercentMoveSpeedModifiers()
    {
        float FinalPercentMoveSpeedMod = 1.0f;
        foreach(float PercentMSMod in PercentMoveSpeedModifiers)
        {
            FinalPercentMoveSpeedMod += PercentMSMod;
        }

        return FinalPercentMoveSpeedMod;
    }

    public bool AddRawMoveSpeedModifier(float i_RawMoveSpeedModifier)
    {
        m_RawMoveSpeedModifiers.Add(i_RawMoveSpeedModifier);
        return true;
    }

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    public bool AddPercentMoveSpeedModifier(float i_PercentMoveSpeedModifier)
    {
        float ClampedPercentMoveSpeedMod = Mathf.Clamp01(i_PercentMoveSpeedModifier);
        m_PercentMoveSpeedModifiers.Add(ClampedPercentMoveSpeedMod);
        return true;
    }
    #endregion


    #region TurnRate
    private float m_TurnRate = 0.0f;
    private List<float> m_RawTurnRateModifiers = new List<float>();
    private List<float> m_PercentTurnRateModifiers = new List<float>();
    public float TurnRate { get { return FinalTurnRate(); } }
    public List<float> RawTurnRateModifiers { get { return m_RawTurnRateModifiers; } }
    public List<float> PercentTurnRateModifiers { get { return m_PercentTurnRateModifiers; } }
    private float FinalTurnRate()
    {
        float FinalTurnRate = (m_TurnRate + AggregateRawTurnRateModifiers()) * AggregatePercentTurnRateModifiers();
        return FinalTurnRate;
    }

    private float AggregateRawTurnRateModifiers()
    {
        float FinalRawTurnRateMod = 0.0f;
        foreach (float RawTurnRateMod in RawTurnRateModifiers)
        {
            FinalRawTurnRateMod += RawTurnRateMod;
        }

        return FinalRawTurnRateMod;
    }

    private float AggregatePercentTurnRateModifiers()
    {
        float FinalPercentTurnRateMod = 1.0f;
        foreach (float PercentTurnRateMod in PercentTurnRateModifiers)
        {
            FinalPercentTurnRateMod += PercentTurnRateMod;
        }

        return FinalPercentTurnRateMod;
    }

    public bool AddRawTurnRateModifier(float i_RawTurnRateModifier)
    {
        m_RawTurnRateModifiers.Add(i_RawTurnRateModifier);
        return true;
    }

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    public bool AddPercentTurnRateModifier(float i_PercentTurnRateModifier)
    {
        float ClampedPercentTurnRateMod = Mathf.Clamp01(i_PercentTurnRateModifier);
        m_PercentTurnRateModifiers.Add(ClampedPercentTurnRateMod);
        return true;
    }
    #endregion


    #region HealthRegen
    private float m_HealthRegen = 0.0f;
    private List<float> m_RawHealthRegenModifiers = new List<float>();
    private List<float> m_PercentHealthRegenModifiers = new List<float>();
    public float HealthRegen { get { return FinalHealthRegen(); } }
    public List<float> RawHealthRegenModifiers { get { return m_RawHealthRegenModifiers; } }
    public List<float> PercentHealthRegenModifiers { get { return m_PercentHealthRegenModifiers; } }

    private float FinalHealthRegen()
    {
        float FinalHealthRegen = (m_HealthRegen + AggregateRawHealthRegenModifiers()) * AggregatePercentHealthRegenModifiers();
        return FinalHealthRegen;
    }

    private float AggregateRawHealthRegenModifiers()
    {
        float FinalRawHealthRegenMod = 0.0f;
        foreach (float RawHealthRegenMod in RawHealthRegenModifiers)
        {
            FinalRawHealthRegenMod += RawHealthRegenMod;
        }

        return FinalRawHealthRegenMod;
    }

    private float AggregatePercentHealthRegenModifiers()
    {
        float FinalPercentHealthRegenMod = 1.0f;
        foreach (float PercentHealthRegenMod in PercentHealthRegenModifiers)
        {
            FinalPercentHealthRegenMod += PercentHealthRegenMod;
        }

        return FinalPercentHealthRegenMod;
    }

    public bool AddRawHealthRegenModifier(float i_RawHealthRegenModifier)
    {
        m_RawHealthRegenModifiers.Add(i_RawHealthRegenModifier);
        return true;
    }

    //Takes in percentage as a ratio from 0 to 1, clamps to 0 or 1 if outside the range
    public bool AddPercentHealthRegenModifier(float i_PercentHealthRegenModifier)
    {
        float ClampedPercentHealthRegenMod = Mathf.Clamp01(i_PercentHealthRegenModifier);
        m_PercentHealthRegenModifiers.Add(ClampedPercentHealthRegenMod);
        return true;
    }
    #endregion


    public void Init(Unit i_Unit, float i_MoveSpeed, float i_TurnRate)
    {
        m_Unit = i_Unit;
        m_MoveSpeed = i_MoveSpeed;
        m_TurnRate = i_TurnRate;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



}
