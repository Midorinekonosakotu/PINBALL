using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// フリッパーの入力処理
/// </summary>
public class FlipperController : MonoBehaviour
{
    [SerializeField] private FlipperInput _leftFlipper;
    [SerializeField] private FlipperInput _rightFlipper;

    private PinballInputActions _actions;

    void Awake()
    {
        _actions = new PinballInputActions();
    }

    void OnEnable()
    {
        _actions.Enable();

        // 左フリッパー
        _actions.GamePlay.LeftFlipper.performed += OnLeftPerformed;
        _actions.GamePlay.LeftFlipper.canceled += OnLeftCanceled;

        // 右フリッパー
        _actions.GamePlay.RightFlipper.performed += OnRightPerformed;
        _actions.GamePlay.RightFlipper.canceled += OnRightCanceled;
    }

    void OnDisable()
    {
        _actions.Disable();

        // 左フリッパー
        _actions.GamePlay.LeftFlipper.performed -= OnLeftPerformed;
        _actions.GamePlay.LeftFlipper.canceled -= OnLeftCanceled;

        // 右フリッパー
        _actions.GamePlay.RightFlipper.performed -= OnRightPerformed;
        _actions.GamePlay.RightFlipper.canceled -= OnRightCanceled;
    }

    /// <summary>
    /// 左フリッパーの処理
    /// </summary>
    /// <param name="ctx"></param>
    private void OnLeftPerformed(InputAction.CallbackContext ctx)
    {
        //if (GameManager.Instance.State != GameState.Playing) return;

        _leftFlipper.SetInput(true);
    }

    private void OnLeftCanceled(InputAction.CallbackContext ctx)
    {
        _leftFlipper.SetInput(false);
    }

    /// <summary>
    /// 右フリッパーの処理
    /// </summary>
    /// <param name="ctx"></param>
    private void OnRightPerformed(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.State != GameStates.Playing)
        {
            return;
        }

        _rightFlipper.SetInput(true);
    }

    private void OnRightCanceled(InputAction.CallbackContext ctx)
    {
        _rightFlipper.SetInput(false);
    }
}
