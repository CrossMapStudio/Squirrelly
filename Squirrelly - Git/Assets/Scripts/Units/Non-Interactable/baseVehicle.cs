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

    public void setList(List<roadNode> _nodePositions)
    {
        nodePositions = _nodePositions;
        currentTargetNode = nodePositions[1];
        transform.rotation = Quaternion.LookRotation((currentTargetNode.position - transform.position).normalized);
    }

    private void Update()
    {
        if (currentTargetNode != null)
        {
            if (Vector3.Distance(transform.position, currentTargetNode.position) <= .5f)
            {
                if (currentTargetNode.nextNode != null)
                {
                    index++;
                    currentTargetNode = currentTargetNode.nextNode;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            lookRotation = Quaternion.LookRotation((currentTargetNode.position - transform.position).normalized);
            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * vehicleRotationMultiplier);
        }
    }

    private void FixedUpdate()
    {
        if (currentTargetNode != null)
            transform.position += transform.forward * vehicleSpeed * Time.fixedDeltaTime;

        Collider[] collisionData = Physics.OverlapBox(transform.position + originModification, halfExt, Quaternion.identity, unitLayerMask);
        if (collisionData != null)
        {
            foreach(Collider element in collisionData)
            {
                float localDot = Vector3.Dot(element.transform.position.normalized, transform.forward);
                element.GetComponent<baseUnit>().changeState(2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + originModification, halfExt);
    }
}
