using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeToGameScene()
    {
        SceneHolder.SameScene();
    }

    public void ChangeToTutorialScene()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ChangeToManualScene()
    {
        SceneManager.LoadScene("ManualScene");
        SceneHolder.SceneReset();
    }

    public void ChangeToCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
