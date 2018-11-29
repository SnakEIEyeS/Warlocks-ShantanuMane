using UnityEngine;

public interface IUnitController {

    bool HasReachedDestination { get; }

	void MoveTo(Vector3 target);
	void StopAll();

}
