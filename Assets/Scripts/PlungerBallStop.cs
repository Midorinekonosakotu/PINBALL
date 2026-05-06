using UnityEngine;

public class PlungerBallStop : MonoBehaviour
{
    public Transform lockPoint;

    private Rigidbody lockedBall;

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.Instance.State != GameStates.LaunchReady)
            return;

        if (!other.CompareTag("Ball"))
            return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        lockedBall = rb;

        // 完全固定
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = lockPoint.position;
    }

    public Rigidbody GetLockedBall()
    {
        return lockedBall;
    }
}
