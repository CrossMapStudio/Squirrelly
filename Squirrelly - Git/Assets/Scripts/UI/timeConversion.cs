using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeConversion : MonoBehaviour
{
    public static string convertToTime(float currentTime)
    {
        float minutes = currentTime <= 0 ? 0 : Mathf.FloorToInt(currentTime / 60);
        float seconds = currentTime <= 0 ? 0 : Mathf.FloorToInt(currentTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
