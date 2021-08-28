using System;
using System.Collections;
using UnityEngine;

public static class gameSettingsController
{
    public static float masterVolume = 1, vehicleVolume = 1, effectsVolume = 1, musicVolume = 1;
}

[Serializable]
public class settingStorage
{
    public static settingStorage storage;
    public float masterVolume, vehicleVolume, effectsVolume, musicVolume;
    public static bool valuesUpdated;
    public enum volumeType
    {
        vehicle,
        effect,
        music
    }

    public void setStorage()
    {
        storage = this;
        updateValues();
    }

    public static void updateValues()
    {
        storage.masterVolume = gameSettingsController.masterVolume;
        storage.vehicleVolume = gameSettingsController.vehicleVolume;
        storage.effectsVolume = gameSettingsController.effectsVolume;
        storage.musicVolume = gameSettingsController.musicVolume;
        valuesUpdated = true;
    }

    public static void setValues(ref float value, volumeType type)
    {
        switch (type)
        {
            case volumeType.effect:
                if (storage.masterVolume < storage.effectsVolume)
                    value = storage.masterVolume;
                else
                    value = storage.effectsVolume;
                break;
            case volumeType.vehicle:
                if (storage.masterVolume < storage.vehicleVolume)
                    value = storage.masterVolume;
                else
                    value = storage.vehicleVolume;
                break;
            case volumeType.music:
                if (storage.masterVolume < storage.musicVolume)
                    value = storage.masterVolume;
                else
                    value = storage.musicVolume;
                break;
            default:
                value = storage.masterVolume;
                break;
        }
    }
}
