using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    [SerializeField]
    private int inventorySize = 20;

    private bool pickUpItem;

    public UnityEvent OnInventoryChanged;

    private Coroutine cancelPickupCoroutine;

    public bool WantsToPickUp()
    {
        return pickUpItem;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        pickUpItem = false;
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        OnInventoryChanged?.Invoke();
    }

    public int GetItemsCount()
    {
        return items.Count;
    }

    public Item GetItem(int index)
    {
        return items[index];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && items.Count < inventorySize)
        {
            pickUpItem = true;
            if (cancelPickupCoroutine != null)
                StopCoroutine(cancelPickupCoroutine);
            cancelPickupCoroutine = StartCoroutine(CancelPickUpCoroutine());
        }
    }

    private IEnumerator CancelPickUpCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        pickUpItem = false;
        cancelPickupCoroutine = null;
    }
}
