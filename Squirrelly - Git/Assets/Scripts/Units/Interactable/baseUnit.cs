using GridHandler;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class baseUnit : MonoBehaviour
{
    private Rigidbody rb;
    [HideInInspector]
    public Vector3 worldPosition, pos1, pos2, vel;
    [HideInInspector]
    public bool winState;
    [HideInInspector]
    public baseUnit[] neighbors = new baseUnit[2];
    [HideInInspector]
    public bool selected = false, potential = false;
    [HideInInspector]
    public baseUnit entity;
    [HideInInspector]
    public float winPos;

    public float speedScaler;
    private MeshRenderer mRend;
    private Material baseMat;
    public Material selectedMat, potentialMat;
    public ParticleSystem onDelete;
    public Text inputText;

    //New Stuff
    public Animator unitAnimator;
    private stateMachine unitStateMachine;
    private animationController animControl;
    private audioController audioControl;

    private enum inputMapController
    {
        WB,
        SB,
        NB,
        EB,
        L1,
        L2,
        R1,
        R2
    }

    private enum inputMapKeyMouse
    { 
        Q,
        W,
        E,
        R,
        T,
        Y,
        U,
        I
    }

    //This will change most likely
    private enum unitState
    {
        idle,
        moving
    }

    private unitState currentState = unitState.idle;

    private void Awake()
    {
        //Enter the Awake State ---
        audioControl = new audioController();
        animControl = new animationController(unitAnimator);

        unitStateMachine = new stateMachine(animControl, audioControl);
        unitStateMachine.changeState(new awakeState());

        rb = GetComponent<Rigidbody>();
        mRend = transform.GetChild(1).GetComponent<MeshRenderer>();
        baseMat = mRend.material;
    }

    public void triggerNeighbors(bool triggerStatus = false)
    {
        if (neighbors[0] != null)
            neighbors[0].potential = triggerStatus;

        if (neighbors[1] != null)
            neighbors[1].potential = triggerStatus;
    }

    public void destroyUnit()
    {
        //This will Handle Rerouting Neighbors/Killing the Unit
        if (neighbors[0] != null)
            neighbors[0].neighbors[1] = neighbors[1];

        if (neighbors[1] != null)
            neighbors[1].neighbors[0] = neighbors[0];

        var clone = Instantiate(onDelete, transform.position + new Vector3(0f, .5f, 0f), Quaternion.identity);
        Destroy(clone, 2f);
        Destroy(gameObject);
    }

    public void setInput(int controllerType, int input)
    {
        switch (controllerType)
        {
            case 0:
                inputMapController ps = (inputMapController)input;
                inputText.text = ps.ToString();
                break;
            case 1:
                inputMapKeyMouse keyMouse = (inputMapKeyMouse)input;
                inputText.text = keyMouse.ToString();
                break;
        }
    }

    public void Update()
    {
        Material temp = selected ? selectedMat : baseMat;
        temp = potential ? potentialMat : temp;

        if (temp != mRend.material)
            mRend.material = temp;

        if (unitStateMachine != null)
            unitStateMachine.executeStateUpdate();
    }

    public void FixedUpdate()
    {
        if (transform.position != worldPosition)
        {
            if (gameController.pauseState)
            {
                currentState = unitState.idle;
            }
            else
            {
                //Begin moving the block at the speed
                transform.position = Vector3.MoveTowards(transform.position, worldPosition, speedScaler * Time.fixedDeltaTime);
                if (currentState != unitState.moving)
                    currentState = unitState.moving;
            }
        }

        if (currentState == unitState.moving)
        {
            if (Vector3.Distance(worldPosition, transform.position) <= .05f)
            {
                transform.position = worldPosition;
                currentState = unitState.idle;
            }

            if (worldPosition.x != winPos)
            {
                if (winState == true)
                {
                    winState = false;
                    gameStateController.compareNum--;
                }
            }
        }
        else
        {
            if (worldPosition.x == winPos)
            {
                if (winState == false)
                {
                    winState = true;
                    gameStateController.compareNum++;
                }
            }
        }

        if (unitStateMachine != null)
            unitStateMachine.executeStateFixedUpdate();
    }

    //Mainly for Anim Triggers and Call Within the Base - 
    public void changeState(int switchValue)
    {
        switch (switchValue)
        {
            case 0:
                unitStateMachine.changeState(new idleState());
                break;
        }
    }
}

