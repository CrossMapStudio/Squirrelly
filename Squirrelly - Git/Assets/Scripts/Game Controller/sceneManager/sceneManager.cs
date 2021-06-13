using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    private int sceneToLoad;
    private bool beginLoad;

    public static int currentLevelIndex = 0;
    private int mainMenuIndex = 0, playSceneIndex = 1;

    private gameController c;

    public void Start()
    {
        c = GetComponent<gameController>();
    }

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
        currentLevelIndex = sceneToLoad;
        if (currentLevelIndex == mainMenuIndex)
        {
            gameStateDestroy();
        }
        
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (currentLevelIndex == playSceneIndex)
        {
            gameStateInit();
        }
        //This may change^^
    }

    private void gameStateInit()
    {
        //May need optimizing
        dataInterpreter local = gameObject.AddComponent(typeof(dataInterpreter)) as dataInterpreter;
        inputHandler input = gameObject.AddComponent(typeof(inputHandler)) as inputHandler;

        local.activeLevel = c.currentlySelectedLevel;
        //Will Vary based on the GameMode that was Selected + Difficulty
        local.unitList = c.currentlySelectedLevel.getTimeMode.units;
        local.unitLayer = c.unitLayer;
        local.enabled = true;
        local.initializeOnStart();
    }

    private void gameStateDestroy()
    {
        c.GetComponent<dataInterpreter>().Grid.gridControl.clearGrid();
        Destroy(c.GetComponent<dataInterpreter>());
        Destroy(c.GetComponent<inputHandler>());
    }

    public void restart(int index)
    {
        gameStateDestroy();
        loadScene(index);
    }
}
