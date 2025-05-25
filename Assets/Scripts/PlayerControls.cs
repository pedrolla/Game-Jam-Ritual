using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
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
    private bool flashlightLimit;
    private bool isDemon;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private float lockCD = 1f;
    [SerializeField]
    private float flashLightCD;
    [SerializeField]
    private float cameraCD = 1f;
    private int upStage = 1;
    private Direction? activeRawDirection;
    [SerializeField]
    private float flashLightDistance;

    [SerializeField]
    private GameObject mainSprite;
    [SerializeField]
    private GameObject upSprite;
    [SerializeField]
    private GameObject backSprite;

    [SerializeField]
    private Sprite mainDark;
    [SerializeField]
    private Sprite leftLight;
    [SerializeField]
    private Sprite rightLight;
    [SerializeField]
    private Sprite upLight1;
    [SerializeField]
    private Sprite upLight2;
    [SerializeField]
    private Sprite upLight3;
    [SerializeField]
    private Sprite frontLight;
    [SerializeField]
    private Sprite upDark;
    [SerializeField]
    private Sprite backLight;
    [SerializeField]
    private Sprite backDark;
    [SerializeField]
    private Sprite backMonster;

    [SerializeField]
    private float battery;
    [SerializeField]
    private float batteryRate = 10f;
    [SerializeField]
    private float secondsPerHour = 60;
    private float currenTime = 0f;
    [SerializeField]
    private int currentHour = 0;
    [SerializeField]
    private GameObject pauseMenu;


    public UnityEvent flashlightBack; 
    public UnityEvent flashlightOff;
    public UnityEvent flashlightUp;
    public UnityEvent demon1Light;
    public UnityEvent demon2Light;
    public UnityEvent demon3Light;


    private enum Direction { A, W, D, S }

    private Dictionary<Direction, Direction> inputMap = new Dictionary<Direction, Direction>
    {
        { Direction.A, Direction.A },
        { Direction.W, Direction.W },
        { Direction.D, Direction.D },
        { Direction.S, Direction.S }
    };

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

        inputActions.Player.A.performed += ctx => TryLookFromInput(Direction.A);
        inputActions.Player.S.performed += ctx => TryLookFromInput(Direction.S);
        inputActions.Player.D.performed += ctx => TryLookFromInput(Direction.D);
        inputActions.Player.W.performed += ctx => TryLookFromInput(Direction.W);
        inputActions.Player.Menu.performed += ShowPauseMenu;

        inputActions.Player.A.canceled += OnInputReleased;
        inputActions.Player.S.canceled += OnInputReleased;
        inputActions.Player.D.canceled += OnInputReleased;
        inputActions.Player.W.canceled += OnInputReleased;

        inputActions.Player.Flashlight.performed += FlashlightPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.A.performed -= ctx => TryLookFromInput(Direction.A);
        inputActions.Player.S.performed -= ctx => TryLookFromInput(Direction.S);
        inputActions.Player.D.performed -= ctx => TryLookFromInput(Direction.D);
        inputActions.Player.W.performed -= ctx => TryLookFromInput(Direction.W);

        inputActions.Player.A.canceled -= OnInputReleased;
        inputActions.Player.S.canceled -= OnInputReleased;
        inputActions.Player.D.canceled -= OnInputReleased;
        inputActions.Player.W.canceled -= OnInputReleased;

        inputActions.Player.Flashlight.performed -= FlashlightPerformed;

        inputActions.Disable();
    }

    private void ShowPauseMenu(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            pauseMenu.SetActive(true);
        }
        else HidePauseMenu();
    }

    public void HidePauseMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
    }

    private void TryLookFromInput(Direction rawInput)
    {
        Direction remapped = inputMap[rawInput];

        Transform direction = remapped switch
        {
            Direction.A => leftPoint,
            Direction.S => backPoint,
            Direction.D => rightPoint,
            Direction.W => upPoint,
            _ => frontPoint
        };

        TryLook(direction, rawInput);
    }

    public void RotateControlsLeft()
    {
        inputMap[Direction.A] = Direction.W;
        inputMap[Direction.W] = Direction.D;
        inputMap[Direction.D] = Direction.S;
        inputMap[Direction.S] = Direction.A;
    }

    public void RotateControlsRight()
    {
        inputMap[Direction.A] = Direction.S;
        inputMap[Direction.S] = Direction.D;
        inputMap[Direction.D] = Direction.W;
        inputMap[Direction.W] = Direction.A;
    }

    private void TryLook(Transform direction, Direction rawInput)
    {
        if (isLooking) return;

        targetPoint = direction;
        isLooking = true;
        activeRawDirection = rawInput;

        if (rawInput != Direction.S)
        {
            SoundManager.Instance.PlayMoveInBed();
        }
        else
        {
            SoundManager.Instance.PlayLookUnderBed();
        }

            FlashlightReleased();
    }

    private void OnInputReleased(InputAction.CallbackContext context)
    {
        Direction? released = GetDirectionFromControlName(context.control.name);
        if (!released.HasValue || activeRawDirection != released.Value) return;

            targetPoint = frontPoint;

            if (isFlashlight)
            {
                FlashlightReleased();
            }

            StartCoroutine(CameraReset());
    }

    private Direction? GetDirectionFromControlName(string controlName)
    {
        return controlName switch
        {
            "a" => Direction.A,
            "s" => Direction.S,
            "d" => Direction.D,
            "w" => Direction.W,
            _ => null
        };
    }


    private IEnumerator CameraReset()
    {
        yield return new WaitForSeconds(cameraCD);
        isLooking = false;
    }

    private void FlashlightPerformed(InputAction.CallbackContext context)
    {
        if (isFlashlight)
        {
            FlashlightReleased();
            return;
        }

        if (Vector2.Distance(transform.position, targetPoint.position) < flashLightDistance)
        {
            Flashlight();
            return;
        }

        StartCoroutine(WaitForCamera());
    }

    private IEnumerator WaitForCamera()
    {
        while (Vector2.Distance(transform.position, targetPoint.position) > flashLightDistance)
        {
            yield return null;
        }

        Flashlight();
    }

    private void Flashlight()
    {
        if (flashlightLimit)
        {
            return;
        }
            isFlashlight = true;

            if (Vector2.Distance(frontPoint.position, targetPoint.position) < flashLightDistance)
            {
            mainSprite.GetComponent<SpriteRenderer>().sprite = frontLight;
            }

            if (Vector2.Distance(leftPoint.position, targetPoint.position) < flashLightDistance)
            {
                mainSprite.GetComponent<SpriteRenderer>().sprite = leftLight;
            }

            if (Vector2.Distance(rightPoint.position, targetPoint.position) < flashLightDistance)
            {
                mainSprite.GetComponent<SpriteRenderer>().sprite = rightLight;
            }

            if (Vector2.Distance(upPoint.position, targetPoint.position) < flashLightDistance)
            {
            upSprite.GetComponent<SpriteRenderer>().sprite = CheckUpStage();
                flashlightUp.Invoke();
            }

            if (Vector2.Distance(backPoint.position, targetPoint.position) < flashLightDistance)
            {
            backSprite.GetComponent<SpriteRenderer>().sprite = backLight;
                flashlightBack.Invoke();
            }
        SoundManager.Instance.PlayFlashlight();
    }

    private void FlashlightReleased()
    {
        if (isLocked) return;

        if (isFlashlight)
        {
            SoundManager.Instance.PlayFlashlight();
        }
        isFlashlight = false;
        flashlightOff.Invoke();

        mainSprite.GetComponent<SpriteRenderer>().sprite = mainDark;
        backSprite.GetComponent<SpriteRenderer>().sprite = ChangeBack();
        upSprite.GetComponent<SpriteRenderer>().sprite = upDark;
    }

    public void FlashlightLimit()
    {
        flashlightLimit = true;
        isFlashlight = false;
        flashlightOff.Invoke();

        mainSprite.GetComponent<SpriteRenderer>().sprite = mainDark;
        backSprite.GetComponent<SpriteRenderer>().sprite = ChangeBack();
        upSprite.GetComponent<SpriteRenderer>().sprite = upDark; 
        StartCoroutine(FlashLightCD());
    }

    private IEnumerator FlashLightCD()
    {
        yield return new WaitForSeconds(flashLightCD);
        flashlightLimit = false;
    }

    public void LockCamera()
    {
        isLocked = true;
        StartCoroutine(UnlockCamera());
    }

    private IEnumerator UnlockCamera()
    {
        yield return new WaitForSeconds(lockCD);
        isLocked = false;
    }

    private void Update()
    {
        Clock();
        if (!isLocked)
        {
            if (targetPoint != null)
            {
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(targetPoint.position.x, targetPoint.position.y, transform.position.z),
                    cameraMoveSpeed * Time.deltaTime);
            }

            if (isFlashlight)
            {
                Battery();

                string currentDirection = GetDirection();

                if (PositionManager.Instance.GetDemonPosition("demon1") == currentDirection)
                {
                    demon1Light.Invoke();
                    Debug.Log("Invoke event");
                }

                if (PositionManager.Instance.GetDemonPosition("demon2") == currentDirection)
                {
                    demon2Light.Invoke();
                }

                if (PositionManager.Instance.GetDemonPosition("demon3") == currentDirection)
                {
                    demon3Light.Invoke();
                }
            }
        }
    }

    private string GetDirection()
    {
        if (targetPoint == frontPoint) return "front";
        if (targetPoint == leftPoint) return "left";
        if (targetPoint == rightPoint) return "right";
        else return null;
    }

    public void ChangeUpStage(int stage)
    {
        upStage = stage;
        if (upStage == 4)
        {
            upStage = 3;
        }

        if (targetPoint == upPoint && isFlashlight)
        {
            upSprite.GetComponent<SpriteRenderer>().sprite = CheckUpStage();
        }
    }

    private Sprite CheckUpStage()
    {
        return upStage switch
        {
            3 => upLight3,
            2 => upLight2,
            _ => upLight1,
        };
    }

    private Sprite ChangeBack()
    {
        if (isDemon)
        {
            return backMonster;
        }

        else return backDark;
    }

    public void IsBackMonster()
    {
        isDemon = true;
        backSprite.GetComponent<SpriteRenderer>().sprite = ChangeBack();
    }

    public void IsNotBackMonster()
    {
        isDemon = false;
    }

    private void Battery()
    {
        battery -= batteryRate * Time.deltaTime;
        
        if (battery <= 0)
        {
            battery = 0;
            UIManager.Instance.Flashlight0();

            flashlightLimit = true;
            isFlashlight = false;
            flashlightOff.Invoke();

            mainSprite.GetComponent<SpriteRenderer>().sprite = mainDark;
            backSprite.GetComponent<SpriteRenderer>().sprite = ChangeBack();
            upSprite.GetComponent<SpriteRenderer>().sprite = upDark;
        }

        if (battery < 80 && battery > 60)
        {
            UIManager.Instance.Flashlight4();
        }

        if (battery < 60 && battery > 40)
        {
            UIManager.Instance.Flashlight3();
        }

        if (battery < 40 && battery > 20)
        {
            UIManager.Instance.Flashlight2();
        }

        if (battery < 20 && battery > 0)
        {
            SoundManager.Instance.PlayLowBattery();
            UIManager.Instance.Flashlight1();
        }
    }

    private void Clock()
    {
        currenTime += Time.deltaTime;

        int newHour = Mathf.FloorToInt(currenTime / secondsPerHour);

        if (newHour > currentHour)
        {
            currentHour = newHour;
            UIManager.Instance.UpdateTime(currentHour);

            if (currentHour == 6)
            {
                // You win!
            }
        }
    }
}
