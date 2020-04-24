using UnityEngine;

public class ListViewItemPool : MonoBehaviour
{

    [SerializeField] int PoolSize = 1;
    [SerializeField] GameObject PoolObjectPrefab;

    public int ItemHeight
    {
        get
        {
            return (int)PoolObjectPrefab.GetComponent<RectTransform>().rect.height;
        }
    }

    int head = 0;

    void Awake()
    {
        head = 0;

        for (int i = 0; i < PoolSize; i++)
        {
            GameObject poolObj = Instantiate(PoolObjectPrefab, this.transform) as GameObject;

            poolObj.SetActive(false);
        }
    }

    public GameObject ItemBorrow()
    {
        if (head >= PoolSize)
        {
            return null;
        }

        head++;
        return this.transform.GetChild(0).gameObject;
    }

    public void ItemReturn(GameObject go)
    {
        if (head <= 0)
        {
            return;
        }

        head--;
        go.SetActive(false);
        go.transform.SetParent(this.transform);
    }
}
