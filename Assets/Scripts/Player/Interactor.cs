using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float interactionRange = 1f;

    private HUDManager hudManager;

    private void Awake()
    {
        hudManager = FindObjectOfType<HUDManager>();
    }

    void Update()
    {
        if (!hudManager.GetInputEnabled())
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            UseInteractable();
    }

    private void UseInteractable()
    {
        Collider[] interactables = Physics.OverlapSphere(transform.position, interactionRange, (1 << (int)Layer.Interactable));
        if (interactables.Length > 0)
            interactables[0].GetComponent<Interactable>().Interact(gameObject);
    }
}
