using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Item item = (Item)target;

        item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
        item.itemDescription = EditorGUILayout.TextField("Item Description", item.itemDescription);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Icon");
        item.icon = (Sprite)EditorGUILayout.ObjectField(item.icon, typeof(Sprite), allowSceneObjects: true);
        EditorGUILayout.EndHorizontal();

        item.stackLimit = EditorGUILayout.IntField("Stack Limit", item.stackLimit);
    }
}
