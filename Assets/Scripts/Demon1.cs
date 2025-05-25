using System.Collections;
using UnityEngine;

public class Demon1 : MonoBehaviour
{
    private bool isLatern;
    private bool isActive;
    [SerializeField]
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer.enabled = false;
    }

    public void Flashlight()
    {
        if (isActive)
        {
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
            DemonManager.Instance.ReturnDemon(gameObject);
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
        Debug.Log("Killed by 1");
        StopAllCoroutines();
    }

    public void ActivateDemon()
    {
        Debug.Log("activated");

        isActive = true;
    }
}
