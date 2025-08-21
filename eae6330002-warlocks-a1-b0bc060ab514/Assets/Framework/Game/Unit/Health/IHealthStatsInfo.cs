public interface IHealthStatsInfo {

    IHealthSource Source { get; }

    float Current { get; }
    float Max { get; }
	
}
