using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MinimapMarkGuard : MonoBehaviour
{
    [SerializeField]
    private Material allyMaterial;

    private void Start()
    {
        if (transform.parent.gameObject.layer == (int)Layer.Player)
            GetComponent<Renderer>().material = allyMaterial;

        Destroy(this);
    }
}
