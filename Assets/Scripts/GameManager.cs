using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton( 土のスクリプトからもアクセスできる )

    public GameStates State;

    public int Score;
    public int RemainingBalls;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        Score = 0;
        RemainingBalls = 3;

        ChangeState(GameStates.LaunchReady);
    }

    /// <summary>
    /// 状態遷移
    /// switch で管理
    /// </summary>
    /// <param name="newstate"></param>
    public void ChangeState(GameStates newstate)
    {
        State = newstate;

        switch(newstate)
        {
            case GameStates.Idle:
                Debug.Log("Idle状態");
                break;

            case GameStates.LaunchReady:
                Debug.Log("発射待機");
                break;

            case GameStates.Playing:
                Debug.Log("プレイ開始");
                break;

            case GameStates.BallLost:
                HandleBallLost();
                break;

            case GameStates.GameOver:
                Debug.Log("ゲームオーバー");
                break;
        }
    }

    /// <summary>
    /// ボールロスト処理
    /// </summary>
    void HandleBallLost()
    {
        RemainingBalls--;

        if (RemainingBalls > 0)
        {
            ChangeState(GameStates.LaunchReady);
        }
        else
        {
            ChangeState(GameStates.GameOver);
        }
    }
}
