using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{
    public float radius = 1f;


    private void Awake()
    {
        GetComponent<SphereCollider>().radius = radius;
    }

    public virtual void Interact(){}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
