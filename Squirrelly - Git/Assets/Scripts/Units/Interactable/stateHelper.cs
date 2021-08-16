using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateHelper : MonoBehaviour
{
    [SerializeField] private baseUnit parentedUnit;
    public void animationTriggerToChangeStateFromAnimationUser(int state)
    {
        parentedUnit.changeState(state);
    }

    public void animationTriggerToDestroyUnit()
    {
        parentedUnit.destroyUnit();
    }
}
