using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour {

    [SerializeField]
    private App app = null;

    [SerializeField]
    private float waitTime = 3f;

    void Start() {
        StartCoroutine(Start_Coroutine());
    }

    IEnumerator Start_Coroutine() {
        yield return new WaitForSeconds(waitTime);
        app.Scenes.GoToMainMenu();
    }
}
