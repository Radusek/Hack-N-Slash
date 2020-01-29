using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializeField]
    private LootInfo lootInfo;


    public void GenerateLoot()
    {
        List<GameObject> generatedLoot = new List<GameObject>();

        for (int i = 0; i < lootInfo.possibleLoot.Length; i++)
        {
            for (int j = 0; j < lootInfo.possibleLoot[i].maxAmount; j++)
            {
                float roll = Random.value;
                if (roll < lootInfo.possibleLoot[i].dropChance)
                    generatedLoot.Add(lootInfo.possibleLoot[i].item);
            }
        }

        if (generatedLoot.Count > 0)
            StartCoroutine(DropItemsCoroutine(generatedLoot));
    }

    private IEnumerator DropItemsCoroutine(List<GameObject> generatedLoot)
    {
        float droppingRange = 0.5f;

        int lootCount = generatedLoot.Count;
        var delay = new WaitForSeconds(0.15f);
        for (int i = 0; i < lootCount; i++)
        {
            yield return delay;
            float angle = (float)i / lootCount * 2f * Mathf.PI;
            Vector3 offsetVector = droppingRange * (Mathf.Cos(angle) * Vector3.right + Mathf.Sin(angle) * Vector3.forward);
            Vector3 itemSpawnPosition = transform.position + offsetVector;
            GameObject go = Instantiate(generatedLoot[i], itemSpawnPosition, Quaternion.identity);
            go.name = GetOriginalObjectName(go.name);
        }
    }

    private string GetOriginalObjectName(string name)
    {
        //removing "(clone)" string from instantiated object
        return name.Remove(name.Length - 7, 7);
    }
}

