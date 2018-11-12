using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour {
    
    [SerializeField]
    private Scenes scenes = null;
    public Scenes Scenes { get { return scenes; } }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void Exit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


	
}
