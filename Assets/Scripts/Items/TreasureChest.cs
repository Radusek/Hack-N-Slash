using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TreasureChest : Interactable
{
    [SerializeField]
    private ItemPickup[] loot;
    [SerializeField]
    private Light light;

    public override void Interact(GameObject interactor)
    {
        gameObject.layer = (int)Layer.Default;
        StartCoroutine(LightCoroutine());
        GetComponent<Animator>().SetTrigger("Open");

        foreach (var item in loot)
        {
            ItemPickup ip = Instantiate(item, transform.position, Quaternion.identity);
            ip.name = item.item.itemName;
        }
    }

    private IEnumerator LightCoroutine()
    {
        light.enabled = true;

        float startTime = Time.time;
        float intensityVariation = 2f;
        int glowingCycles = 4;
        float cycleDuration = 1.6f;
        float durationInversed = 1f / cycleDuration;

        while(Time.time - startTime < glowingCycles * cycleDuration)
        {
            light.intensity = intensityVariation * (1f - (Mathf.Cos(2f * Mathf.PI * (Time.time - startTime) * durationInversed)*0.5f + 0.5f));
            yield return null;
        }
        
        light.enabled = false;
    }

    public override string GetTooltipText()
    {
        StringBuilder sb = new StringBuilder(base.GetTooltipText());
        sb.Append($"\nOpen {GetKeyName()}");
        return sb.ToString();
    }
}
