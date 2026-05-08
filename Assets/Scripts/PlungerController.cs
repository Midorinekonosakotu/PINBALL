using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プランジャーの挙動処理
/// </summary>
public class PlungerController : MonoBehaviour
{
    public Transform LaunchPoint;
    public Rigidbody CurrentBall;
    public Transform PlungerVisual;
    public Transform EndPoint;

    public float MaxForce = 3f;
    public float MaxCharge = 1f;
    public float ChargeSpeed = 1.5f;

    public float MaxPullDistance = 0.3f;

    private float charge;
    private bool isCharging;
    private Vector3 initialLocalPos;    // プランジャーの初期位置

    // Input System
    private PinballInputActions inputActions;

    private void Awake()
    {
        inputActions = new PinballInputActions();
    }

    private void OnEnable()
    {
        initialLocalPos = PlungerVisual.localPosition;
        inputActions.Enable();

        inputActions.GamePlay.Plunger.performed += OnPlunger;
        inputActions.GamePlay.Plunger.canceled += OnPlungerRelease;
    }

    private void OnDisable()
    {
        inputActions.GamePlay.Plunger.performed -= OnPlunger;
        inputActions.GamePlay.Plunger.canceled -= OnPlungerRelease;

        inputActions.Disable();
    }

    private void Update()
    {
        // GameStates制御
        if (GameManager.Instance.State != GameStates.LaunchReady)
        {
            return;
        }

        // チャージ中
        if (isCharging)
        {
            charge += ChargeSpeed * Time.deltaTime;
            charge = Mathf.Clamp(charge, 0f, MaxCharge);

            UpdateVisual();
        }
    }

    // 押している間
    private void OnPlunger(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.State != GameStates.LaunchReady)
        {
            return;
        }

        //Debug.Log("Plunger Pressed");
        isCharging = true;
    }

    // 離した瞬間
    private void OnPlungerRelease(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.State != GameStates.LaunchReady)
        {
            return;
        }

        //Debug.Log("Plunger Released");
        LaunchBall();

        isCharging = false;
        charge = 0f;

        ResetVisual();

        GameManager.Instance.ChangeState(GameStates.Playing);
    }

    private void LaunchBall()
    {
        if (CurrentBall == null) return;

        Rigidbody rb = CurrentBall;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // ボールに向かって射出
        float force = charge * MaxForce;
        rb.AddForce(LaunchPoint.forward * force, ForceMode.Impulse);
    }

    /// <summary>
    /// 見た目の更新
    /// </summary>
    private void UpdateVisual()
    {
        if (PlungerVisual == null)
        {
            return;
        }

        float pull = charge * MaxPullDistance;
        float curved = Mathf.Pow(charge, 2f);
        Vector3 localBack = -PlungerVisual.forward;

        // ローカルZ方向に引く想定
        PlungerVisual.localPosition = Vector3.Lerp(initialLocalPos, EndPoint.localPosition, curved);
    }

    /// <summary>
    /// 見た目のリセット
    /// </summary>
    private void ResetVisual()
    {
        if (PlungerVisual == null)
        {
            return;

        }

        PlungerVisual.localPosition = initialLocalPos;
    }

    /// <summary>
    /// 外部からボールをセット
    /// </summary>
    public void SetBall(Rigidbody ball)
    {
        CurrentBall = ball;

        ball.transform.localPosition = LaunchPoint.position;
        ball.transform.localRotation = LaunchPoint.rotation;
    }
}