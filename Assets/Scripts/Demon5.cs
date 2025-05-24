using System.Collections;
using UnityEngine;

public class Demon5 : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown = 2f;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private bool looked;
    private bool isActive;
    private bool isCamera;

    private void Start()
    {
        HideMonster();
    }

    public void StartDemon()
    {
        SoundManager.Instance.PlayMonster5();
        StartCoroutine(DemonAttack());
        isActive = true;
    }

    private IEnumerator DemonAttack()
    {
        yield return new WaitForSeconds(attackCooldown);

        if (looked)
        {
            looked = false;
            yield break;
        }

        Kill();
    }

    private void Kill()
    {
        Debug.Log("Killed");
        SoundManager.Instance.PlayJumpScare();
    }

    public void HasCamera()
    {
        isCamera = true;
        StartCoroutine(ShowMonster());
    }

    private IEnumerator ShowMonster()
    {
        if (isCamera)
        {
            while (!isActive)
            {
                yield return null;
            }

            {
                spriteRenderer.enabled = true;
                looked = true;

                StartCoroutine(DeactivateMonster());
            }
        }
    }

    public void HideMonster()
    {
        isCamera = false;
        spriteRenderer.enabled = false;
    }

    private IEnumerator DeactivateMonster()
    {
        yield return new WaitForSeconds(1);
        HideMonster();
        looked = false;
        isActive = false;
    }
}
