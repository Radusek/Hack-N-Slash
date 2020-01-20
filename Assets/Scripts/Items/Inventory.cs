using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ItemStack
{
    public Item item;
    public int count;


    public void AddItem()
    {
        count++;
    }

    // returns true if should destroy stack
    public bool RemoveItem()
    {
        count--;
        if (count == 0)
        {
            item = null;
            return true;
        }

        return false;
    }

    public bool IsFull()
    {
        return count == item.stackLimit;
    }
}


public class Inventory : MonoBehaviour
{
    private List<ItemStack> itemStacks = new List<ItemStack>();

    [SerializeField]
    private int inventorySize = 20;

    public UnityEvent OnInventoryChanged;


    public void AddItem(Item item)
    {
        if (ShouldUseNewSlot(item))
        {
            ItemStack newStack = new ItemStack();
            newStack.item = item;
            newStack.count = 1;
            itemStacks.Add(newStack);
        }
        else
            StackUpItem(item);

        OnInventoryChanged?.Invoke();
    }

    private bool ShouldUseNewSlot(Item item)
    {
        foreach(var stack in itemStacks)
            if (stack.item == item && !stack.IsFull())
                return false;
        
        return true;
    }

    private void StackUpItem(Item item)
    {
        foreach(var stack in itemStacks)
        {
            if (stack.item == item && !stack.IsFull())
            {
                stack.AddItem();
                return;
            }
        }

        Debug.LogError("Items couldn't be added");
    }

    public void RemoveItem(Item item)
    {
        RemoveFromStack(item);
        OnInventoryChanged?.Invoke();
    }

    private void RemoveFromStack(Item item)
    {
        for (int i = itemStacks.Count - 1; i >= 0; i--)
        {
            if (itemStacks[i].item == item)
            {
                if (itemStacks[i].RemoveItem())
                    itemStacks.RemoveAt(i);
                return;
            }
        }
    }

    public int GetItemsCount()
    {
        return itemStacks.Count;
    }

    public ItemStack GetItem(int index)
    {
        return itemStacks[index];
    }

    public bool CanPickItem(Item item)
    {
        return itemStacks.Count < inventorySize || !ShouldUseNewSlot(item);
    }
}
