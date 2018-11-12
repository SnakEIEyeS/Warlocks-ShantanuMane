using System.Collections.Generic;

public interface IPlayerController {

    IPlayer Player { get; }
    List<IUnit> Units { get; }
	
}
