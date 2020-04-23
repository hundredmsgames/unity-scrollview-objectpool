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

    [SerializeField] float ItemHeight = 1;      // TODO: Replace it with dynamic height
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



    public void Setup(ListViewItemModel[] data)
    {
        ScrollRect.onValueChanged.AddListener(OnDragDetectionPositionChange);

        this.data = data;
        int Length = data.Length;
        DragDetectionT.sizeDelta = new Vector2(DragDetectionT.sizeDelta.x, Length * ItemHeight + (Length - 1) * spacing);
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

        for (int i = 0; i < data.Length; i++)
        {
            int itemType = data[i].GetItemType(); 
            modelToItem.Add(data[i], components[i % components.Length][itemType]);
        }
    }



    #region UI Event Handling

    public void OnDragDetectionPositionChange(Vector2 dragNormalizePos)
    {
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
            ContentT.anchoredPosition = new Vector2(ContentT.anchoredPosition.x, ContentT.anchoredPosition.y - item.ItemHeight - spacing);
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
            ContentT.anchoredPosition = new Vector2(ContentT.anchoredPosition.x, ContentT.anchoredPosition.y + item.ItemHeight + spacing);
        }
    }
    //Diyelim 5 tane obje aktif ilk 5 obje ilk 5 veriye karşılık geliyor.
    //aşağı scroll yaptığım zaman yukarıya çıkan 1. obje sınırı geçince
    //aşağı inecek ve 6. veri 1. objeye yazılacak. yani her veri her zaman
    //aynı objeye denk gelecek.
    ///
    /// 
    /// Pool bize bir item verdiğimizde ona karşılık gelen listviewItem'ı
    //dönderebilsin. ListViewItem için bir çatımız var ondan ayrı olarak biz
    //içeriği özel olarak dolduracağız zaten. Oraya UpdateUI metodlarımızı
    //yazarız ve income değiştiği zaman update edilmesi gerekenler update edilebilir.
    /// 
    #endregion
}
