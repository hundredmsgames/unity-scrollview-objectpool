using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ListViewItem : MonoBehaviour, IListViewItem
{

    [SerializeField] Text itemText;

    public int ItemHeight { get { return 100; } }

    public void Setup(IListViewItemModel model)
    {
        var itemModel = (ListViewItemModel) model;
        gameObject.name = ((int)(itemModel.Data)).ToString();
        itemText.text = ((int)(itemModel.Data)).ToString();
    }

}
