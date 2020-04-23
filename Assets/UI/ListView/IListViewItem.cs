
public interface IListViewItem<T> where T : new()
{
    void Setup(ListViewItemModel<T> model);
    int ItemHeight { get; }
}
