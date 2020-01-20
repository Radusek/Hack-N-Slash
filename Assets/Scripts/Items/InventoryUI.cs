using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [HideInInspector]
    public Inventory inventory;

    [SerializeField]
    private Transform itemsParent;

    private InventorySlot[] slots;

    [SerializeField]
    private GameObject descriptionObject;
    [SerializeField]
    private TextMeshProUGUI descriptionText;


    private void OnDisable()
    {
        DisableDescription();
    }

    private void Awake()
    {
        inventory = transform.parent.parent.GetComponent<HUDManager>().GetPlayer().GetComponent<Inventory>();

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        int occupiedSlots = inventory.GetItemsCount();

        for (int i = 0; i < occupiedSlots; i++)
            slots[i].SetItem(inventory.GetItem(i));

        for (int i = occupiedSlots; i < slots.Length; i++)
            slots[i].ClearSlot();
    }

    public void SetDescription(string description)
    {
        if (description == null)
            DisableDescription();

        descriptionObject.SetActive(true);
        descriptionText.text = description;
    }

    public void DisableDescription()
    {
        descriptionObject.gameObject.SetActive(false);
    }
}
