using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameCanvas : MonoBehaviour
{
    gameController gameControl;
    public Text timer;
    public GameObject pauseMenuUI;
    private void Awake()
    {
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
    }

    //Debugging/Testing
    public void starAddition(int stars)
    {
        gameControl.activeStorage.addStar(stars);
    }

    public void waveReset()
    {
        gameControl.gameObject.GetComponent<dataInterpreter>().gameStateControl.waveTrigger();
    }

    public void quitScene() 
    {
        Debug.Log("Saving");
        gameControl.modifyLevelData();
        gameControl.serializationMenu(0);
        Destroy(gameControl.gameObject.GetComponent<dataInterpreter>());
        Destroy(gameControl.gameObject.GetComponent<inputHandler>());
        gameControl.sceneControl.loadScene(0);
    }

    public void triggerPauseMenuUI()
    {
        pauseMenuUI.SetActive(gameController.pauseState);
    }

    public void endGame()
    {
        pauseMenuUI.SetActive(true);
        //Update States by Passing from Game Controller
    }

    public void pauseMenu(int index)
    {
        switch (index)
        {
            case 0:
                gameController.pauseState = false;
                triggerPauseMenuUI();
                break;
            case 1:
                gameController.pauseState = false;
                gameControl.sceneControl.restart(1);
                break;
            case 2:
                gameController.pauseState = false;
                gameControl.sceneControl.loadScene(0);
                break;
            case 3:
                break;
        }
    }
}
