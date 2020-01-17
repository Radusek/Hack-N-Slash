using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "HackNSlash/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New item";
    public Sprite icon = null;


    public virtual void Use() {}
}
