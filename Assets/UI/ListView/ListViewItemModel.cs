using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListViewItemModel<T> where T : new()
{
    public T Data { get { return data; } }

    T data;

    public ListViewItemModel(T data)
    {
        this.data = data;
    }
}
