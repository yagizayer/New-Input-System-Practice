/// Author: Yağız A. AYER
/// Github: github.com/yagizayer
/// Date: 30 May 2021
/// Used Style guide: Google C# StyleGuide (https://google.github.io/styleguide/csharp-style.html)


using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


// creating templates for required events
[Serializable]
public class MoveInputEvent : UnityEvent<float, float> { }
[Serializable]
public class SprintInputEvent : UnityEvent<bool> { }


public class InputHandler : MonoBehaviour
{

    [Tooltip("Player character event for forward, backward, left, right movement")]
    [SerializeField] private MoveInputEvent _moveInputEvent;
    [Tooltip("Player character event for sprint movement")]
    [SerializeField] private SprintInputEvent _sprintInputEvent;
    private InputManager _inputManager;

    private void Awake()
    {
        _inputManager = new InputManager();
    }
    private void OnEnable()
    {
        _inputManager.Gameplay.Enable();
        _inputManager.Gameplay.Move.performed += OnMovePerformed;
        _inputManager.Gameplay.Move.canceled += OnMovePerformed;    // we are binding on cancelled too because we want to 
                                                                    // perform one more movement(stopping move) when player relase the key
        _inputManager.Gameplay.Sprint.started += OnSprintStarted;
        _inputManager.Gameplay.Sprint.canceled += OnSprintStarted;  // we are binding on cancelled too because we want to 
                                                                    // perform one more movement(stopping move) when player relase the key
    }
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector3 moveInput = context.ReadValue<Vector2>();
        _moveInputEvent.Invoke(moveInput.x, moveInput.y);
    }
    void OnSprintStarted(InputAction.CallbackContext context)
    {
        if (context.started)
            _sprintInputEvent.Invoke(true);
        if (context.canceled)
            _sprintInputEvent.Invoke(false);
    }
}
