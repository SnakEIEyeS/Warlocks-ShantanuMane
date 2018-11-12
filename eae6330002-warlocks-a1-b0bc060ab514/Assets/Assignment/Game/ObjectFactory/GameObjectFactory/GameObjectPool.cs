using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : UnityObjectPool<GameObject>
{

    [SerializeField]
    private int m_InitialSize = 10;
    [SerializeField]
    private bool m_AutoFill = true;
    [SerializeField]
    private int m_MinSize = 1;
    [SerializeField]
    private bool m_CreateIfNecessary = true;

#region UnityObjectPool
    [SerializeField]
    private List<GameObject> m_GameObjectList = new List<GameObject>();

    public override int InitialSize { get { return m_InitialSize; } set { m_InitialSize = value; } }
    public override bool AutoFill { get { return m_AutoFill; } }
    public override int MinSize { get { return m_MinSize; } }
    public override bool CreateIfNecessary { get { return m_CreateIfNecessary; } }

    public override GameObject Get()
    {
        if(m_GameObjectList.Count > m_MinSize)
        {
            GameObject RetGameObj = m_GameObjectList[0];
            m_GameObjectList.RemoveAt(0);
            return RetGameObj;
        }
        else if(CreateIfNecessary)
        {
            return (GameObject)Instantiate(prefab);
        }
        else
        {
            return null;
        }
    }
    public override void Return (GameObject t)
    {
        //if(t == prefab)
        {
            print("Return called");
            m_GameObjectList.Add(t);
        }
       
    }
    public override void Resize(int count) {
        if (count >= MinSize)
        {
            m_GameObjectList.Capacity = count;
        }
    }
#endregion

    // Use this for initialization
    void Start () {
		if(AutoFill)
        {
            Initialize();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetGameObject() { return m_GameObjectList[0]; }
    public override GameObject GetPrefab() { return prefab; }
        private void Initialize()
    {
        for(int i = 0; i < InitialSize; i++)
        {
            m_GameObjectList.Add((GameObject)Instantiate(prefab));
        }
    }
}
