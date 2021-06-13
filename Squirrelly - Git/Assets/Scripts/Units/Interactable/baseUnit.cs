using GridHandler;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class baseUnit : MonoBehaviour
{
    private Rigidbody rb;
    [HideInInspector]
    public Vector3 worldPosition, pos1, pos2;
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
            //Begin moving the block at the speed
            target = worldPosition - transform.position;
            rb.MovePosition(transform.position + (target.normalized * speedScaler) * Time.fixedDeltaTime);
            if (currentState != unitState.moving)
                currentState = unitState.moving;
        }

        if (currentState == unitState.moving)
        {
            if (Vector3.Distance(worldPosition, transform.position) <= .2f)
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
