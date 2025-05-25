using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private AudioSource myAudioSource;
    [SerializeField]
    private AudioClip monster5;
    [SerializeField]
    private AudioClip jumpScare;
    [SerializeField]
    private AudioClip flashlight;
    [SerializeField]
    private AudioClip lowBattery;
    [SerializeField]
    private AudioClip catJumpscare;
    [SerializeField]
    private AudioClip handJumpscare;
    [SerializeField]
    private AudioClip roofJumpscare;
    [SerializeField]
    private AudioClip bedJumpscare;


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

    public void PlayMonster5()
    {
        myAudioSource.PlayOneShot(monster5); 
    }

    public void PlayJumpScare()
    {
        myAudioSource.PlayOneShot(jumpScare);
    }

    public void PlayFlashlight()
    {
        myAudioSource.PlayOneShot(flashlight);
    }

    public void PlayLowBattery()
    {
        myAudioSource.PlayOneShot(lowBattery);
    }

    public void PlayHandJumpscare()
    {
        myAudioSource.PlayOneShot(handJumpscare);
    }

    public void PlayRoofJumpscare()
    {
        myAudioSource.PlayOneShot(roofJumpscare);
    }

    public void PlayBedJumpscare()
    {
        myAudioSource.PlayOneShot(bedJumpscare);
    }

    public void PlayCatJumpscare()
    {
        myAudioSource.PlayOneShot(catJumpscare);
    }
}
