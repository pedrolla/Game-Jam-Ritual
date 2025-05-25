using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Demon2 : MonoBehaviour
{
    // Precisas de apontar a lanterna para ele sair senão ele dá te jumpscare e morres.
    // Tem um timer que vai aumentando quando ele é ativado.
    // Se chegar ao valor máximo ele mata-te.
    // Quando lhe mandas luz da lanterna vai diminuindo o timer, se chegar a 0 ele é desativado.

    //int of timer, if it reaches an x value u die
    //while lantern on the timer is getting closser to 0
    //make demon spawn in assigned place by the manager

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private int startTimer = 7;

    [SerializeField] private int maxTimer = 15;

    private int timer;

    private bool lightOn = false;
    private bool isActive;
    private bool firstTimeLook = true;

    private void Awake()
    {
        spriteRenderer.enabled = false;
    }

    public void SpawnedDemon(string position)
    {
        DemonManager.Instance.AddActiveDemons(gameObject);

        if (position == "front")
        {
            spriteRenderer.sprite = frontSprite;

            Vector2 currentPos = transform.position;
            currentPos = new Vector2(currentPos.x - 0.37f, currentPos.y - 0.11f);
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

        timer = startTimer;
        isActive = true;
        StartCoroutine(AddTime());
        StartCoroutine(TakeTime());
    }

    public void FlashlightOn()
    {
        if (isActive)
        {
            if (firstTimeLook)
            {
                firstTimeLook = false;
                cameraController.LockCamera();
            }

            lightOn = true;

            spriteRenderer.enabled = true;
        }
    }


    public void Lightoff()
    {
        if (isActive && lightOn)
        {
            lightOn = false;
            spriteRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (timer <= 0 && isActive)
        {
            timer = startTimer;
            DeactivateDemon();
        }

        else if (timer >= maxTimer)
        {
            timer = startTimer;
            GrandmaDead();
            DeactivateDemon();
        }
    }

    private void GrandmaDead()
    {
        JumpscareManager.Instance.PlayCatJumpScare();
    }

    private void DeactivateDemon()
    {
        isActive = false;
        spriteRenderer.enabled = false;
        timer = startTimer;
        firstTimeLook = true;
        DemonManager.Instance.ReturnDemon(gameObject);
        PositionManager.Instance.ReleasePosition(gameObject);
        DemonManager.Instance.RemoveActivateDemons(gameObject);
    }

    private IEnumerator AddTime()
    {
        while (isActive)
        {
            if (!lightOn && timer < maxTimer)
            {
                timer++;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator TakeTime()
    {
        while (isActive)
        {
            if (lightOn && timer > 0)
            {
                timer -= 2;
                if (timer < 0) timer = 0;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }
}
