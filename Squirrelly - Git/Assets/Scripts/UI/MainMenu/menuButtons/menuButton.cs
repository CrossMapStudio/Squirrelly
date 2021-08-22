using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Controls;

public class menuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    GameObject controllerParent;
    gameController controller;
    bool mouseHovering;
    Animator buttonAnim;
    [SerializeField] private string animationHoverName = "hover";
    [SerializeField] private GameObject loadingScreen;
    public string startAnimationName = "loadIn", targetAnimationName = "inFrame";


    public enum buttonType
    {
        sceneChange,
        levelSelect,
        scroller,
        mute,
        nullAction,
        exitGame
    }
    public buttonType buttonTypeVariation;

    public enum sceneToChangeTo
    {
        restart = -1,
        levelSelect = 1,
        gameScene = 2,
        mainMenu = 0,
        campaignSelect = 3
    }
    public sceneToChangeTo sceneIndex;
    private buttonAction assignedAction;
    private dynamicLevelSelect activeButton;
    //This is for the Scroller
    private scrollerButtonSelect activeScroller;
    //North, NE, East, SE, South, SW, West, NW ->
    public bool dynamicNeighboring;
    [Header("Manual Linking")]
    public menuButton northButton, northEastButton, eastButton, southEastButton, southButton, southWestButton, westButton, northWestButton;
    private void Awake()
    {
        buttonAnim = GetComponent<Animator>();
        if (!loadingScreen || dynamicNeighboring)
        loadingScreen = GameObject.FindGameObjectWithTag("loadingScreen");
    }
    private void Start()
    {
        controllerParent = GameObject.FindGameObjectWithTag("GameController");
        controller = controllerParent.GetComponent<gameController>();

        switch (buttonTypeVariation)
        {
            case buttonType.sceneChange:
                assignedAction = new sceneChangeButton((int)sceneIndex, controller.GetComponent<sceneManager>(), loadingScreen.GetComponent<Animator>(), startAnimationName, targetAnimationName);
                break;
            case buttonType.levelSelect:
                levelButton currentLevelButton = GetComponent<levelButton>();
                var clone = new levelSelectButton(controller, currentLevelButton, (int)sceneIndex, controller.GetComponent<sceneManager>(), loadingScreen.GetComponent<Animator>(), startAnimationName, targetAnimationName);
                assignedAction = clone;
                activeButton = clone;
                break;
            case buttonType.scroller:
                scrollContainer container = GetComponent<scrollContainer>();
                activeScroller = new scrollerButton(container);
                break;
            case buttonType.exitGame:
                assignedAction = new exitGameButton(controller);
                break;
            case buttonType.nullAction:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData element)
    {
        if (!sceneManager.inLoadState)
        {
            mouseHovering = true;
            setHoveringValue = true;
            Debug.Log("Button Name: " + name);
            if (buttonAnim != null)
                buttonAnim.SetBool("hover", true);
        }
    }

    public void OnPointerExit(PointerEventData element)
    {
        if (!sceneManager.inLoadState)
        {
            mouseHovering = false;
            setHoveringValue = false;
            Debug.Log(mouseHovering);
            if (buttonAnim != null)
                buttonAnim.SetBool("hover", false);
        }
    }

    public void OnPointerClick(PointerEventData element)
    {
        if (!sceneManager.inLoadState)
            callButtonAction();
    }

    public menuButton getNeighbor(int index)
    {
        switch (index)
        {
            case 0:
                return northButton;
            case 1:
                return northEastButton;
            case 2:
                return eastButton;
            case 3:
                return southEastButton;
            case 4:
                return southButton;
            case 5:
                return southWestButton;
            case 6:
                return westButton;
            case 7:
                return northWestButton;
            default:
                throw errorHandler.valueOutsideOfSwitchRange;
        }
    }

    public void callButtonAction()
    {
        if (assignedAction != null && !sceneManager.inLoadState)
            assignedAction.triggerAction();
    }

    public bool setHoveringValue { set { mouseHovering = value; if (buttonAnim != null) buttonAnim.SetBool("hover", mouseHovering); if (activeButton != null) activeButton.setActiveLevel(mouseHovering); } }

    public scrollerButton getScrollButton { get { return activeScroller != null ? activeScroller.parent : null; } }
}

public interface buttonAction
{
    public void triggerAction();
}

public interface dynamicLevelSelect
{
    public bool isDynamic { get; set; }
    public void setActiveLevel(bool hovering);
}

public interface scrollerButtonSelect
{
    public int currentSelection { get; set; }
    public scrollerButton parent { get; }
    public void scrollOver(int direction, int clampSize);
}

public class sceneChangeButton : buttonAction
{
    int sceneIndex;
    sceneManager sceneManage;
    Animator loadingAnimator;
    string startAnim, targetAnim;
    public sceneChangeButton(int _sceneIndex, sceneManager _sceneManage, Animator _loadingAnimator, string _startAnimName, string _targetAnimName)
    {
        sceneIndex = _sceneIndex;
        sceneManage = _sceneManage;
        loadingAnimator = _loadingAnimator;
        startAnim = _startAnimName;
        targetAnim = _targetAnimName;
    }
    public void triggerAction()
    {
        if (sceneIndex == -1)
        {
            gameController.pauseState = false;
            sceneManage.restart(2);
        }
        else
        {
            sceneManage.loadScene(sceneIndex, loadingAnimator, startAnim, targetAnim);
        }
    }
}

public class levelSelectButton : buttonAction, dynamicLevelSelect
{
    gameController controller;
    
    int sceneIndex;
    sceneManager sceneManage;
    Animator loadingAnimator;
    string startAnim, targetAnim;

    levelButton buttonData;

    public levelSelectButton(gameController _controller, levelButton _buttonData, int _sceneIndex, sceneManager _sceneManage, Animator _loadingAnimator, string _startAnimName, string _targetAnimName)
    {
        controller = _controller;
        sceneIndex = _sceneIndex;
        sceneManage = _sceneManage;
        loadingAnimator = _loadingAnimator;
        startAnim = _startAnimName;
        targetAnim = _targetAnimName;
        buttonData = _buttonData;
    }
    public void triggerAction()
    {
        controller.launchActiveLevel(loadingAnimator, startAnim, targetAnim);
    }

    public void setActiveLevel(bool hovering)
    {
        if (!buttonData.disabled)
            buttonData.setStatus(hovering);
    }

    public bool isDynamic { get; set; } = true;
}

public class scrollerButton : scrollerButtonSelect
{
    scrollContainer scroller;
    public scrollerButton(scrollContainer _scroller)
    {
        scroller = _scroller;
    }
    public int currentSelection { get; set; } = 0;
    public scrollerButton parent { get { return this; } }
    public void scrollOver(int direction, int clampSize)
    {
        if (currentSelection + -direction >= 0 && currentSelection + -direction < clampSize)
        {
            Vector3 origin = scroller.originPoint;
            origin.x += (scroller.xIncremental * direction);
            scroller.originPoint = origin;
            currentSelection += -direction;
        }
    }
}

public class exitGameButton : buttonAction
{
    gameController controller;
    public exitGameButton(gameController _controller)
    {
        controller = _controller;
    }

    public void triggerAction()
    {
        controller.serializationMenu(0);
        controller.sceneControl.quitGame();
    }
}