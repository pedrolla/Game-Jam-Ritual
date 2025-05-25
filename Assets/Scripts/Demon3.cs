using System.Collections;
using UnityEngine;

public class Demon3 : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private bool isActive;
    private bool firstTimeLook = true;
    [SerializeField]
    private float lanternCD = 1f;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite normalSprite;


    private void Start()
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
                cameraController.LockCamera();
            }

            isActive = false;
            spriteRenderer.enabled = true;
            StartCoroutine(FlashlightOff());
        }
    }

    private IEnumerator FlashlightOff()
    {
        yield return new WaitForSeconds(lanternCD);
        cameraController.FlashlightLimit();
        spriteRenderer.enabled = false;
        firstTimeLook = true;
        DemonManager.Instance.ReturnDemon(gameObject);
        PositionManager.Instance.ReleasePosition(gameObject);
        DemonManager.Instance.RemoveActivateDemons(gameObject);
    }

    public void ActivateDemon(string position)
    {
        DemonManager.Instance.AddActiveDemons(gameObject);

        if (position == "front")
        {
            spriteRenderer.sprite = frontSprite;
            Vector2 currentPos = transform.position;
            currentPos = new Vector2(currentPos.x - 0.52f, currentPos.y - 1.58f);
            transform.position = currentPos;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
            if (position == "left")
            {
                Vector2 currentPos = transform.position;
                currentPos = new Vector2(currentPos.x - 5, currentPos.y);
                transform.position = currentPos;
                transform.right = -transform.right;
            }
            if (position == "right")
            {
                Vector2 currentPos = transform.position;
                currentPos = new Vector2(currentPos.x, currentPos.y - 2.5f);
                transform.position = currentPos;
            }
        }

            Debug.Log("demon3 activated");
        isActive = true;
    }
}
