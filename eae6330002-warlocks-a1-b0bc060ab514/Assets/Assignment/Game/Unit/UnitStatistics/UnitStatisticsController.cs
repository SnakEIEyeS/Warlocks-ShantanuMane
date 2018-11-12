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
