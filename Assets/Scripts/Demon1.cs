using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Demon1 : MonoBehaviour
{
    private bool isLatern;
    [SerializeField]
    private bool isActive;
    private bool isRight;
    private bool firstTimeLook = true;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite leftSprite;
    [SerializeField] 
    private Sprite rightSprite;
    [SerializeField]
    private CameraController cameraController;


    private void Awake()
    {
        spriteRenderer.enabled = false;
    }

    public void Flashlight()
    {
        if (isActive)
        {
            if (firstTimeLook)
            {
                firstTimeLook = false;
            }

            isLatern = true;
            spriteRenderer.enabled = true;
            StartCoroutine(DemonTime());
        }
    }

    public void OffFlashlight()
    {
        if (isActive && isLatern)
        {
            StopAllCoroutines();
            isActive = false;
            spriteRenderer.enabled = false;
            ReturnDemon();

            if (!isRight)
            {
                cameraController.RotateControlsLeft();
                return;
            }

            cameraController.RotateControlsRight();
        }

        isLatern = false;
    }

    private IEnumerator DemonTime()
    {
        while (isLatern)
        {
            yield return new WaitForSeconds(5);
            Kill();
        }
    }

    private void Kill()
    {
        StopAllCoroutines();
        JumpscareManager.Instance.PlayHandJumpScare();
    }

    public void ActivateDemon(string position)
    {
        DemonManager.Instance.AddActiveDemons(gameObject);

        isActive = true;

        float index = Random.value;

        if (position == "front")
        {
            spriteRenderer.sprite = frontSprite;
            isRight = false;

            Vector2 currentPos = transform.position;
            float randomNum = Random.Range(-3.5f, 4);
            currentPos = new Vector2(currentPos.x + 6.05f, currentPos.y + randomNum);
            transform.position = currentPos;
        }

        if (position == "right")
        {
            isRight = false;
            Vector2 currentPos = transform.position;
            float randomNum = Random.Range(-6, 4.5f);
            currentPos = new Vector2(currentPos.x, currentPos.y + randomNum);
            transform.position = currentPos;
            spriteRenderer.sprite = rightSprite;
        }

        if (position == "left")
        {
            isRight = true;
            Vector2 currentPos = transform.position;
            float randomNum = Random.Range(-9, 3);
            currentPos = new Vector2(currentPos.x, currentPos.y + randomNum);
            transform.position = currentPos;
            spriteRenderer.sprite = leftSprite;
        }
    }

    private void ReturnDemon()
    {
        firstTimeLook = true;
        DemonManager.Instance.ReturnDemon(gameObject);
        PositionManager.Instance.ReleasePosition(gameObject);
        DemonManager.Instance.RemoveActivateDemons(gameObject);
    }
}
