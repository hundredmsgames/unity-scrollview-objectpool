using System;

[Serializable]
public class ListViewItemModel : IListViewItemModel
{
    public System.Object Data { get { return data; } }

    System.Object data;

    public ListViewItemModel(System.Object data)
    {
        this.data = data;
    }

    public int GetItemType()
    {
        return 0;
    }
}
