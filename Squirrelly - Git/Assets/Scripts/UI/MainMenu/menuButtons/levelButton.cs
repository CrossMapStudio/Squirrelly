using UnityEngine;
using UnityEngine.UI;

public class levelButton : MonoBehaviour
{
    public storedLevelData levelData { get; set; }
    [SerializeField] private Image[] levelStars;
    [SerializeField] private Text levelName, highScore;

    [SerializeField] private Color starColor, defaultColor;
    gameController gData;

    public void Awake()
    {
        gData = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
    }
    public void setPar(storedLevelData associated, int starsEarned, string _levelName, int _highScore)
    {
        levelData = associated;
        for (int i = 0; i < levelStars.Length; i++)
            levelStars[i].color = starsEarned > i ? starColor : defaultColor;

        levelName.text = _levelName;
        highScore.text = "High Score: " + _highScore.ToString();
    }

    public void setStatus(bool status)
    {
        if (status)
        {
            gData.getActiveLevel(levelData.id, levelData);
        }
    }
}
