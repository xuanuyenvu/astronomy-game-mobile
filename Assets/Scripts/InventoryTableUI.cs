using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTableUI : MonoBehaviour
{
    [SerializeField]
    private InventoryItemUI itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    public List<InventoryItemUI> listOfItemsUI = new List<InventoryItemUI>();

    public void InitializeInventoryUI(int inventorySize)
    {
        for(int i = 0; i < inventorySize; i++)
        {
            InventoryItemUI itemUI = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            itemUI.transform.SetParent(contentPanel);
            listOfItemsUI.Add(itemUI);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
