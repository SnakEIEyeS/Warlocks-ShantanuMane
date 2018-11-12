using System.Collections;

public interface ISceneLoader {

    IEnumerator Load();
    void OnSceneReady();
    float LoadProgress { get; }
    string LoadPhase { get; }

}
