using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuCanvas : MonoBehaviour
{
    public GameObject levelButton, buttonOrigin;
    private gameController gameControl;
    //For UI Bases
    List<levelButton> levels;
    levelButton currentActive;

    public Dropdown gameMode, difficulty;

    private void Awake()
    {
        levels = new List<levelButton>();
    }

    private void Start()
    {
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        populateLevelSelect();
    }

    //This can change based on desired UI Layout
    void populateLevelSelect()
    {
        foreach (storedLevelData element in gameControl.updatedLevels)
        {
            var clone = Instantiate(levelButton, buttonOrigin.transform);
            levelButton button = clone.GetComponent<levelButton>();
            clone.GetComponent<Button>().onClick.AddListener(() => { checkButton(button); });
            updateStats(ref button, element);

            levels.Add(button);
        }


        currentActive = levels[0];
        levels[0].setStatus(true);
    }

    public void updateButtons()
    {
        for(int i = 0; i < levels.Count; i++)
        {
            levelButton el = levels[i];
            updateStats(ref el, gameControl.updatedLevels[i]);
        }
    }

    void updateStats(ref levelButton button, storedLevelData element)
    {
       container current = element.getContainer(element.gameModeTags[gameMode.value]);
        if (difficulty.value >= current.dataSize)
        {
            if (gameControl.compareCurrentLevel(element.id))
            {
                gameControl.currentlySelectedLevel = null;
            }

            button.gameObject.SetActive(false);
        }
        else
        {
            button.gameObject.SetActive(true);
            gameInfo local = current.getGameInfo(difficulty.value);
            button.setPar(element, local.starsEarned, element.displayName);
            //For Pulling Save Later in Game --- Can Optionally Clear these before Return
            element.difficultyIndex = difficulty.value;
            element.containerIndex = gameMode.value;
        }
    }

    public void checkButton(levelButton btn)
    {
        if (currentActive != null)
        currentActive.setStatus(false);
        currentActive = btn;
        currentActive.setStatus(true);
    }

    public void launchGame()
    {
        gameControl.launchActiveLevel();
    }
}
