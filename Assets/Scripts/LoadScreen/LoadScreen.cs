using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class LoadScreen : MonoBehaviour
{

    public GameObject LoadingScreen;
    public int SceneId;

    public void Loading(int SceneId)
    {
        if (SceneId.Equals(null)) SceneId = 1;
        StartCoroutine(LoadAsync(SceneId));
    }

    IEnumerator LoadAsync(int SceneId)
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(SceneId);
        loadAsync.allowSceneActivation = false;
        LoadingScreen.SetActive(true);

        while (!loadAsync.isDone)
        {
            loadAsync.allowSceneActivation = true;
            yield return null;
        }
        
    }
}
