using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PooledListView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region Child Components

    [SerializeField] ScrollRect ScrollRect;
    [SerializeField] RectTransform viewPortT;
    [SerializeField] RectTransform DragDetectionT;
    [SerializeField] RectTransform ContentT;
    [SerializeField] ListViewItemPool ItemPool;

    #endregion



    #region Layout Parameters

    float ItemHeight;
    [SerializeField] int BufferSize;

    #endregion



    #region Layout Variables

    float spacing { get { return ContentT.GetComponent<VerticalLayoutGroup>().spacing; } }
    int TargetVisibleItemCount { get { return Mathf.Max(Mathf.CeilToInt(viewPortT.rect.height / ItemHeight), 0); } }
    int TopItemOutOfView { get { return Mathf.CeilToInt(ContentT.anchoredPosition.y / (ItemHeight + spacing)); } }
    float dragDetectionAnchorPreviousY = 0;

    #endregion



    #region Data

    IListViewItemModel[] data;
    int dataHead = 0;
    int dataTail = 0;

    Dictionary<IListViewItemModel, IListViewItem> modelToItem = new Dictionary<IListViewItemModel, IListViewItem>(50);
    #endregion



    public void Setup(IListViewItemModel[] data)
    {
        ScrollRect.onValueChanged.AddListener(OnDragDetectionPositionChange);

        this.data = data;
        this.ItemHeight = ItemPool.ItemHeight;
        int length = data.Length;
        DragDetectionT.sizeDelta = new Vector2(DragDetectionT.sizeDelta.x, length * ItemHeight + (length - 1) * spacing);
        IListViewItem[][] components = new IListViewItem[TargetVisibleItemCount + BufferSize][];

        for (int i = 0; i < TargetVisibleItemCount + BufferSize; i++)
        {
            GameObject itemGO = ItemPool.ItemBorrow();
            itemGO.transform.SetParent(ContentT);
            itemGO.SetActive(true);
            itemGO.transform.localScale = Vector3.one;
            var items = itemGO.GetComponents<IListViewItem>();
            var itemModel = data[dataTail];
            int itemType = itemModel.GetItemType();
            items[itemType].Setup(itemModel);
            components[i] = items;
            dataTail++;
        }

        for (int i = 0; i < length; i++)
        {
            int itemType = data[i].GetItemType();
            modelToItem.Add(data[i], components[i % components.Length][itemType]);
        }
    }

    /// <summary>
    /// This method will be returning IListViewItem with
    /// respect to given model.
    /// </summary>
    /// <returns>Matched IListViewItem</returns>
    public IListViewItem GetItem(IListViewItemModel itemModel)
    {
        return modelToItem[itemModel];
    }

    /// <summary>
    /// This method will return an array of IListViewItem that is visible.
    /// Thus we can update it or we can do whatever we want.
    /// </summary>
    /// <returns></returns>
    public KeyValuePair<IListViewItemModel, IListViewItem>[] GetAllItemsInContent()
    {
        KeyValuePair<IListViewItemModel, IListViewItem>[] modelItemPairs =
            new KeyValuePair<IListViewItemModel, IListViewItem>[dataTail - dataHead];

        for (int i = dataHead; i < dataTail; ++i)
        {
            modelItemPairs[i - dataHead] = new KeyValuePair<IListViewItemModel, IListViewItem>(
                data[i],
                modelToItem[data[i]]
            );
        }
        return modelItemPairs;
    }



    #region UI Event Handling

    public void OnDragDetectionPositionChange(Vector2 dragNormalizePos)
    {
		if(Input.touchCount > 1)
			return;
		
        float dragDelta = DragDetectionT.anchoredPosition.y - dragDetectionAnchorPreviousY;

        ContentT.anchoredPosition = new Vector2(ContentT.anchoredPosition.x, ContentT.anchoredPosition.y + dragDelta);

        UpdateContentBuffer();

        dragDetectionAnchorPreviousY = DragDetectionT.anchoredPosition.y;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        dragDetectionAnchorPreviousY = DragDetectionT.anchoredPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    #endregion



    #region Infinite Scroll Mechanism

    void UpdateContentBuffer()
    {
        if (TopItemOutOfView > BufferSize)
        {
            if (dataTail >= data.Length)
            {
                return;
            }

            Transform firstChildT = ContentT.GetChild(0);
            firstChildT.SetSiblingIndex(ContentT.childCount - 1);
            var model = data[dataTail];
            var item = modelToItem[model];
            item.Setup(model);
            ContentT.anchoredPosition = new Vector2(ContentT.anchoredPosition.x, ContentT.anchoredPosition.y - ItemHeight - spacing);
            dataHead++;
            dataTail++;
        }
        else if (TopItemOutOfView < BufferSize)
        {
            if (dataHead <= 0)
            {
                return;
            }

            Transform lastChildT = ContentT.GetChild(ContentT.childCount - 1);
            lastChildT.SetSiblingIndex(0);
            dataHead--;
            dataTail--;
            var model = data[dataHead];
            var item = modelToItem[model];
            item.Setup(model);
            ContentT.anchoredPosition = new Vector2(ContentT.anchoredPosition.x, ContentT.anchoredPosition.y + ItemHeight + spacing);
        }
    }

    #endregion
}
