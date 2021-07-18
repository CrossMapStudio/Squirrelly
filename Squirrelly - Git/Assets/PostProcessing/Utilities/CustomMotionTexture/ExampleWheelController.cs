using UnityEngine;

public class ExampleWheelController : MonoBehaviour
{
    public float acceleration;
    public Renderer motionVectorRenderer; // Reference to the custom motion vector renderer

    Rigidbody m_Rigidbody;

    static class Uniforms
    {
        internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); // Get reference to rigidbody
        m_Rigidbody.maxAngularVelocity = 100; // Set max velocity for rigidbody
    }
}
