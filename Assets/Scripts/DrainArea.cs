using UnityEngine;

/// <summary>
/// ボールロストの処理
/// </summary>
public class DrainArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger検出" + other.gameObject.name);
        if (!other.CompareTag("Ball"))
        {
            return;
        }

        BallController ball = other.GetComponent<BallController>();
        ball?.LostBall();
    }
}
