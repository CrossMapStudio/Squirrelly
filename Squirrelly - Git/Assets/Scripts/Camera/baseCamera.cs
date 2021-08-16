using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class baseCamera : MonoBehaviour
{
    public CinemachineVirtualCamera mcVirtual;
    private Camera main;

    #region Toggles
    private bool toggleZoom;
    #endregion

    #region Saved Values
    private float savedZoomValue;
    #endregion

    #region Update Target Values
    private float targetZoomValue;
    private static float shakeCounter;

    private static float targetAmplitude = 0;
    private static float targetFrequency = 0;
    private static float duration;
    private static float activeZoomModifier;
    private CinemachineBasicMultiChannelPerlin noiseChannel;
    #endregion

    #region Objects
    #endregion

    #region Setter Values
    public static AudioSource onePlayAudio;
    public static audioController audioControl;
    public List<AudioClip> onePlayAudioClips;

    public enum onePlaySounds
    {
        swish1,
        swish2,
        swish3,
        completionSound
    }

    public bool trackMouse;
    public float xClamp, yClamp;

    public float zoomMultiplier;
    public static List<presetCameraShakeValues> shakePreset;
    public enum shakePresets
    {
        onUnitDeath,
        onWaveClear,
    }
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        main = Camera.main;
        onePlayAudio = GetComponent<AudioSource>();
        audioControl = new audioController(onePlayAudio, onePlayAudioClips);

        targetZoomValue = savedZoomValue = mcVirtual.m_Lens.FieldOfView;
        noiseChannel = mcVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shakePreset = new List<presetCameraShakeValues>();
        presetCameraShakeValues onUnitDeath = new presetCameraShakeValues(1f, .2f, 1f, 1f);
        presetCameraShakeValues onWaveClear = new presetCameraShakeValues(1.2f, .2f, 1.2f, 1f);
        shakePreset.Add(onUnitDeath);
        shakePreset.Add(onWaveClear);
    }

    private void Update()
    {
        mcVirtual.m_Lens.FieldOfView = Mathf.Lerp(mcVirtual.m_Lens.FieldOfView, targetZoomValue, Time.deltaTime * zoomMultiplier);
        noiseChannel.m_AmplitudeGain = targetAmplitude;
        noiseChannel.m_FrequencyGain = targetFrequency;

        if (shakeCounter >= duration) {
            targetAmplitude = 0f;
            targetFrequency = 0f;
            targetZoomValue = savedZoomValue;
        }
        else
        {
            shakeCounter += Time.deltaTime;
        }
    }

    static public void triggerScreenShake(shakePresets passed)
    {
        targetAmplitude = shakePreset[(int)passed].magnitude;
        targetFrequency = shakePreset[(int)passed].frequencyAdjust;
        duration = shakePreset[(int)passed].duration;
        activeZoomModifier = shakePreset[(int)passed].zoomModifier;
        shakeCounter = 0f;
    }
}

public struct presetCameraShakeValues
{
    public float magnitude;
    public float duration;
    public float frequencyAdjust;
    public float zoomModifier;
    public presetCameraShakeValues(float _magnitudeOfShake, float _durationOfShake, float _frequencyOfShake, float _zoomModificationOnShake)
    {
        magnitude = _magnitudeOfShake;
        duration = _durationOfShake;
        frequencyAdjust = _frequencyOfShake;
        zoomModifier = _zoomModificationOnShake;
    }
}
