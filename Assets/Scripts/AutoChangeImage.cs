using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoChangeImage : MonoBehaviour
{
    public Image autoChangeImg;
    private int counter = 0;

    [SerializeField]
    private List<Sprite> listSprites;

    void Start()
    {
        ChangeImage();
    }

    private void ChangeImage()
    {
        StartCoroutine(AutoChangeTimer());
    }

    private IEnumerator AutoChangeTimer()
    {
        if (counter > 3)
            counter = 0;
        
        yield return new WaitForSecondsRealtime(3.0f);
        autoChangeImg.sprite = listSprites[counter];
        counter++;
        ChangeImage();
    }
}
