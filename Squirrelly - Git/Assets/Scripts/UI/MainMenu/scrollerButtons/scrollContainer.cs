using UnityEngine;

public class scrollContainer : MonoBehaviour
{
    [HideInInspector]
    public Vector3 originPoint;
    [SerializeField] GameObject scroller;
    public float xIncremental;
    // Update is called once per frame
    private void Awake()
    {
        originPoint = scroller.transform.localPosition;
    }

    void Update()
    {
        if (scroller.transform.localPosition != originPoint)
        {
            scroller.transform.localPosition = Vector3.Lerp(scroller.transform.localPosition, originPoint, Time.deltaTime * 5f);
        }
    }
}
