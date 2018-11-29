using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class HealthTest : IHealthSource, IDamageSource {

    private static int instanceCounter = 0;

    [TestCase(100, 100)]
    [TestCase(0, 100)]
    [Test]
    public void Test_Setters(float newHealth, float maxHealth) {
        Health health = CreateInstance(newHealth, maxHealth);
        Assert.AreEqual(health.Max, maxHealth);
        Assert.AreEqual(health.Current, newHealth);
        DestroyInstance(health);
    }

    [TestCase(100, 100, 1f)]
    [TestCase(0, 100, 0f)]
    [TestCase(50, 100, 0.5f)]
    [TestCase(25, 100, 0.25f)]
    [TestCase(10, 100, 0.1f)]
    [Test]
    public void Test_Percentage(float current, float maxHealth, float percentage) {
        Health health = CreateInstance(current, maxHealth);
        Assert.AreEqual(health.Percent, percentage);
        DestroyInstance(health);
    }

    [TestCase(100, 10, 90)]
    [TestCase(100, 100, 0)]
    [TestCase(10, 11, 0)]
    [TestCase(0, 5, 0)]
    [Test]
    public void Test_Damage(float startHealth, float damage, float endHealth) {
        Health health = CreateInstance(startHealth, startHealth);

        DamageInfo damageInfo = new DamageInfo(this, damage, DamageType.Normal);
        health.Damage(damageInfo);
        Assert.AreEqual(health.Current, endHealth);

        DestroyInstance(health);
    }

    [Test]
    [TestCase(10, 100, false)]
    [TestCase(0, 100, true)]
    public void Test_IsDead(float startHealth, float maxHealth, bool expected) {
        Health health = CreateInstance(startHealth, maxHealth);
        Assert.AreEqual(expected, health.IsDead);
        DestroyInstance(health);
    }

    [Test]
    [TestCase(10, 100, true)]
    [TestCase(0, 100, false)]
    public void Test_IsAlive(float startHealth, float maxHealth, bool expected) {
        Health health = CreateInstance(startHealth, maxHealth);
        Assert.AreEqual(expected, health.IsAlive);
        DestroyInstance(health);
    }

    private Health CreateInstance(float currentHealth, float maxHealth) {
        GameObject gameObject = new GameObject(string.Format("Test Health ({0})", instanceCounter++));
        Health health = gameObject.AddComponent<Health>();

        HealthStatsInfo stats = new HealthStatsInfo(this, currentHealth, maxHealth);
        health.SetStats(stats);
        return health;
    }

    private void DestroyInstance(Health health) {
        Object.DestroyImmediate(health.gameObject);
    }
}
