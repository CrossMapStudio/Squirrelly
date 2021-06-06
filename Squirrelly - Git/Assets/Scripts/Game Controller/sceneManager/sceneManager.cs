using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    private int sceneToLoad;
    private bool beginLoad;

    public static int currentLevelIndex = 0;
    private int playSceneIndex = 1;

    public void loadScene(int scene)
    {
        sceneToLoad = scene;
        beginLoad = true;
    }

    private void Update()
    {
        if (beginLoad)
        {
            StartCoroutine(loadAsync());
            beginLoad = false;
        }
    }

    IEnumerator loadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        //This may change^^
        if (SceneManager.GetActiveScene().buildIndex == playSceneIndex)
        {
            gameStateInit();
        }
    }

    private void gameStateInit()
    {
        //May need optimizing
        dataInterpreter local = gameObject.AddComponent(typeof(dataInterpreter)) as dataInterpreter;
        inputHandler input = gameObject.AddComponent(typeof(inputHandler)) as inputHandler;

        gameController c = GetComponent<gameController>();
        local.activeLevel = c.currentlySelectedLevel;
        //Will Vary based on the GameMode that was Selected + Difficulty
        local.unitList = c.currentlySelectedLevel.getTimeMode.units;
        local.unitLayer = c.unitLayer;
        local.enabled = true;
        local.initializeOnStart();
    }
}
