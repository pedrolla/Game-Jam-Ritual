using System.Collections;
using UnityEngine;

public class Demon4 : MonoBehaviour
{
    //Vai cada vez saindo mais do buraco no teto (stages); works
    //quando chega à última stage ele ataca-te ao fim de X tempo ou se olhares para ele. works

    //make activate sprite e audio source
    //make update sprite depending in stage works
    //make timer de cada stage works
    //make on last stage either run out of time or if u point light at u die works

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite demonStage4;

    [SerializeField] private int stageInterval = 5;
    [SerializeField] private int deathcountdown = 5;
    private bool firstTime = true;

    [SerializeField]
    private int stageNumber;
    [SerializeField]
    private CameraController cameraController;

    private void Start()
    {
        DemonManager.Instance.AddActiveDemons(gameObject);

        stageNumber = 1;
        spriteRenderer.sprite = null;
        spriteRenderer.enabled = false;
        StartCoroutine(ChangeStageTimer());
    }

    public void LightOnDemon4()
    {
        spriteRenderer.enabled = true;
        if (stageNumber == 4)
        {
            //kill
            GrandmaDead();
        }

        else if (stageNumber == 1)
        {
            StopCoroutine(ChangeStageTimer());
            StartCoroutine(ChangeStageTimer());
        }

        else if (stageNumber > 1 && !firstTime)
        {
            firstTime = true;
            --stageNumber;
            ChangeStage();

            return;
        }
        firstTime = false;
    }

    public void LightOff()
    {
        spriteRenderer.enabled = false;
    }

    private void ChangeStage()
    {
        cameraController.ChangeUpStage(stageNumber);

        if (stageNumber == 4)
        {
            spriteRenderer.sprite = demonStage4;

            if (spriteRenderer.enabled is true)
            {
                //kill
                GrandmaDead();
            }
            else
            {
                //start death countdown courotine
                StartCoroutine(DeathCountdown());
            }
        }
        else spriteRenderer.sprite = null;
    }

    private void GrandmaDead()
    {
        JumpscareManager.Instance.PlayRoofJumpScare();
    }

    private IEnumerator ChangeStageTimer()
    {
        while (stageNumber < 4)
        {
            yield return new WaitForSeconds(stageInterval);
            ++stageNumber;

            ChangeStage();
            if (stageNumber == 4) yield return null;
        }
    }

    private IEnumerator DeathCountdown()
    {
        yield return new WaitForSeconds(deathcountdown);
        GrandmaDead();
    }
}

