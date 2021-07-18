using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instController : MonoBehaviour
{
    public GameObject gameController;
    public Texture2D cursorTexture;
    private void Awake()
    {
        if (cursorTexture != null)
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        if (!GameObject.FindGameObjectWithTag("GameController"))
        {
            var clone = Instantiate(gameController);
        }
    }
}
