using System.Collections.Generic;
using UnityEngine;

public class baseUnit : MonoBehaviour
{
    private Rigidbody rb;
    [HideInInspector]
    public Vector3 worldPosition, pos1, pos2;
    [HideInInspector]
    public int winStatePos;
    [HideInInspector]
    public bool winState;
    [HideInInspector]
    public Vector3[] winStateCorr; 
    [HideInInspector]
    public baseUnit[] neighbors = new baseUnit[2];
    [HideInInspector]
    public bool selected = false, potential = false;
    [HideInInspector]
    public baseUnit entity;

    public float speedScaler;
    private MeshRenderer mRend;
    private Material baseMat;
    public Material selectedMat, potentialMat;

    //Add to this?
    public static List<baseUnit> finalList = new List<baseUnit>();

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

    public void Update()
    {
        Material temp = selected ? selectedMat : baseMat;
        temp = potential ? potentialMat : temp;

        if (temp != mRend.material)
            mRend.material = temp;
    }

    public void FixedUpdate()
    {
        if (transform.position != worldPosition)
        {
            //Begin moving the block at the speed
            Vector3 target = worldPosition - transform.position;
            rb.MovePosition(transform.position + (target.normalized * speedScaler) * Time.fixedDeltaTime);
        }

        Debug.Log(winStateCorr[winStatePos].x);
        if (Mathf.Abs(transform.position.x - winStateCorr[winStatePos].x) <= .5f)
        {
            if (!finalList.Contains(this))
            finalList.Add(this);
        }
        else
        {
            if (finalList.Contains(this))
                finalList.Remove(this);
        }
    }
}
