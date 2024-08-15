using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontDoor : InteractParent
{
    [SerializeField] float waitTime;
    Coroutine loadSceneRoutine;

    private void Start()
    {
        loadSceneRoutine = null;
    }

    public override void Interact()
    {
        if (loadSceneRoutine == null)
            loadSceneRoutine = StartCoroutine(LoadSceneRoutine());
    }

    IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        //Fade to black
        SceneManager.LoadScene("House");

        loadSceneRoutine = null;
    }
}
