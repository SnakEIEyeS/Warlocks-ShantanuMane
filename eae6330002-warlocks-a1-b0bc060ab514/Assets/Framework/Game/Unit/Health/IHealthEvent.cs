public interface IHealthEvent {

    IHealth Target { get; }

    float StartHealth { get; }
    float EndHealth { get; }
    float HealthChange { get; }

    float StartMaxHealth { get; }
    float EndMaxHealth { get; }

    IHealthSource Source { get; }

}
