using System;
using System.Collections.Generic;
using UnityEngine;

public class vehicleController : MonoBehaviour
{
    public List<roadNodeContainer> paths;
    [SerializeField] private GameObject[] vehicleUnits;
    [HideInInspector]
    public int waveCount = 0, waveTarget, currentWaveIncremental;
    [HideInInspector]
    public float currentSpawnMultiplier, targetSpawnTime;
    public float screenShakeDistanceModifier, audioDepreciationTarget = 20f;
    private float currentSpawnCounter;

    private void Awake()
    {
        roadNode startShakeTrigger = null;
        roadNode endShakeTrigger = null;
        foreach (roadNodeContainer element in paths)
        {
            for (int i = 0; i < element.nodes.Count; i++)
            {
                if (i+1<element.nodes.Count)
                    element.nodes[i].nextNode = element.nodes[i + 1];

                element.nodes[i].setPos();
                if (element.nodes[i].startTriggerCameraShake)
                    startShakeTrigger = element.nodes[i];

                if (element.nodes[i].endTriggerCameraShake && startShakeTrigger != null)
                {
                    endShakeTrigger = element.nodes[i];
                    startShakeTrigger.endNode = endShakeTrigger;
                }
            }
        }
    }

    private void Update()
    {
        if (!gameController.pauseState && !gameController.gameIntroState)
        {
            if (currentSpawnCounter >= targetSpawnTime)
            {
                int spawnPos = UnityEngine.Random.Range(0, paths.Count);
                var clone = Instantiate(vehicleUnits[UnityEngine.Random.Range(0, vehicleUnits.Length)], paths[spawnPos].nodes[0].position, Quaternion.identity);
                clone.GetComponent<baseVehicle>().setList(paths[spawnPos].nodes, screenShakeDistanceModifier, audioDepreciationTarget);
                currentSpawnCounter = 0f;
            }
            else
            {
                currentSpawnCounter += Time.deltaTime * (1 + currentWaveIncremental * currentSpawnMultiplier);
            }
        }
    }

    public void checkWaveIncrementation()
    {
        waveCount++;
        if (waveCount >= waveTarget)
        {
            currentWaveIncremental++;
            waveCount = 0;
        }
    }
}

[Serializable]
public class roadNodeContainer
{
    public List<roadNode> nodes;
}

[Serializable]
public class roadNode
{
    public GameObject targetNodeMarker;
    public bool startTriggerCameraShake;
    public bool endTriggerCameraShake;
    [HideInInspector]
    public Vector3 position;
    public roadNode nextNode;
    public roadNode endNode;

    public void setPos()
    {
        position = targetNodeMarker.transform.position;
    }
}
