using System.Collections;
using UnityEngine;

/// <summary>
/// ピンボールの全てのイベント管理
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameStates State;

    //public int Score;
    public int MaxBalls = 5;    // 最大ボール数
    public int RemainingBalls;  // 残りボール数
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlungerController plunger;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
       StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        //Score = 0;
        RemainingBalls = MaxBalls;

        yield return null;

        SpawnBall();
        ChangeState(GameStates.LaunchReady);
    }

    /// <summary>
    /// ロスト処理
    /// </summary>
    public void OnBallLost()
    {
        if(State != GameStates.Playing)
        {
            return;
        }

        ChangeState(GameStates.BallLost);

        HandleBallLost();
    }

    void HandleBallLost()
    {
        RemainingBalls--;
        Debug.Log("残機: " + RemainingBalls);

        if (RemainingBalls > 0)
        {
            //Debug.Log("Spawn実行");
            SpawnBall();
            ChangeState(GameStates.LaunchReady);
        }
        else
        {
            //Debug.Log("GameOver");
            ChangeState(GameStates.GameOver);
        }
    }

    /// <summary>
    /// ボールの生成
    /// </summary>
    void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        rb.isKinematic = true;
        plunger.SetBall(rb);
    }

    /// <summary>
    /// 状態遷移
    /// </summary>
    public void ChangeState(GameStates newstate)
    {
        State = newstate;
        Debug.Log("State: " + State);
    }
}
