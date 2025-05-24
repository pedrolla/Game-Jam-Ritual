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
}
