using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryTableUI inventoryUI;

    public int inventorySize = 10;

    private void Start() 
    {
        inventoryUI.Show();
        inventoryUI.InitializeInventoryUI(inventorySize);
    }
}