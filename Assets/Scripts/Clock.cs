using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public static ClockScript Instance;

    [SerializeField]
    private Sprite image1;
    [SerializeField] 
    private Sprite image2;
    [SerializeField]
    private Image clock;
    [SerializeField]
    private GameObject clockObject;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeClock()
    {
        StartCoroutine(ClockChange());  
    }

    private IEnumerator ClockChange()
    {
        clockObject.SetActive(true);
            clock.sprite = image1;
            yield return new WaitForSeconds(1);
            clock.sprite = image2;
        yield return new WaitForSeconds(1);
        clock.sprite = image1;
        SceneHolder.NextScene();
    }
}
