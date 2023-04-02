using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSciriptLS : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LoadingScreen;
    void Start()
    {
        StartCoroutine(LoadAsync());
    }
    IEnumerator LoadAsync()
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(1);
        loadAsync.allowSceneActivation = false;
        LoadingScreen.SetActive(true);

        while (!loadAsync.isDone)
        {

            if(loadAsync.progress >=0.9f && !loadAsync.allowSceneActivation)
            {
                yield return new WaitForSeconds(1.5f);
                loadAsync.allowSceneActivation = true;

            }

            yield return null;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
