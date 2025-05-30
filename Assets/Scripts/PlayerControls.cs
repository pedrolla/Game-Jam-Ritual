using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


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
    private int newHour;
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField] 
    private CinemachineCamera virtualCamera;
    [SerializeField] 
    private float maxZoom = 2f;
    [SerializeField]
    private float zoomDuration = 0.3f;
    [SerializeField]
    private float blurDuration = 0.2f;
    [SerializeField]
    private Volume myVolume;
    [SerializeField]
    private float originalSize;
    private bool isZoomingBack = false;


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
        originalSize = 9;
        NoBlur();
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

        if (remapped == Direction.S)
        {
            StartCoroutine(ZoomInBed());
        }
        else
        {
            TryLook(direction, rawInput);
        }
    }

    private IEnumerator ZoomInBed()
    {
        if (isLooking || isZoomingBack) yield break;

        isZoomingBack = true;
        isLooking = true;
        activeRawDirection = Direction.S;

        SoundManager.Instance.PlayLookUnderBed();
        FlashlightReleased();

        float elapsed = 0f;
        GetBlur();

        // Zoom in
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration;
            virtualCamera.Lens.OrthographicSize = Mathf.Lerp(originalSize, maxZoom, t);
            yield return null;
        }

        virtualCamera.Lens.OrthographicSize = maxZoom;

        yield return new WaitForSeconds(blurDuration);


        transform.position = new Vector3(backPoint.position.x, backPoint.position.y, transform.position.z);
        targetPoint = backPoint;
        virtualCamera.Lens.OrthographicSize = originalSize; // Reset zoom instantly


        NoBlur(); 

        isZoomingBack = false;
        isLooking = false;
    }

    private IEnumerator ZoomOutBed()
    {
        Debug.Log("ZoomOutBed called");

        if (isZoomingBack || isLooking) yield break;

        isZoomingBack = true;
        isLooking = true;
        activeRawDirection = null;

        FlashlightReleased();

        GetBlur();

        float elapsed = 0f;
        float currentSize = virtualCamera.Lens.OrthographicSize;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration;
            virtualCamera.Lens.OrthographicSize = Mathf.Lerp(currentSize, 10, t);
            yield return null;
        }



        yield return new WaitForSeconds(blurDuration);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = frontPoint.position; targetPoint = frontPoint;

        transform.position = new Vector3(targetPosition.x, targetPosition.y, startPosition.z);
        virtualCamera.Lens.OrthographicSize = originalSize;

        NoBlur();

        isZoomingBack = false;
        isLooking = false;
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

    public void ControlsDefault()
    {
        inputMap[Direction.A] = Direction.A;
        inputMap[Direction.S] = Direction.S;
        inputMap[Direction.D] = Direction.D;
        inputMap[Direction.W] = Direction.W;
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

        if (released == Direction.S)
        {
            StartCoroutine(ZoomOutBed());
            return;
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

    private void Update()
    {
        Clock();
        if (!isLocked && !isZoomingBack)
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
            UIManager.Instance.Flashlight1();
        }
    }

    private void Clock()
    {
        currenTime += Time.deltaTime;

        newHour = Mathf.FloorToInt(currenTime / secondsPerHour);

        if (newHour > currentHour)
        {
            currentHour = newHour;
            UIManager.Instance.UpdateTime(currentHour);

            if (currentHour == 6)
            {
                ClockScript.Instance.ChangeClock();
            }
        }
    }

    private void GetBlur()
    {
        DepthOfField dof;
        if (myVolume.profile.TryGet(out dof))
        {
            dof.active = true;
            dof.focusDistance.value = 10f;
        }
    }

    private void NoBlur()
    {
        DepthOfField dof;
        if (myVolume.profile.TryGet(out dof))
        {
            dof.active = false;
        }
    }
}
