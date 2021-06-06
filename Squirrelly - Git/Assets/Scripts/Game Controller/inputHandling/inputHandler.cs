using System.Collections;
using UnityEngine;

public class inputHandler : MonoBehaviour
{

    /// <summary>
    /// Using Primarily Mouse and Keyboard for Right Now This Input Handler will Control the Players ability to interact with the World
    /// </summary>

    private Camera main;
    private dataInterpreter data;
    private bool fireCast = true;

    private void Awake()
    {
        main = Camera.main;
        data = GetComponent<dataInterpreter>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            data.Grid.gridControl.startSwitch();
        }
    }

    private void FixedUpdate()
    {
        if (fireCast)
        {
            RaycastHit unitHit;
            Ray ray = main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out unitHit, data.unitLayer))
            {
                data.Grid.gridControl.checkNodeSelection(unitHit.collider.GetComponent<baseUnit>());
            }
            else
            {
               data.Grid.gridControl.checkNodeSelection(null);
            }
        }
    }
}
