using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    [SerializeField]
    private float imageCD;
    [SerializeField]
    private float fireCD;
    [SerializeField]
    private float fireTime;
    [SerializeField]
    private float endCD;
    [SerializeField]
    private List<Sprite> cutsceneImages = new List<Sprite>();
    [SerializeField]
    private Image cutsceneImage;
    private int imageCount;

    private bool isFirstHalf = true;
    private bool isSecondHalf = true;
    private bool isFire = true;


    private void Start()
    {
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene()
    {
        for (imageCount = 0; imageCount <= 13; imageCount++)
        {
            cutsceneImage.sprite = cutsceneImages[imageCount];
            yield return new WaitForSeconds(imageCD);
        }

        yield return StartCoroutine(FireEffect());
        yield return StartCoroutine(EndCutscene());
    }

    private IEnumerator FireEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fireTime)
        {
            cutsceneImage.sprite = cutsceneImages[14];
            yield return new WaitForSeconds(fireCD);
            elapsedTime += fireCD;

            cutsceneImage.sprite = cutsceneImages[15];
            yield return new WaitForSeconds(fireCD);
            elapsedTime += fireCD;
        }
    }

    private IEnumerator EndCutscene()
    {
        for (imageCount = 16; imageCount <= 17; imageCount++)
        {
            cutsceneImage.sprite = cutsceneImages[imageCount];
            yield return new WaitForSeconds(imageCD);
        }

        yield return new WaitForSeconds(endCD);
        SceneManager.LoadScene("MainMenu");
    }
}