using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float interactionRange = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            UseInteractable();
    }

    private void UseInteractable()
    {
        Collider[] interactables = Physics.OverlapSphere(transform.position, interactionRange, (1 << (int)Layer.Interactable));
        if (interactables.Length > 0)
            interactables[0].GetComponent<Interactable>().Interact(gameObject);
    }
}
