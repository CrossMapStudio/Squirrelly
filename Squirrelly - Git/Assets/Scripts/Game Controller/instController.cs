using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instController : MonoBehaviour
{
    public GameObject gameController;
    private void Awake()
    {
        if (!GameObject.FindGameObjectWithTag("GameController"))
        {
            var clone = Instantiate(gameController);
        }
    }
}
