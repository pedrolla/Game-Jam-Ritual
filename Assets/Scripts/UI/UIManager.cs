using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject flashlight5;
    [SerializeField]
    private GameObject flashlight4;
    [SerializeField]
    private GameObject flashlight3;
    [SerializeField]
    private GameObject flashlight2;
    [SerializeField]
    private GameObject flashlight1;
    [SerializeField]
    private GameObject flashlight0;
    [SerializeField]
    private TextMeshProUGUI timeText;


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



    private void Start()
    {
        Flashlight5();
    }
    private void Flashlight5()
    {
        flashlight5.SetActive(true);
    }

    public void Flashlight4()
    {
        flashlight4.SetActive(true);
        flashlight5.SetActive(false);

    }

    public void Flashlight3()
    {
        flashlight3.SetActive(true);
        flashlight4.SetActive(false);
    }

    public void Flashlight2()
    {
        flashlight2.SetActive(true);
        flashlight3.SetActive(false);
    }

    public void Flashlight1()
    {
        flashlight1.SetActive(true);
        flashlight2.SetActive(false);
    }

    public void Flashlight0()
    {
        flashlight0.SetActive(true);
        flashlight1.SetActive(false);
    }

    public void UpdateTime(int hour)
    {
        timeText.text = (hour.ToString() + " A.M");
    }
}
