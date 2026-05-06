using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(HingeJoint))]
public class FlipperInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private HingeJoint joint;

    [Header("Control")]
    [SerializeField] private float torque = 2000f;
    [SerializeField] private float impulseBoost = 1.8f;
    [SerializeField] private float maxAngularVelocity = 60f;
    [SerializeField] private float defaultAngle = 15f;

    [Header("Debug")]
    [SerializeField] private bool debugDraw = false;

    private bool isPressed;
    private bool justPressed;
    [SerializeField] private bool isLeft;

    void Awake()
    {
        if(!rb) rb = GetComponent<Rigidbody>();
        if(!joint) joint = GetComponent<HingeJoint>();
    }

    void Start()
    {
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.State != GameStates.Playing) return;    // Playing 中のみ動作

        float dir = isLeft ? -1f : 1f;
        JointSpring spring = joint.spring;

        if (isPressed)
        {
            joint.useSpring = false;

            rb.AddTorque(transform.up * torque * dir, ForceMode.Acceleration);

            if (justPressed)
            {
                rb.AddTorque(transform.up * torque * impulseBoost * dir, ForceMode.Impulse);
                justPressed = false;
            }
        }
        else
        {
            joint.useSpring = true;

            spring.targetPosition = defaultAngle;
            joint.spring = spring;
        }
    }

    public void SetInput(bool pressed)
    {
        if(pressed && !isPressed)
        {
            justPressed = true;
        }

        isPressed = pressed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Rigidbody ball)) return;

        Vector3 contactPoint = collision.contacts[0].point;

        Vector3 velocity = rb.GetPointVelocity(contactPoint);

        ball.AddForce(velocity * 0.8f, ForceMode.Impulse);

        if(debugDraw)
        {
            Debug.DrawRay(contactPoint, velocity, Color.red, 1f);
        }
    }
}
