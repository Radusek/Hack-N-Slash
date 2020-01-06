using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    private TextMeshProUGUI text;

    private Vector3 baseTextScale; 

    private float lastTimeDamaged;

    private int currentDamage;
    private int totalDamage;

    private float displayingTime = 1.5f;

    private Coroutine animateTextCoroutine;

    [SerializeField]
    private AnimationCurve curve;


    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        baseTextScale = text.rectTransform.localScale;
    }

    public void DamageTaken(int amount)
    {
        if (animateTextCoroutine != null)
            StopCoroutine(animateTextCoroutine);

        totalDamage += amount;
        animateTextCoroutine = StartCoroutine(AnimateTextCoroutine());
    }

    private IEnumerator AnimateTextCoroutine()
    {
        text.text = (-totalDamage).ToString();

        float startTime = Time.time;
        while (Time.time < startTime + displayingTime)
        {
            float fraction = (Time.time - startTime) / displayingTime;
            Vector3 newTextScale = baseTextScale * curve.Evaluate(fraction);
            text.rectTransform.localScale = newTextScale;
            yield return null;
        }

        currentDamage = 0;
        totalDamage = 0;
        animateTextCoroutine = null;
        text.text = string.Empty;
    }
}
