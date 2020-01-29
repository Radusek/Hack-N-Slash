using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "HackNSlash/Items/Item")]
public class Item : ScriptableObject
{
    [HideInInspector]
    public GameObject owner;

    public string itemName = "New item";
    public string itemDescription = "Description";
    public Sprite icon = null;

    public int stackLimit = 1;

    // returns true if should be destroyed after using it
    public virtual bool Use() { return false; }
    public virtual string GetDescription() { return $"<b>{itemName}</b>"; }
}
