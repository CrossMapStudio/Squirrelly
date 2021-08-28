using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicController : MonoBehaviour
{
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip[] musicList;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer.clip = musicList[0];
        musicPlayer.Play();
    }

    private void Update()
    {
        settingsListener();
    }

    private void settingsListener()
    {
        if (settingStorage.valuesUpdated)
        {
            float volume = musicPlayer.volume;
            settingStorage.setValues(ref volume, settingStorage.volumeType.music);
            musicPlayer.volume = volume;
        }
    }

}
