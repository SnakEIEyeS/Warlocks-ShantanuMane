using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectFactory<T, U>
{

    T Get(U u);

    //IEnumerable<T> Get(int count);



    //void Return(T t);

    //void Return(IEnumerable<T> ts);

}
