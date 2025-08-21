using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnityObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : Object
{



    [SerializeField]

    protected T prefab = null;



    [SerializeField]

    protected UnityAction<T> prepare = null;



    public abstract int InitialSize { get; set; }

    public abstract bool AutoFill { get; }

    public abstract int MinSize { get; }

    public abstract bool CreateIfNecessary { get; }



    public abstract T Get();

    //public abstract IEnumerable<T> Get(int count);



    public abstract void Return(T t);

    //public abstract void Return(IEnumerable<T> ts);



    public abstract void Resize(int count);

    public abstract T GetPrefab();

}
