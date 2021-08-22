using UnityEngine;
using UnityEngine.UI;

public class levelButton : MonoBehaviour
{
    public storedLevelData levelData { get; set; }
    [SerializeField] private Image[] levelStars;
    [SerializeField] private Text levelName, highScore;

    [SerializeField] private Color starColor, defaultColor;
    gameController gData;

    [HideInInspector]
    public animationController animController;
    private Animator anim;

    [HideInInspector]
    public bool disabled;

    public void Awake()
    {
        gData = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        anim = GetComponent<Animator>();
        animController = new animationController(anim);
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
            gData.setActiveLevel(levelData.id, levelData);
        }
    }

    public void disableButton()
    {
        animController.setBool("disabled", true);
        disabled = true;
    }

    public void enableButton()
    {
        animController.setBool("disabled", false);
        disabled = false;
    }
}
