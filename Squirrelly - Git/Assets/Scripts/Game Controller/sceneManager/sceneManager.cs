using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    private int sceneToLoad;
    private bool beginLoad, waitForAnim;

    public static int currentLevelIndex = 0;
    public static bool inLoadState = false;
    private int levelSelectIndex = 1, playSceneIndex = 2, mainMenuIndex = 0;

    private gameController c;

    private Animator passedAnim;
    string loadStartAnim;

    private inputHandler inputHandle;
    private dataInterpreter data;

    public void Start()
    {
        c = GetComponent<gameController>();
        inputHandle = c.GetComponent<inputHandler>();
        data = gameObject.GetComponent<dataInterpreter>();
    }

    public void loadScene(int scene, Animator loadSceneAnim = null, string animToTrigger = null, string animationNameForLoadStart = null)
    {
        if (data.Grid.gridControl != null)
            data.Grid.gridControl.clearGrid();

        if (!loadSceneAnim)
        {
            sceneToLoad = scene;
            beginLoad = true;
        }
        else
        {
            sceneToLoad = scene;
            passedAnim = loadSceneAnim;
            loadStartAnim = animationNameForLoadStart;
            passedAnim.Play(animToTrigger);
            beginLoad = false;
            waitForAnim = true;
        }
    }

    private void Update()
    {
        if (beginLoad)
        {
            StartCoroutine(loadAsync());
            beginLoad = false;
        }
        else if (waitForAnim)
        {
            inLoadState = true;
            if (passedAnim.GetCurrentAnimatorStateInfo(0).IsName(loadStartAnim))
            {
                beginLoad = true;
                waitForAnim = false;
            }
        }
    }

    IEnumerator loadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        currentLevelIndex = sceneToLoad;
        data.clear();
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (currentLevelIndex == playSceneIndex)
        {
            gameStateInit();
        }

        inLoadState = false;
        //This may change^^
    }

    private void gameStateInit()
    {
        Debug.Log("Currently Selected Level: " + c.currentlySelectedLevel);
        data.activeLevel = c.currentlySelectedLevel;
        //Will Vary based on the GameMode that was Selected + Difficulty
        data.unitList = c.currentlySelectedLevel.getTimeMode.units;
        data.unitLayer = c.unitLayer;
        data.enabled = true;
        gameController.gameEndState = false;
        data.initializeOnStart();
    }

    public void restart(int index)
    {
        loadScene(index);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
