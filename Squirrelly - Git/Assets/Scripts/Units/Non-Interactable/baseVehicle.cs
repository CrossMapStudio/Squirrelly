using GridHandler;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

public class baseVehicle : MonoBehaviour
{
    [HideInInspector] //Sets Dynamically
    public List<roadNode> nodePositions;
    private roadNode currentTargetNode;
    private int index;

    private Quaternion lookRotation;
    [SerializeField] private float vehicleSpeed, vehicleRotationMultiplier = 5f;

    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private Vector3 halfExt, originModification;


    private static bool vehicleShakingCamera;
    private float cameraShakeTracker;
    private float vehicleDistanceToClosingNode;

    //Shake Start Node
    roadNode startShakeNode;
    Vector3 targetMidPoint;
    float distance, distanceModifier;
    //Debugging Purpose
    public GameObject midPointMarker;

    //Audio
    private audioController audioControl;
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> driveBySounds;
    private float distanceToBeginAudioDepreciation;
    private bool oneHit;

    public void Awake()
    {
        Destroy(gameObject, 8f);
        audioSource = GetComponent<AudioSource>();
        audioControl = new audioController(audioSource, driveBySounds);
    }

    public void setList(List<roadNode> _nodePositions, float distModifier, float audioModifier)
    {
        distanceModifier = distModifier;
        distanceToBeginAudioDepreciation = audioModifier;
        nodePositions = _nodePositions;
        currentTargetNode = nodePositions[1];
        transform.rotation = Quaternion.LookRotation((currentTargetNode.position - transform.position).normalized);

        if (nodePositions[0].startTriggerCameraShake)
            vehicleShakingCamera = true;
    }

    private void Update()
    {
        if (currentTargetNode != null && !gameController.pauseState && !gameController.gameIntroState)
        {
            if (Vector3.Distance(transform.position, currentTargetNode.position) <= .5f)
            {
                if (currentTargetNode.nextNode != null)
                {
                    index++;
                    currentTargetNode = currentTargetNode.nextNode;
                    if (currentTargetNode.startTriggerCameraShake && !vehicleShakingCamera || nodePositions[0].startTriggerCameraShake && !vehicleShakingCamera)
                    {
                        vehicleShakingCamera = true;
                        if (currentTargetNode.startTriggerCameraShake)
                            startShakeNode = currentTargetNode;
                        else
                            startShakeNode = nodePositions[0];

                        targetMidPoint = (startShakeNode.position + startShakeNode.endNode.position) / 2;
                        var clone = Instantiate(midPointMarker, targetMidPoint, Quaternion.identity);
                    }
                    else if (currentTargetNode.endTriggerCameraShake && vehicleShakingCamera)
                        vehicleShakingCamera = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            lookRotation = Quaternion.LookRotation((currentTargetNode.position - transform.position).normalized);
            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * vehicleRotationMultiplier);

            if (vehicleShakingCamera)
            {
                distance = Vector3.Distance(transform.position, targetMidPoint);
                //baseCamera.valueBasedScreenShake(Mathf.Clamp(distanceModifier - distance, 0f, .2f) + .2f, 2f);
                if (distance >= distanceToBeginAudioDepreciation)
                    audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * 3f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentTargetNode != null && !gameController.pauseState && !gameController.gameIntroState)
            transform.position += transform.forward * vehicleSpeed * Time.fixedDeltaTime;

        Collider[] collisionData = Physics.OverlapBox(transform.position + originModification, halfExt, Quaternion.identity, unitLayerMask);
        if (collisionData != null)
        {
            foreach(Collider element in collisionData)
            {
                float localDot = Vector3.Dot(element.transform.position.normalized, transform.forward);
                element.GetComponent<baseUnit>().changeState(2);
                if (!oneHit)
                {
                    audioControl.playSoundOnIndex(1);
                    oneHit = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + originModification, halfExt);
    }
}
