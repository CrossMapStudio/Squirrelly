using UnityEngine;
using UnityEngine.UI;

public class levelButton : MonoBehaviour
{
    public storedLevelData levelData { get; set; }
    public string accessTag;
    [SerializeField] private Image levelSelected;
    [SerializeField] private Image[] levelStars;
    [SerializeField] private Text levelName, levelDetails;

    [SerializeField] private Color activeColor, starColor, defaultColor;
    gameController gData;

    public void Awake()
    {
        gData = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
    }
    public void setPar(storedLevelData associated, int starsEarned, string _levelName)
    {
        levelData = associated;
        for (int i = 0; i < levelStars.Length; i++)
            levelStars[i].color = starsEarned > i ? starColor : defaultColor;

        levelName.text = _levelName;
    }

    public void setStatus(bool status)
    {
        levelSelected.color = status ? activeColor : defaultColor;
        if (status)
        {
            gData.getActiveLevel(levelData.id, levelData);
        }
    }
}
