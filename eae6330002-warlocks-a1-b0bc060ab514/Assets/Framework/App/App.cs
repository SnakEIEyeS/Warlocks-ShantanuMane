using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour {
    
    [SerializeField]
    private Scenes scenes = null;
    public Scenes Scenes { get { return scenes; } }

    [SerializeField]
    private PUNPersistent m_PUNPersistent = null;
    public PUNPersistent PUNPersistentObj { get { return m_PUNPersistent; } }

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
