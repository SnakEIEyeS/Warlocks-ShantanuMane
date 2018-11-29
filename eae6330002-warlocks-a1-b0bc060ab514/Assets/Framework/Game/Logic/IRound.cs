using System.Collections.Generic;

public interface IRound<PhaseType,EventType> {
    
	List<IUnit> AliveUnits { get; }
    List<IPlayer> AlivePlayers { get; }
    IPlayer Winner { get; }

    PhaseType Phase { get; }
    void StartRound();

    EventType OnPhaseChanged { get; }

}
