using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ChangeToTutorialScene()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ChangeToManualScene()
    {
        SceneManager.LoadScene("ManualScene");
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
