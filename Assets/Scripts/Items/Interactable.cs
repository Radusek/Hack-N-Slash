using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(GameObject interactor){}

    public virtual string GetTooltipText()
    {
        return $"<b>{gameObject.name}</b>";
    }

    protected string GetKeyName()
    {
        return "[Space]";
    }
}
