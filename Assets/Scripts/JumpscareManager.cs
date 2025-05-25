using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareManager : MonoBehaviour
{
    public static JumpscareManager Instance;

    [SerializeField]
    private GameObject catJumpscare;
    [SerializeField] 
    private GameObject roofJumpscare;
    [SerializeField] 
    private GameObject handJumpscare;
    [SerializeField] 
    private GameObject bedJumpscare;
    [SerializeField]
    private Transform jumpscareCanvas;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private float jumpscareCD;


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

    public void PlayCatJumpScare()
    {
        SoundManager.Instance.PlayCatJumpscare();
        Instantiate(catJumpscare, jumpscareCanvas);
        DemonManager.Instance.DeleteDemons();
        cameraController.enabled = false;
        StartCoroutine(MainMenu());
    }

    public void PlayRoofJumpScare()
    {
        SoundManager.Instance.PlayRoofJumpscare();
        Instantiate(roofJumpscare, jumpscareCanvas);
        DemonManager.Instance.DeleteDemons();
        cameraController.enabled = false;
        StartCoroutine(MainMenu());
    }

    public void PlayHandJumpScare()
    {
        SoundManager.Instance.PlayHandJumpscare();
        Instantiate(handJumpscare, jumpscareCanvas);
        DemonManager.Instance.DeleteDemons();
        cameraController.enabled = false;
        StartCoroutine(MainMenu());
    }

    public void PlayBedJumpScare()
    {
        SoundManager.Instance.PlayBedJumpscare();
        Instantiate(bedJumpscare, jumpscareCanvas);
        DemonManager.Instance.DeleteDemons();
        cameraController.enabled = false;
        StartCoroutine(MainMenu());
    }

    private IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(jumpscareCD);
        SceneManager.LoadScene("MainMenu");
    }
}
