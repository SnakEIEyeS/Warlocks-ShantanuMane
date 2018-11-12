using UnityEngine;

public interface IPlayer {

	Color Color { get; }
	string Name { get; }

    bool IsLocal { get; }

}

