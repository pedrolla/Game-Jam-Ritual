using System.Collections;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GoBack());
    }

    private IEnumerator GoBack()
    {
        yield return new WaitForSeconds(4);
        SceneHolder.SceneReset();
    }
}
