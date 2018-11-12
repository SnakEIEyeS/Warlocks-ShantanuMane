using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Scenes : MonoBehaviour {

    [SerializeField]
    private App app = null;

    [SerializeField]
    private string loadingScreenSceneName = "LoadingScreen";

    [SerializeField]
    private string gameSceneName = "WarlocksGame";

    [SerializeField]
    private string mainMenuSceneName = "MainMenu";

    public void GoToMainMenu() {
        LoadScene(mainMenuSceneName, false);
    }

    public void LoadGame() {
        LoadScene(gameSceneName);
    }

    private void LoadScene(string sceneName, bool showLoadingScreen = true) {

        if (showLoadingScreen) {
            StartCoroutine(Load_Coroutine(loadingScreenSceneName, SceneManager.GetActiveScene().name, (Scene loadingScene) => {
                OnLoadComplete(loadingScene);
                LoadingScreen loadingScreen = loadingScene.FindObject<LoadingScreen>();
                StartCoroutine(Load_Coroutine(sceneName, loadingScreenSceneName, OnLoadComplete, loadingScreen));
            }));
        } else {
            StartCoroutine(Load_Coroutine(sceneName, SceneManager.GetActiveScene().name, OnLoadComplete));
        }

    }

    IEnumerator Load_Coroutine(string sceneName, string unloadSceneName, UnityAction<Scene> onComplete, LoadingScreen loadingScreen = null) {

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadOp;

        Scene scene = SceneManager.GetSceneByName(sceneName);
        List<ISceneLoader> sceneLoaders = scene.FindObjects<ISceneLoader>();
        if(loadingScreen != null) {
            loadingScreen.SetLoaders(sceneLoaders);
        }
        foreach (ISceneLoader loader in sceneLoaders) {
            yield return StartCoroutine(loader.Load());
        }

        if (unloadSceneName != null) {
            Scene unloadScene = SceneManager.GetSceneByName(unloadSceneName);
            if (unloadScene.isLoaded) {
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(unloadScene);
                yield return unloadOp;
            }
        }

        if (onComplete != null) {
            onComplete(scene);
        }

    }

    private void OnLoadComplete(Scene scene) {
        BindAppAwares(scene);
        SceneManager.SetActiveScene(scene);
        NotifySceneReady(scene);
    }

    private void NotifySceneReady(Scene scene) {
        List<ISceneLoader> sceneLoaders = scene.FindObjects<ISceneLoader>();
        foreach (ISceneLoader loader in sceneLoaders) {
            loader.OnSceneReady();
        }
    }

    private void BindAppAwares(Scene scene) {
        List<IAppAware> appAwares = scene.FindObjects<IAppAware>();
        foreach (IAppAware appAware in appAwares) {
            appAware.App = app;
        }
    }

    //private List<T> FindObjectsInScene<T>(Scene scene) {
    //    List<T> objects = new List<T>();
    //    foreach (GameObject go in scene.GetRootGameObjects()) {
    //        T[] ts = go.GetComponentsInChildren<T>();
    //        objects.AddRange(ts);
    //    }
    //    return objects;
    //}
	
}
