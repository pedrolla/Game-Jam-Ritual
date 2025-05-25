using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHolder
{
    private static int nextScene = 1;

    public static void SceneReset()
    {
        nextScene = 1;
        SceneManager.LoadScene("Night1");
    }

    public static void SameScene()
    {
        if (nextScene == 1)
        {
            SceneManager.LoadScene("Night1");
        }

        if (nextScene == 2)
        {
            SceneManager.LoadScene("Night2");
        }

        if (nextScene == 3)
        {
            SceneManager.LoadScene("Night3");
        }

        if (nextScene == 4)
        {
            SceneManager.LoadScene("Night4");
        }

        if (nextScene == 5)
        {
            SceneManager.LoadScene("Night5");
        }
        if (nextScene == 6)
        {
            SceneManager.LoadScene("Win");
        }
    }

    public static void NextScene()
    {
        nextScene++;
        
        if (nextScene == 2)
        {
            SceneManager.LoadScene("Night2");
        }

        if (nextScene == 3)
        {
            SceneManager.LoadScene("Night3");
        }

        if (nextScene == 4)
        {
            SceneManager.LoadScene("Night4");
        }

        if (nextScene == 5)
        {
            SceneManager.LoadScene("Night5");
        }
         
        if (nextScene == 6)
        {
            //win
        }
    }
}
