using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textGenerator : MonoBehaviour
{
    public GameObject textHolder;
    public static textGenerator textG;

    public void Awake()
    {
        textG = this;
    }

    public static void textOnUnit(Transform _transform, string text, Color textColor)
    {
        var clone = Instantiate(textG.textHolder, _transform.position, Quaternion.identity);
        clone.transform.LookAt(Camera.main.transform.position);
        Vector3 local = clone.transform.rotation.eulerAngles;
        local.z = 0f;
        clone.transform.rotation = Quaternion.Euler(local);
        TextMesh element = clone.transform.GetChild(0).GetComponent<TextMesh>();
        element.text = text;
        element.color = textColor;
        Destroy(clone, 2f);
    }

    public static void textOnGUI()
    {

    }
}
