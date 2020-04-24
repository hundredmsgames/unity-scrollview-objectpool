using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ListViewItem : MonoBehaviour, IListViewItem
{

    [SerializeField] Text itemText;

    //We need to think about which object will give us the correct size
    public int ItemHeight { get { return (int)GetComponent<RectTransform>().rect.height; } }

    public void Setup(IListViewItemModel model)
    {
        //  Debug.Log(height);
        var itemModel = (ListViewItemModel)model;
        
        itemText.text = ((int)(itemModel.Data)).ToString();
    }

}
