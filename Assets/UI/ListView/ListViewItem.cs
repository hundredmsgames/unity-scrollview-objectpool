using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ListViewItem : MonoBehaviour, IListViewItem
{

    [SerializeField] Text itemText;

    public void Setup(IListViewItemModel model)
    {
        var itemModel = (ListViewItemModel) model;
        gameObject.name = itemModel.Data.ToString();
        itemText.text = ((int)(itemModel.Data)).ToString();
    }

}
