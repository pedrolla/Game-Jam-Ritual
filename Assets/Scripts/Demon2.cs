using System.Collections;
using UnityEngine;

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
    private int startTimer = 7;

    [SerializeField] private int maxTimer = 15;

    private int timer;

    private bool lightOn = false;
    private bool isActive;

    private void Awake()
    {
        spriteRenderer.enabled = false;
    }

    public void SpawnedDemon()
    {
        print("spawnou o 2");
        timer = startTimer;
        StartCoroutine(AddTime());
        StartCoroutine(TakeTime());
        isActive = true;
    }

    public void FlashlightOn()
    {
        if (isActive)
        {
            lightOn = true;
            spriteRenderer.enabled = true;
        }
    }


    public void Lightoff()
    {
        if(isActive && lightOn)
        {
            lightOn = false;
            spriteRenderer.enabled = false;
        }
    }

    private void Update()
    {

        if (timer == 0 && isActive)
        {
            DeactivateDemon();
        }
    }

    private void GrandmaDead()
    {
        print("you ded");
    }

    private void DeactivateDemon()
    {
        isActive = false;
        spriteRenderer.enabled = false;
        timer = startTimer;
        DemonManager.Instance.ReturnDemon(gameObject);
    }

    private IEnumerator AddTime()
    {
        while (timer < maxTimer && isActive)
        {
            yield return new WaitForSeconds(1);
            ++timer;
        }
        GrandmaDead();
        StopCoroutine(AddTime());
    }

    private IEnumerator TakeTime()
    {
        while (true)
        {
            if(timer > 0 && lightOn)
            {
                Debug.Log("hh");
                yield return new WaitForSeconds(1);
                timer -= 2;
            }
            else
            {
                yield return null; // prevent CPU freeze
            }
        }
    }
}
