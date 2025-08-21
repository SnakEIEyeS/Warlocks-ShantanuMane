using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    private List<ISceneLoader> loaders = null;

    [SerializeField]
    private Slider loadingBar = null;

    [SerializeField]
    private Text loadingLabel = null;

    private string loadingTitle = "Loading{0}{1}{2}";
    private string dot = ".";
    private int minDots = 1;
    private int maxDots = 3;
    private int curDots = 3;
    private float dotCycle = 1f;
    private float dotTime = 0;

    private string loadingPhase = null;
    private float loadProgress = 0f;

    public void SetLoaders(List<ISceneLoader> loaders) {
        this.loaders = loaders;
    }

    private void Update() {

        bool dirty = false;

        dotTime += Time.deltaTime;
        if(dotTime > dotCycle) {
            dotTime -= dotCycle;
            curDots = Mathf.Clamp((curDots + 1) % (maxDots+1), minDots, maxDots);
            dirty = true;
        }

        if (loaders != null) {
            foreach (ISceneLoader loader in loaders) {
                float newProgress = Mathf.Clamp01(loader.LoadProgress);

                if (newProgress >= 1f) {
                    continue;
                }

                loadProgress = newProgress;
                loadingPhase = loader.LoadPhase;
                dirty = true;
                break;
            }
        }

        if (dirty) {
            UpdateUI();
        }

    }

    private void UpdateUI() {
        loadingLabel.text = string.Format(
            loadingTitle,
            string.IsNullOrEmpty(loadingPhase) ? "" : " ",
            string.IsNullOrEmpty(loadingPhase) ? "" : loadingPhase,
            GetDotsString(curDots)
        );
        loadingBar.value = loadProgress;
    }

    private string GetDotsString(int count) {
        string dots = "";
        for (int i = 0; i < count; i++) {
            dots += dot;
        }
        return dots;
    }

}
