using GridHandler;
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
        rb = GetComponent<Rigidbody>();
        mRend = GetComponent<MeshRenderer>();
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
    }

    public void FixedUpdate()
    {
        Vector3 target = new Vector3();
        if (transform.position != worldPosition)
        {
            if (gameController.pauseState)
            {
                currentState = unitState.idle;
            }
            else
            {
                //Begin moving the block at the speed
                transform.position = Vector3.SmoothDamp(transform.position, worldPosition, ref vel, speedScaler);
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
    }
}

public class stateMachine
{
    private state currentState, previousState;

    public void changeState(state newState, GameObject stateIdentity = null)
    {
        if (currentState != null)
        {
            this.currentState.onExit();
        }
        this.previousState = this.currentState;
        this.currentState = newState;
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
}
