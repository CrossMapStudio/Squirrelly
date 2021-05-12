using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDEController : MonoBehaviour
{
    private bool showing;
    public void toggleLDE()
    {
        showing = !showing;
        gameObject.SetActive(showing);
    }
}
