using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFactory : MonoBehaviour, IObjectFactory<AbilityObjectPool, GameObject>
{

    [SerializeField]
    private List<AbilityObjectPool> m_AbilityObjectPools = new List<AbilityObjectPool>();

    #region IObjectFactory
    public AbilityObjectPool Get(GameObject i_GameObject)
    {
        foreach (AbilityObjectPool GOPool in m_AbilityObjectPools)
        {
            if (GOPool.GetPrefab() == i_GameObject)
            {
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
