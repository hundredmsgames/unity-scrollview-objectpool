using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PooledListView listView;
    [SerializeField] int DataCount;

    void Start()
    {
        ListViewItemModel<System.Object>[] demoData = new ListViewItemModel<System.Object>[DataCount];
        for(int i = 0; i < DataCount; i++)
        {
            demoData[i] = new ListViewItemModel<System.Object>(i + 1);
        }

        listView.Setup(demoData);
    }
}
