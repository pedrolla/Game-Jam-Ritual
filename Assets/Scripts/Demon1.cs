using System.Collections;
using UnityEngine;

public class Demon1 : MonoBehaviour
{
    private bool isLatern;
    private bool isActive;
    [SerializeField]
    private SpriteRenderer spriteRenderer;


    private void Flashlight()
    {
            isLatern = true;
            StartCoroutine(ShowMonster());
    }

    private IEnumerator ShowMonster()
    {
        if (isLatern)
        {
            while (!isActive)
            {
                yield return null;
            }

            {
                spriteRenderer.enabled = true;
                StartCoroutine (DemonTime());
            }
        }
    }

    private void OffFlashlight()
    {
        if (isActive && isLatern)
        {
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
        Debug.Log("Killed");
    }

    public void ActivateDemon()
    {
        isActive = true;
    }
}
