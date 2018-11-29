using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFactory : MonoBehaviour, IObjectFactory<GameObjectPool, GameObject> {

    [SerializeField]
    private List<GameObjectPool> m_GameObjectPools = new List<GameObjectPool>();

#region IObjectFactory
    public GameObjectPool Get(GameObject i_GameObject)
    {
        foreach(GameObjectPool GOPool in m_GameObjectPools)
        {
            if(GOPool.GetPrefab() == i_GameObject)
            {
                print("GOPool found!");
                return GOPool;
            }
        }
        return null;
    }

    /*public void Return(GameObject i_GameObject)
    {
        System.Type type = i_GameObject.GetType();
        print(i_GameObject.GetType());
        foreach (GameObjectPool GOPool in m_GameObjectPools)
        {
            if (GOPool.GetGameObject().GetComponent<Component>().GetType() == i_GameObject.GetComponent<Component>().GetType())
            {
                GOPool.Return(i_GameObject);
                return;
            }
        }
    }*/
#endregion
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
