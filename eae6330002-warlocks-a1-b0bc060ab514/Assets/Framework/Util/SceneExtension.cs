using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneExtension {

    public static List<T> FindObjects<T>(this Scene scene) {
        List<T> objects = new List<T>();
        foreach (GameObject go in scene.GetRootGameObjects()) {
            T[] ts = go.GetComponentsInChildren<T>();
            objects.AddRange(ts);
        }
        return objects;
    }

    public static T FindObject<T>(this Scene scene) {
        foreach (GameObject go in scene.GetRootGameObjects()) {
            T[] ts = go.GetComponentsInChildren<T>();
            if(ts.Length > 0) {
                return ts[0];
            }
        }
        return default(T);
    }
	
}
