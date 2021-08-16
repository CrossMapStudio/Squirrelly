using GridHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class baseUnit : MonoBehaviour
{
    private Rigidbody rb;
    [HideInInspector]
    public Vector3 worldPosition, pos1, pos2, vel;
    [HideInInspector]
    public bool winState, unitDead;
    [HideInInspector]
    public baseUnit[] neighbors = new baseUnit[2];
    [HideInInspector]
    public bool selected = false, potential = false, lockedSelection = false;
    [HideInInspector]
    public baseUnit entity;
    [HideInInspector]
    public float winPos;
    [HideInInspector]
    public dataInterpreter gameData;

    [HideInInspector]
    public List<Node> activeNodes;

    public float speedScaler;
    private MeshRenderer mRend;
    private Material baseMat;
    public Material selectedMat, potentialMat;
    public ParticleSystem onDelete, onDeleteSuccess;
    public Text inputText;

    //New Stuff
    public Animator unitAnimator;
    private stateMachine unitStateMachine;
    private animationController animControl;
    private audioController audioControl;
    private particleController particleControl;
    public ParticleSystem[] particleSystems;

    public float RotationSpeed = 2f;
    private Vector3 _direction;
    private Quaternion _lookRotation;

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
        audioControl = new audioController(null, null);
        animControl = new animationController(unitAnimator);
        particleControl = new particleController(particleSystems);

        unitStateMachine = new stateMachine(this, animControl, audioControl, particleControl);
        unitStateMachine.changeState(new idleState());

        //For Tracking Unit on Death for Win State
        gameData = GameObject.FindGameObjectWithTag("GameController").GetComponent<dataInterpreter>();

        rb = GetComponent<Rigidbody>();
        mRend = transform.GetChild(1).GetComponent<MeshRenderer>();
        baseMat = mRend.material;

        activeNodes = new List<Node>();
    }

    public void triggerNeighbors(bool triggerStatus = false)
    {
        if (neighbors[0] != null)
            neighbors[0].potential = triggerStatus;

        if (neighbors[1] != null)
            neighbors[1].potential = triggerStatus;
    }

    public void destroyUnit(int deathCause = 0)
    {
        if (deathCause == 0)
        {
            var clone = Instantiate(onDelete, transform.position + new Vector3(0f, .5f, 0f), Quaternion.identity);
            Destroy(clone, 2f);
        }
        else
        {
            var clone = Instantiate(onDeleteSuccess, transform.position + new Vector3(0f, .5f, 0f), Quaternion.identity);
            Destroy(clone, 2f);
        }
        activeNodes[0].isWinPos = false;
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

        if (worldPosition == pos1 && transform.position == worldPosition)
        {
            if (activeNodes[0] != null)
                activeNodes[0].inWinPos = true;
        }
        else
        {
            if (activeNodes[0] != null)
                activeNodes[0].inWinPos = false;
        }

        //For Dynamic Line Cast
        if (selected)
        {
            if (worldPosition == pos1)
            {
                if (!activeNodes[1].targetSymbol.activeSelf)
                {
                    activeNodes[0].targetSymbol.SetActive(false);
                    activeNodes[1].targetSymbol.SetActive(true);
                    activeNodes[0].directionalParticleSystem.gameObject.SetActive(true);
                }
            }
            else
            {
                if (!activeNodes[0].targetSymbol.activeSelf)
                {
                    activeNodes[1].targetSymbol.SetActive(false);
                    activeNodes[0].targetSymbol.SetActive(true);
                    activeNodes[1].directionalParticleSystem.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            activeNodes[0].targetSymbol.SetActive(false);
            activeNodes[1].targetSymbol.SetActive(false);
            activeNodes[0].directionalParticleSystem.gameObject.SetActive(false);
            activeNodes[1].directionalParticleSystem.gameObject.SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        if (transform.position != worldPosition && unitStateMachine.getCurrentState() != "deathState")
        {
            if (gameController.pauseState || gameController.gameEndState)
            {
                currentState = unitState.idle;
            }
            else
            {
                //Begin moving the block at the speed
                transform.position = Vector3.MoveTowards(transform.position, worldPosition, speedScaler * Time.fixedDeltaTime);
                if (currentState != unitState.moving)
                {
                    currentState = unitState.moving;
                    changeState(1);
                }
            }
        }

        if (currentState == unitState.moving)
        {
            if (transform.position == worldPosition)
            {
                currentState = unitState.idle;
                changeState(0);

                if (worldPosition == pos1)
                {
                    if (activeNodes[0] != null)
                        activeNodes[0].inWinPos = true;
                }

                gameData.gameModeInt.checkNodeData();
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
            case 1:
                unitStateMachine.changeState(new movingState());
                break;
            case 2:
                if (unitStateMachine.getCurrentState() != "deathState")
                {
                    unitStateMachine.changeState(new deathState());
                    unitDead = true;
                    gameData.gameModeInt.checkNodeData();
                }
                break;
        }
    }
}

public class stateMachine
{
    private state currentState, previousState;
    private baseUnit controller;
    private animationController animControl;
    private audioController audioControl;
    private particleController particleControl;

    public stateMachine(baseUnit _controller, animationController _animControl, audioController _audioControl, particleController _particleControl)
    {
        controller = _controller;
        animControl = _animControl;
        audioControl = _audioControl;
        particleControl = _particleControl;
    }

    public void changeState(state newState, GameObject stateIdentity = null)
    {
        if (currentState != null)
        {
            this.currentState.onExit();
        }
        this.previousState = this.currentState;
        this.currentState = newState;
        this.currentState.controller = controller;
        this.currentState.animControl = animControl;
        this.currentState.audioControl = audioControl;
        this.currentState.particleControl = particleControl;
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

    public baseUnit controller { get; set; }
    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
    public particleController particleControl { get; set; }
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

    public baseUnit controller { get; set; }
    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
    public particleController particleControl { get; set; }
}
public class idleState : state
{
    #region Members
    Vector3 lookDirection;
    Quaternion lookRotation;
    #endregion
    public void onEnter()
    {
        //Debug.Log("Idle State");
        animControl.setBool("idleState", true);
    }

    public void onUpdate()
    {
        //find the vector pointing from our position to the target
        if (controller.worldPosition == controller.pos1)
        {
            lookDirection = (controller.pos2 - controller.transform.position).normalized;
        }
        else
        {
            lookDirection = (controller.pos1 - controller.transform.position).normalized;
        }

        //create the rotation we need to be in to look at the target
        lookRotation = Quaternion.LookRotation(lookDirection);

        //rotate us over time according to speed until we are in the required rotation
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 4f);

        if (controller.selected)
            particleControl.playParticles(0);
        else
            particleControl.stopParticles(0);
    }

    public void onFixedUpdate()
    {

    }

    public void onExit()
    {
        animControl.setBool("idleState", false);
        particleControl.stopParticles(0);
    }

    public baseUnit controller { get; set; }
    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
    public particleController particleControl { get; set; }
}
public class movingState : state
{
    #region Members
    Vector3 lookDirection;
    Quaternion lookRotation;
    #endregion
    public void onEnter()
    {
        animControl.setBool("movingState", true);
        Debug.Log("Movement State");
        particleControl.InstantiateParticleEffect(1, controller.transform, 2f, Quaternion.identity, true);
        particleControl.InstantiateParticleEffect(2, controller.transform, 2f, Quaternion.identity);
    }

    public void onUpdate()
    {
        //find the vector pointing from our position to the target
        lookDirection = (controller.worldPosition - controller.transform.position).normalized;
        float dot = Vector3.Dot(controller.transform.forward, lookDirection);
        animControl.setFloat("movementXParam", dot);
        float localAnimParam = 1 + dot;

        if (controller.worldPosition == controller.pos1)
        {
            animControl.setFloat("movementYParam", -2 + localAnimParam);
        }
        else
        {
            animControl.setFloat("movementYParam", 2 - localAnimParam);
        }
        //create the rotation we need to be in to look at the target
        lookRotation = Quaternion.LookRotation(lookDirection);
        //rotate us over time according to speed until we are in the required rotation
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 4f);
    }

    public void onFixedUpdate()
    {

    }

    public void onExit()
    {
        animControl.setBool("movingState", false);
    }

    public baseUnit controller { get; set; }
    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
    public particleController particleControl { get; set; }
}
public class deathState : state
{
    public void onEnter()
    {
        //animControl.setTrigger("dead");
        //This will Handle Rerouting Neighbors/Killing the Unit
        if (controller.neighbors[0] != null)
            controller.neighbors[0].neighbors[1] = controller.neighbors[1];

        if (controller.neighbors[1] != null)
            controller.neighbors[1].neighbors[0] = controller.neighbors[0];

        controller.gameData.gameModeInt.onUnitDeath(controller.transform);
        animControl = null;
        audioControl = null;

        controller.gameData.Grid.gridControl.disableTargetSymbols(controller);
        baseCamera.triggerScreenShake(baseCamera.shakePresets.onUnitDeath);
        controller.destroyUnit();
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

    public baseUnit controller { get; set; }
    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
    public particleController particleControl { get; set; }
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

    public baseUnit controller { get; set; }
    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
    public particleController particleControl { get; set; }
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

    public baseUnit controller { get; set; }
    public animationController animControl { get; set; }
    public audioController audioControl { get; set; }
    public particleController particleControl { get; set; }
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
    AudioSource source;
    List<AudioClip> clips;
    public audioController(AudioSource _source, List<AudioClip> _clips)
    {
        source = _source;
        clips = _clips;
    }

    public void playSoundOnIndex(int index)
    {
        source.PlayOneShot(clips[index]);
    }

    public void playSoundOnClip(AudioClip clip) 
    {
        source.PlayOneShot(clip);
    }

    public void playSoundClipBetweenRange(int start, int end)
    {
        source.PlayOneShot(clips[Random.Range(start, end)]);
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

public class particleController
{
    ParticleSystem[] unitParticleSystems;
    public particleController(ParticleSystem[] _unitParticleSystems)
    {
        unitParticleSystems = _unitParticleSystems;
    }

    public void playParticles(int particleIndex)
    {
        if (!unitParticleSystems[particleIndex].isPlaying)
            unitParticleSystems[particleIndex].Play();
    }

    public void stopParticles(int particleIndex)
    {
        if (!unitParticleSystems[particleIndex].isStopped)
            unitParticleSystems[particleIndex].Stop();
    }

    public void stopAllParticles()
    {
        for (int i = 0; i < unitParticleSystems.Length; i++)
        {
            if (unitParticleSystems[i].isPlaying)
                unitParticleSystems[i].Stop();
        }
    }

    public void InstantiateParticleEffect(int particleIndex, Transform transform, float destroyTime, Quaternion particleSystemRotation, bool followObject = false)
    {
        GameObject clone = Object.Instantiate(unitParticleSystems[particleIndex], transform.position, particleSystemRotation).gameObject;
        if (followObject)
        {
            var followScript = clone.gameObject.AddComponent<trailFollow>();
            followScript.target = transform;
        }
        Object.Destroy(clone, destroyTime);
    }
}
#endregion
