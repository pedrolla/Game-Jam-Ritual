using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private InputActions inputActions;

    [SerializeField]
    private Transform targetPoint;
    [SerializeField]
    private Transform leftPoint;
    [SerializeField]
    private Transform rightPoint;
    [SerializeField]
    private Transform frontPoint;
    [SerializeField]
    private Transform backPoint;
    [SerializeField]
    private Transform upPoint;

    [SerializeField]
    private float cameraMoveSpeed = 5f;
    private bool isLooking;
    private bool isFlashlight;
    [SerializeField]
    private float cameraCD = 1f;
    private string activeInput;

    [SerializeField]
    private GameObject leftSprite;
    [SerializeField]
    private GameObject rightSprite;
    [SerializeField]
    private GameObject upSprite;
    [SerializeField]
    private GameObject frontSprite;
    [SerializeField]
    private GameObject backSprite;

    [SerializeField]
    private Sprite leftLight;
    [SerializeField]
    private Sprite leftDark;
    [SerializeField]
    private Sprite rightLight;
    [SerializeField]
    private Sprite rightDark;
    [SerializeField]
    private Sprite upLight;
    [SerializeField]
    private Sprite upDark;
    [SerializeField]
    private Sprite frontLight;
    [SerializeField]
    private Sprite frontDark;
    [SerializeField]
    private Sprite backLight;
    [SerializeField]
    private Sprite backDark;

    public UnityEvent flashlightBack; 
    public UnityEvent flashlightOff;
    public UnityEvent flashlightUp;


    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void Start()
    {
        targetPoint = frontPoint;
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.A.performed += ctx => TryLook(leftPoint, "a");
        inputActions.Player.S.performed += ctx => TryLook(backPoint, "s");
        inputActions.Player.D.performed += ctx => TryLook(rightPoint, "d");
        inputActions.Player.W.performed += ctx => TryLook(upPoint, "w");

        inputActions.Player.A.canceled += OnInputReleased;
        inputActions.Player.S.canceled += OnInputReleased;
        inputActions.Player.D.canceled += OnInputReleased;
        inputActions.Player.W.canceled += OnInputReleased;

        inputActions.Player.Flashlight.performed += FlashlightPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.A.performed -= ctx => TryLook(leftPoint, "a");
        inputActions.Player.S.performed -= ctx => TryLook(backPoint, "s");
        inputActions.Player.D.performed -= ctx => TryLook(rightPoint, "d");
        inputActions.Player.W.performed -= ctx => TryLook(upPoint, "w");

        inputActions.Player.A.canceled -= OnInputReleased;
        inputActions.Player.S.canceled -= OnInputReleased;
        inputActions.Player.D.canceled -= OnInputReleased;
        inputActions.Player.W.canceled -= OnInputReleased;

        inputActions.Player.Flashlight.performed -= FlashlightPerformed;

        inputActions.Disable();
    }


    private void TryLook(Transform direction, string key)
    {
        if (isLooking) return;

        targetPoint = direction;
        isLooking = true;
        activeInput = key;

        FlashlightReleased();
    }

    private void OnInputReleased(InputAction.CallbackContext context)
    {
        if (context.control.name != activeInput) return;

        targetPoint = frontPoint;
        if (isFlashlight)
        {
            FlashlightReleased();
        }
        StartCoroutine(CameraReset());
    }

    private IEnumerator CameraReset()
    {
        yield return new WaitForSeconds(cameraCD);
        isLooking = false;
        activeInput = null;
    }

    private void FlashlightPerformed(InputAction.CallbackContext context)
    {
        if (isFlashlight)
        {
            FlashlightReleased();
            return;
        }

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            Flashlight();
            return;
        }

        StartCoroutine(WaitForCamera());
    }

    private IEnumerator WaitForCamera()
    {
        while (Vector2.Distance(transform.position, targetPoint.position) > 0.01f)
        {
            yield return null;
        }

        Flashlight();
    }

    private void Flashlight()
    {
        isFlashlight = true;

        if (targetPoint.position == frontPoint.position)
        {
            frontSprite.GetComponent<SpriteRenderer>().sprite = frontLight;
        }

        if (targetPoint.position == leftPoint.position)
        {
            leftSprite.GetComponent<SpriteRenderer>().sprite = leftLight;
        }

        if (targetPoint.position == rightPoint.position)
        {
            rightSprite.GetComponent<SpriteRenderer>().sprite = rightLight;
        }

        if (targetPoint.position == upPoint.position)
        {
            upSprite.GetComponent<SpriteRenderer>().sprite = upLight;
            flashlightUp.Invoke();
        }

        if (targetPoint.position == backPoint.position)
        {
            backSprite.GetComponent<SpriteRenderer>().sprite = backLight;
            flashlightBack.Invoke();
        }
    }

    private void FlashlightReleased()
    {
        isFlashlight = false;
        flashlightOff.Invoke();

        frontSprite.GetComponent<SpriteRenderer>().sprite = frontDark;
        leftSprite.GetComponent<SpriteRenderer>().sprite = leftDark;
        rightSprite.GetComponent<SpriteRenderer>().sprite = rightDark;
        backSprite.GetComponent<SpriteRenderer>().sprite = backDark;
        upSprite.GetComponent<SpriteRenderer>().sprite = upDark;
    }

    private void Update()
    {
        if (targetPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(targetPoint.position.x, targetPoint.position.y, transform.position.z),
                cameraMoveSpeed * Time.deltaTime);
        }
    }
}
