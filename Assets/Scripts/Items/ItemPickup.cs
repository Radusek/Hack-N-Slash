using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField]
    private Item item;


    public override void Interact(GameObject interactor)
    {
        Inventory interactorInventory = interactor.GetComponent<Inventory>();
        if (!interactorInventory.CanPickItem(item))
            return;

        StartCoroutine(FlyTowardsInteractorCoroutine(interactorInventory));
    }

    private IEnumerator FlyTowardsInteractorCoroutine(Inventory interactorInventory)
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.enabled = false;
        GameObject interactor = interactorInventory.gameObject;
        float distanceTreshold = 0.1f;
        float movingSpeed = 5f;
        while ((interactor.transform.position - transform.position).sqrMagnitude > distanceTreshold * distanceTreshold)
        {
            Vector3 relativeVector = interactor.transform.position - transform.position;
            relativeVector.y = 0f;
            transform.Translate(movingSpeed * relativeVector.normalized * Time.deltaTime, Space.World);
            transform.LookAt(interactor.transform);
            yield return null;
        }

        if (!interactorInventory.CanPickItem(item))
        {
            collider.enabled = true;
            yield break;
        }

        item.owner = interactor;
        interactorInventory.AddItem(item);
        Destroy(gameObject);
    }
}
