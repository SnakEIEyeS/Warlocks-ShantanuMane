public interface IHealth {

    float Current { get; }
    float Max { get; }
    float Percent { get; }  /* 0 to 1 */

    bool IsAlive { get; }
    bool IsDead { get; }

    void SetStats(IHealthStatsInfo stats);

    void Damage(IDamageInfo damageInfo);

}
