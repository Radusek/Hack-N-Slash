using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSign : Interactable
{
    [SerializeField]
    [TextArea]
    private string content;


    public override string GetTooltipText()
    {
        return content;
    }
}