public class stateMachine
{
    private state currentState, previousState;
    private animationController animControl;
    private audioController audioControl;

    public stateMachine(animationController _animControl, audioController _audioControl)
    {
        animControl = _animControl;
        audioControl = _audioControl;
    }

    public void changeState(state newState, GameObject stateIdentity = null)
    {
        if (currentState != null)
        {
            this.currentState.onExit();
        }
        this.previousState = this.currentState;
        this.currentState = newState;
        this.currentState.animControl = animControl;
        this.currentState.audioControl = audioControl;
        this.currentState.onEnter();
    }

    public void executeStateUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onUpdate();
        }
    }

    public void executeStateFixedUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            this.currentState.onFixedUpdate();
        }
    }

    //Prolly not needed --- Can Optimize if No Case
    public void previousStateSwitch()
    {
        if (this.previousState != null)
        {
            this.currentState.onExit();
            this.currentState = this.previousState;
            this.currentState.onEnter();
        }
        else
        {
            return;
        }
    }

    //To Allow Us to Check for the State
    public string getCurrentState()
    {
        return this.currentState.ToString();
    }
}

public interface state 
{
    public void onEnter();

    public void onUpdate();

    public void onFixedUpdate();

    public void onExit();

    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
}

#region States
public class awakeState : state
{
    public void onEnter()
    {
        Debug.Log("Awake State");
    }

    public void onUpdate()
    {

    }

    public void onFixedUpdate()
    {

    }

    public void onExit()
    {

    }

    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
}
public class idleState : state
{
    public void onEnter()
    {
        Debug.Log("Idle State");
    }

    public void onUpdate()
    {

    }

    public void onFixedUpdate()
    {

    }

    public void onExit()
    {

    }

    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
}
public class movingState
{
    public class idleState : state
    {

        public void onEnter()
        {

        }

        public void onUpdate()
        {

        }

        public void onFixedUpdate()
        {

        }

        public void onExit()
        {

        }

        public animationController animControl { get; set; }
        public audioController audioControl { get; set; }
    }
}
public class deathState : state
{

    public void onEnter()
    {

    }

    public void onUpdate()
    {

    }

    public void onFixedUpdate()
    {

    }

    public void onExit()
    {

    }

    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
}
public class positionEnterStatepublic : state
{

    public void onEnter()
    {

    }

    public void onUpdate()
    {

    }

    public void onFixedUpdate()
    {

    }

    public void onExit()
    {

    }

    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
}
public class levelCompletionState : state
{

    public void onEnter()
    {

    }

    public void onUpdate()
    {

    }

    public void onFixedUpdate()
    {

    }

    public void onExit()
    {

    }

    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
}
#endregion

#region Unit Management

public class animationController
{
    Animator unitAnimator;
    public animationController(Animator _unitAnimator)
    {
        unitAnimator = _unitAnimator;
    }

    public void setTrigger(string triggerName)
    {
        //Error Handling Later
        unitAnimator.SetTrigger(triggerName);
    }

    public void setBool(string boolName, bool value)
    {
        //Error Handling Good Here
        if (!unitAnimator.GetBool(boolName)) {
            errorHandler.printMessage("The Bool Value Passes is Non-Existent in the Animator"); 
            return;
        }

        unitAnimator.SetBool(boolName, value);
    }

    public void setFloat(string valueName, float value)
    {
        //Error Handling Later
        unitAnimator.SetFloat(valueName, value);
    }
}

public class audioController
{
    public audioController()
    {

    }
}

//Used for CallBacks within the States --- Can Use Refactor
public class changeState
{
    private int stateToChange, passed;
    public changeState(int stateToChangeTo, int passedParameter = 0)
    {
        stateToChange = stateToChangeTo;
        passed = passedParameter;
    }

    public int retrieveStateChange()
    {
        return stateToChange;
    }

    public int retrievePassedParameter()
    {
        return passed;
    }
}

#endregion
