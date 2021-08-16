using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.InputSystem;

public class xpCoinTracking : MonoBehaviour
{
    [HideInInspector]
    public Transform targetPosition;
    public float movementSpeed;
    private float currentSpeed;
    private int keyModifier;
    [SerializeField] private Animator coinAnimator;
    [SerializeField] private string animationNameToWaitFor;

    private gameCanvas canvas;
    private dataInterpreter data;

    public Image coinSprite;
    public Color[] coinColors;
    public enum coinTypes
    {
        singlePoint,
        fivePoint,
        tenPoint,
        fiftyPoint,
        hundredPoint
    }

    private int[] coinValues = { 1, 5, 10, 50, 100 };

    [HideInInspector]
    public int coinValue;
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("gameCanvas").GetComponent<gameCanvas>();
        data = GameObject.FindGameObjectWithTag("GameController").GetComponent<dataInterpreter>();
        coinValue = 1;
    }

    public void setValues(coinTypes type, int scoreIndex)
    {
        coinSprite.color = coinColors[(int)type];
        coinValue = coinValues[(int)type];

        keyModifier = scoreIndex;
    }

    void Update()
    {
        if (coinAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationNameToWaitFor))
        {
            var dist = Vector2.Distance(transform.position, targetPosition.position);
            if (dist >= 5f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, movementSpeed, Time.deltaTime * 5f);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, currentSpeed * Time.deltaTime);
            }
            else
            {
                canvas.playUIStatAnimation(keyModifier.ToString() == "0" ? "" : keyModifier.ToString(), 1, "+" + coinValue);
                data.gameModeInt.runData.currentScore += coinValue;
                Destroy(gameObject);
            }
        }
    }
}
