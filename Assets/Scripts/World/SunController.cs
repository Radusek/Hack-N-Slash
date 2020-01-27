using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    private Light light;

    [SerializeField]
    [Range(0f, 1f)]
    private float dayFractionElapsed;

    [SerializeField]
    private float dayDuration = 10f;

    [SerializeField]
    private float nightTimeScale = 10f;

    [SerializeField]
    private Gradient colorGradient;


    private void Awake()
    {
        light = GetComponent<Light>();
    }
    
    void Update()
    {
        AddElapsedTime();
        SetLightDirection();
        SetLightIntensity();
        SetLightColor();
    }


    private void AddElapsedTime()
    {
        float timeScale = dayFractionElapsed > 0.5f ? nightTimeScale : 1f;
        dayFractionElapsed += timeScale * Time.deltaTime / dayDuration;
        if (dayFractionElapsed > 1f)
            dayFractionElapsed -= 1f;
    }

    private void SetLightDirection()
    {
        light.transform.localRotation = Quaternion.Euler(new Vector3(360f * dayFractionElapsed, 0f, 0f));
    }

    private void SetLightIntensity()
    {
        float newIntensity = Vector3.Dot(light.transform.forward, Vector3.down);
        newIntensity *= 1.25f;
        newIntensity = Mathf.Clamp01(newIntensity);
        light.intensity = newIntensity;
    }

    private void SetLightColor()
    {
        light.color = colorGradient.Evaluate(2f * dayFractionElapsed);
    }
}
