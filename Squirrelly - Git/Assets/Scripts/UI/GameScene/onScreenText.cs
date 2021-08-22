using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onScreenText : MonoBehaviour
{
    private Text textElement;
    [SerializeField] private AudioClip clip;
    public void Awake()
    {
        textElement = GetComponent<Text>();
    }

    public void changeTextOnAnimTrigger(string textValueToChangeTo)
    {
        textElement.text = textValueToChangeTo;
    }

    public void playAudioClipOnAnimTrigger(int index)
    {
        baseCamera.audioControl.playSoundOnIndex(index);
    }

    public void startGame()
    {
        gameController.gameIntroState = false;
        transform.parent.gameObject.SetActive(false);
    }
}
