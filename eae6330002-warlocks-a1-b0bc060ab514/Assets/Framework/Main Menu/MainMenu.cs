using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour, IAppAware {

    [SerializeField]
    private App app = null;
    public App App { get { return app; } set { app = value; } }

    public void StartGame() {
        app.Scenes.LoadGame();
    }

    public void ExitGame() {
        app.Exit();
    }
	
}
