using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField]
    private Item item;


    public override void Interact(GameObject interactor)
    {
        Inventory playerInventory = interactor.GetComponent<Inventory>();
        if (!playerInventory.CanPickItem(item))
            return;

        item.owner = interactor;
        playerInventory.AddItem(item);
        Destroy(gameObject);
    }
}
