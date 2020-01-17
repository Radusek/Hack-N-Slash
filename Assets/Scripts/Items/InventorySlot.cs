using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Button removeButton;

    private Item item;

    
    public void SetItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void UseItem()
    {
        if (item != null)
            item.Use();
    }

    public void OnRemoveButton()
    {
        inventoryUI.inventory.RemoveItem(item);
    }
}
