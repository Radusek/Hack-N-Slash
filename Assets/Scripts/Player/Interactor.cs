using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float interactionRange = 1f;

    [SerializeField]
    private GameObject interactableTooltipObject;
    [SerializeField]
    private TextMeshProUGUI interactableDescription;

    private Interactable currentInteractable;

    private HUDManager hudManager;

    private void Awake()
    {
        hudManager = FindObjectOfType<HUDManager>();
    }

    void Update()
    {
        if (!hudManager.GetInputEnabled())
            return;

        CheckForInteractables();

        if (Input.GetKeyDown(KeyCode.Space))
            UseInteractable();
    }

    private void UseInteractable()
    {
        if (currentInteractable == null)
            return;

        DisableTooltipObject();
        currentInteractable.Interact(gameObject);
        currentInteractable = null;
    }

    private void CheckForInteractables()
    { 
        if (currentInteractable != null)
        {
            if ((currentInteractable.transform.position - transform.position).sqrMagnitude <= interactionRange * interactionRange)
                return;
            else
            {
                DisableTooltipObject();
                currentInteractable = null;
                return;
            }
        }

        Collider[] interactables = Physics.OverlapSphere(transform.position, interactionRange, (1 << (int)Layer.Interactable));
        interactables = interactables.Where(i => (i.transform.position - transform.position).sqrMagnitude <= interactionRange * interactionRange).ToArray();
        if (interactables.Length > 0)
        {
            currentInteractable = interactables[0].GetComponent<Interactable>();
            interactableTooltipObject.transform.position = currentInteractable.transform.position + 2f * Vector3.up;
            interactableTooltipObject.transform.SetParent(currentInteractable.transform);
            interactableDescription.text = currentInteractable.GetTooltipText();
            interactableTooltipObject.SetActive(true);
        }
    }

    private void DisableTooltipObject()
    {
        interactableTooltipObject.transform.SetParent(transform);
        interactableTooltipObject.SetActive(false);
    }
}
