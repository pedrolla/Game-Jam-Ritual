using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualScript : MonoBehaviour
{
    //on button press change int, int changes the image bc of list of images

    private Image imageOfManual;

    private int currentImage;

    [SerializeField]
    private List<Sprite> manualList = new List<Sprite>();

    private void Start()
    {
        imageOfManual = gameObject.GetComponent<Image>();
        currentImage = 0;
        imageOfManual.sprite = manualList[currentImage];
    }

    public void NextImage()
    {
        if (currentImage < manualList.Count - 1)
        {
            ++currentImage;
            UpdateImage();
        }
    }

    public void PreviousImage()
    {
        if (currentImage > 0)
        {
            --currentImage;
            UpdateImage();
        }
    }

    private void UpdateImage()
    {
        imageOfManual.sprite = manualList[currentImage];
    }
}
