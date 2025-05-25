using System.Collections;
using UnityEngine;

public class Night : MonoBehaviour
{
    [SerializeField]
    private GameObject nightObject;
    [SerializeField]
    private GameObject uIObject;


    private void Start()
    {
        uIObject.SetActive(false);
        StartCoroutine(DisplayImage());
    }

    private IEnumerator DisplayImage()
    {
        yield return new WaitForSeconds(2);
        nightObject.SetActive(false);
        uIObject.SetActive(true);
    }
}
