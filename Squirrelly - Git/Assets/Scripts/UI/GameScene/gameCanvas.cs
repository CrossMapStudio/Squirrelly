using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameCanvas : MonoBehaviour
{
    gameController gameControl;
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
}
