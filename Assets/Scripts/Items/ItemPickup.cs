using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField]
    private Item item;

    private Inventory playerInventory;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != (int)Layer.Player || playerInventory != null)
            return;

        playerInventory = other.GetComponent<Inventory>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != (int)Layer.Player)
            return;

        if (other.gameObject != playerInventory.gameObject)
            return;

        if (playerInventory.WantsToPickUp())
        {
            item.owner = playerInventory.gameObject;
            playerInventory.AddItem(item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInventory = null;
    }
}
