using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characters;


    private void Awake()
    {
        characters[CharacterSelected.playerClass].SetActive(true);
    }
}
