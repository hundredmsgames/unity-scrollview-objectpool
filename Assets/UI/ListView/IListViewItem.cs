
public interface IListViewItem
{
    void Setup(IListViewItemModel model);
    int ItemHeight { get; }
}