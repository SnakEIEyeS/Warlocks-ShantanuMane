using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour, IHealth {

    [SerializeField]
    private float MaxHealth = 100f;

    //[SerializeField]
    private float CurrentHealth;

    private DamageDealer m_LastDamageDealer = null;
    

#region IHealth

    public float Current { get { return CurrentHealth; } set { CurrentHealth = value; } }
    public float Max { get { return MaxHealth; } set { MaxHealth = value; } }
    public float Percent { get { return (float)((Current/Max) * 100); } }
    public bool IsAlive { get { return (Current > 0f); } }
    public bool IsDead { get { return (Current <= 0f); } }
    public void Damage(IDamageInfo damageInfo) { Current -= damageInfo.Amount; }
    public void SetStats(IHealthStatsInfo stats) { return; }

    #endregion

    #region IDamageable
    public IDamageDealer DamageDealer { get { return m_LastDamageDealer; } set { m_LastDamageDealer = value as DamageDealer; } }

    public void TakeDamage(IDamageInstance i_DamageInstance)
    {
        if (!IsDead)
        {
            if(i_DamageInstance.DamageDealer != null)
            {
                DamageDealer = i_DamageInstance.DamageDealer;
            }
            Current -= i_DamageInstance.DamageAmount;

            if(IsDead)
            {
                RelayDeath();
                FindObjectOfType<KillEventBus>().DeathEvent.Invoke(this, m_LastDamageDealer);
            }
        }
    }
    #endregion

    void Start()
    {
        Current = Max;
        
    }

    void Update()
    {
        //print(gameObject.name + " Health: " + Current);
        
    }

    public void TakeDamage( float i_Damage)
    {
        //print("TakeDamage called");
        if(!IsDead)
        {
            Current -= i_Damage;
            if(IsDead)
            {
                RelayDeath();
            }
        }
    }

    private void RelayDeath()
    {
        if (gameObject.GetComponentInChildren<Unit>() != null)
        {
            gameObject.GetComponentInChildren<Unit>().UnitDeath();
        }
    }

}
