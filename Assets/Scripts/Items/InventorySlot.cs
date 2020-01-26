using TMPro;
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
    [SerializeField]
    private TextMeshProUGUI stackNumberText;

    private Item item;

    
    public void SetItem(ItemStack itemStack)
    {
        item = itemStack.item;

        int stackNumber = itemStack.count;
        stackNumberText.gameObject.SetActive(stackNumber > 1);
        stackNumberText.text = stackNumber.ToString();

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        removeButton.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        removeButton.gameObject.SetActive(false);
        stackNumberText.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        if (item != null)
        {
            if (item.Use())
                inventoryUI.inventory.RemoveItem(item);
        }
    }

    public void OnRemoveButton()
    {
        inventoryUI.inventory.RemoveItem(item);
    }

    public void OnPointerEnter()
    {
        inventoryUI.SetDescription(item != null ? item.GetDescription() : null);
    }

    public void OnPointerExit()
    {
        inventoryUI.DisableDescription();
    }
}
