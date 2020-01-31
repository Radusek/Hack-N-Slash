using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationEnd : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToEnable;


    public void EnableObject()
    {
        objectToEnable.SetActive(true);
    }
}
