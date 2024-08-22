using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(menuName = "Input Reader", fileName = "New Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    private Action _onPointerClicked;
    private Action _onPointerClickedRelease;
    private Action<Vector2> _onPointerDrag;
    private GameInput _gameInput;

    public Action OnPointerClicked { get => _onPointerClicked; set => _onPointerClicked = value; }
    public Action OnPointerClickedRelease { get => _onPointerClickedRelease; set => _onPointerClickedRelease = value; }
    public Action<Vector2> OnPointerDrag { get => _onPointerDrag; set => _onPointerDrag = value; }


    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
        }

        _gameInput.Player.Enable();
        _gameInput.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        if (_gameInput != null)
        {
            _gameInput.Player.Disable();
            _gameInput.Player.RemoveCallbacks(this);
        }
    }

    public void OnPointerClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnPointerClicked?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnPointerClickedRelease?.Invoke();
        }
    }

    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        OnPointerDrag?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnPointerClicked?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnPointerClickedRelease?.Invoke();
        }
    }
    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        OnPointerDrag?.Invoke(context.ReadValue<Vector2>());
    }
}
