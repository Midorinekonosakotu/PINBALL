using UnityEngine;

/// <summary>
/// ƒ{[ƒ‹‚ÌˆÚ“®ˆ—
/// </summary>
public class BallController : MonoBehaviour
{
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitializeAtPosition(Vector3 position)
    {
        rb.position = position;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void LostBall()
    {
        if(GameManager.Instance.State != GameStates.Playing)
        {
            return;
        }

        GameManager.Instance.OnBallLost();

        Destroy(gameObject);
    }
}
