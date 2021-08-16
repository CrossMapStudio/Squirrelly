using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class levelCompleteMenu : MonoBehaviour
{
    [SerializeField] private Image[] acorns; // Three Achievable Acorns
    [SerializeField] private Color acornEarnedColor; // Two for Normal and Complete ---

    [SerializeField] private Text gameStatus;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text wavesCompleted;
    [SerializeField] private Text unitsLost;
    [SerializeField] private Text timeOfCompletion;

    public void setAllUIElements(interpreterData data)
    {
        for (int i = 0; i < data.currentRewardStatus; i++)
            acorns[i].color = acornEarnedColor;

        gameStatus.text = data.gameStat;
        scoreText.text = "Score: " + data.currentScore.ToString();
        wavesCompleted.text = "Waves Completed: " + data.wavesCompleted.ToString() + "/" + data.totalWaves.ToString();
        unitsLost.text = "Units Lost: " + data.unitsLost.ToString();
        timeOfCompletion.text = "Completed Time: " + data.bestTimeOfCompletion.ToString();
    }
}
