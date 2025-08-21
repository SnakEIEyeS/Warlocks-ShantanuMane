using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool<T>
{

    T Get();

    //IEnumerable<T> Get(int count);



    void Return(T t);

    //void Return(IEnumerable<T> ts);

}