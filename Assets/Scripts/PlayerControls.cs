using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private InputActions inputActions;


    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += MovePerformed;
        inputActions.Player.Move.canceled += MovePerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= MovePerformed;
        inputActions.Player.Move.canceled -= MovePerformed;
    }

    private void MovePerformed(InputAction.CallbackContext context)
    {

    }

}
