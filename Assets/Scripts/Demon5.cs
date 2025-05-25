using System.Collections;
using UnityEngine;

public class Demon5 : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown = 2f;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private CameraController cameraContoller;
    private bool looked;
    private bool isActive;


    private void Start()
    {
        DemonManager.Instance.AddActiveDemons(gameObject);
    }

    public void StartDemon()
    {
        StartCoroutine(DemonAttack());
        cameraContoller.IsBackMonster();
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
        SoundManager.Instance.PlayBedJumpscare();
        JumpscareManager.Instance.PlayBedJumpScare();
    }

    public void OnFlashlightHit()
    {
        if (!isActive) return;

        looked = true;
        cameraContoller.IsNotBackMonster();
        DemonManager.Instance.ResetDemon5();
        isActive = false;
    }
}
