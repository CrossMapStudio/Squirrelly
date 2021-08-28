using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingPanel : MonoBehaviour
{
    //Will Handle Controller Support Later

    public Slider masterVolume;
    public Slider vehicleVolume;
    public Slider effectsVolume;
    public Slider musicVolume;

    public List<Slider> sliderSettings;

    //For Navigating the Settings
    int index = 0;

    private void Awake()
    {
        sliderSettings = new List<Slider>() { masterVolume, vehicleVolume, effectsVolume, musicVolume };
        masterVolume.value = gameSettingsController.masterVolume;
        vehicleVolume.value = gameSettingsController.vehicleVolume;
        effectsVolume.value = gameSettingsController.effectsVolume;
        musicVolume.value = gameSettingsController.musicVolume;
        gameController.settingPanelState = true;
    }

    private void Update()
    {
        gameSettingsController.masterVolume = masterVolume.value;
        gameSettingsController.vehicleVolume = vehicleVolume.value;
        gameSettingsController.effectsVolume = effectsVolume.value;
        gameSettingsController.musicVolume = musicVolume.value;

        if (inputHandler.inputListener == 9)
        {
            if (index != 0)
                index--;

            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 10)
        {
            if (index != sliderSettings.Count - 1)
                index++;

            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 12)
        {
            sliderSettings[index].value += .1f;
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 13)
        {
            sliderSettings[index].value -= .1f;
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 2)
        {
            closeSettings();
            inputHandler.setInputActiveListenerValue(0);
        }
    }

    public void onSave()
    {
        settingStorage.updateValues();
        serializationHandler.saveGame(serializationHandler.fileTag, GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>().getGameDataForSave);
    }

    public void closeSettings()
    {
        onSave();
        gameObject.transform.parent.gameObject.SetActive(false);
        gameController.settingPanelState = false;
    }
}
