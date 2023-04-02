using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{

    public GameObject LoadingScreen;
    

    public void Loading(int SceneId)
    {

        

        StartCoroutine(LoadAsync(SceneId));
    }

    IEnumerator LoadAsync(int SceneId)
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(1);
        loadAsync.allowSceneActivation = false;
        LoadingScreen.SetActive(true);

        while (!loadAsync.isDone)
        {
            loadAsync.allowSceneActivation = true;
            yield return null;
        }
        
    }
}
