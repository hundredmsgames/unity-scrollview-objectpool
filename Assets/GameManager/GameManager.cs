using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PooledListView listView;
    [SerializeField] int DataCount;

    void Start()
    {
        ListViewItemModel[] demoData = new ListViewItemModel[DataCount];
        for (int i = 0; i < DataCount; i++)
        {
            demoData[i] = new ListViewItemModel(i + 1);
        }

        listView.Setup(demoData);
    }

    // FIXME: Don't forget deleting me after debugging
    void Update()
    {
        foreach (ListViewItem i in listView.GetVisibleItems())
        {
            Debug.Log(i.gameObject.name);
        }
        Debug.Log("-------");
    }
}
