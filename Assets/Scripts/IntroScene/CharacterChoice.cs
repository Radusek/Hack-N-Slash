using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterChoice : MonoBehaviour
{
    [SerializeField]
    private int classId;

    private static AsyncOperation sceneLoadingOp;

    private void OnMouseDown()
    {
        if (sceneLoadingOp != null)
            return;

        CharacterSelected.playerClass = classId;
        StartCoroutine(StartLoadingSceneCoroutine());
    }

    private IEnumerator StartLoadingSceneCoroutine()
    {
        sceneLoadingOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //sceneLoadingOp.allowSceneActivation = false;

        while (!sceneLoadingOp.isDone)
            yield return null;

        Debug.Log("New Scene Loaded");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if(sceneLoadingOp != null)
                sceneLoadingOp.allowSceneActivation = true;
        }
    }
}

public static class CharacterSelected
{
    public static int playerClass = 0;
}
