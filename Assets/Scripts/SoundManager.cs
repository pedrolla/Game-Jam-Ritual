using UnityEngine;
using UnityEngine.PlayerLoop;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private AudioSource globalAudioSource;
    [SerializeField]
    private AudioSource middleAudioSource;
    [SerializeField]
    private AudioSource leftAudioSource;
    [SerializeField] 
    private AudioSource rightAudioSource;
    [SerializeField] 
    private AudioSource topAudioSource;
    [SerializeField] 
    private AudioSource bottomAudioSource;

    [SerializeField]
    private AudioClip monster5;
    [SerializeField] 
    private AudioClip monster2;
    [SerializeField] 
    private AudioClip monster3;
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
        bottomAudioSource.PlayOneShot(monster5); 
    }

    public void PlayMonster2(string position)
    {
        if (position == "front")
        {
            middleAudioSource.PlayOneShot(monster2);
            return;
        }

        if (position == "right")
        {
            rightAudioSource.PlayOneShot(monster2);
            return;
        }

        if (position == "left")
        {
            leftAudioSource.PlayOneShot(monster2);
            return;
        }
    }

    public void PlayMonster3(string position)
    {
        if (position == "front")
        {
            middleAudioSource.PlayOneShot(monster3);
            return;
        }

        if (position == "right")
        {
            rightAudioSource.PlayOneShot(monster3);
            return;
        }

        if (position == "left")
        {
            leftAudioSource.PlayOneShot(monster3);
            return;
        }
    }

    public void PlayFlashlight()
    {
        globalAudioSource.PlayOneShot(flashlight);
    }

    public void PlayLowBattery()
    {
        globalAudioSource.PlayOneShot(lowBattery);
    }

    public void PlayHandJumpscare()
    {
        globalAudioSource.PlayOneShot(handJumpscare);
    }

    public void PlayRoofJumpscare()
    {
        globalAudioSource.PlayOneShot(roofJumpscare);
    }

    public void PlayBedJumpscare()
    {
        globalAudioSource.PlayOneShot(bedJumpscare);
    }

    public void PlayCatJumpscare()
    {
        globalAudioSource.PlayOneShot(catJumpscare);
    }
}
