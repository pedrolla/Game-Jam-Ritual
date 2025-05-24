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

    [SerializeField] private Sprite demonStage1;
    [SerializeField] private Sprite demonStage2;
    [SerializeField] private Sprite demonStage3;
    [SerializeField] private Sprite demonStage4;

    [SerializeField] private int stageInterval = 5;
    [SerializeField] private int deathcountdown = 5;

    private int stageNumber;

    private void Start()
    {
        stageNumber = 1;
        spriteRenderer.enabled = false;
        spriteRenderer.sprite = demonStage1; //only when light on
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
        else if (stageNumber > 1)
        {
            --stageNumber;
            ChangeStage();
        }
        else if (stageNumber == 1)
        {
            StopCoroutine(ChangeStageTimer());
            StartCoroutine(ChangeStageTimer());
        }
    }

    public void LightOff()
    {
        spriteRenderer.enabled = false;
    }

    private void ChangeStage()
    {
        if (stageNumber == 1)
        {
            spriteRenderer.sprite = demonStage1;
        }

        if (stageNumber == 2)
        {
            spriteRenderer.sprite = demonStage2;


        }
        else if (stageNumber == 3)
        {
            spriteRenderer.sprite = demonStage3;


        }
        else if (stageNumber == 4)
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
    }

    private void GrandmaDead()
    {
        print("you ded");
    }

    private IEnumerator ChangeStageTimer()
    {
        while (stageNumber < 4)
        {
            yield return new WaitForSeconds(stageInterval);
            ++stageNumber;
            ChangeStage();
        }
    }

    private IEnumerator DeathCountdown()
    {
        yield return new WaitForSeconds(deathcountdown);
        GrandmaDead();
    }
}

