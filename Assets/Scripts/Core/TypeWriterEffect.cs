using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    private float delayWriter = 0.01f;
    private string myText = "Wecome to CeDRI!\nCeDRI has an integrated and multifunctional R&I environment, comprising several laboratorial facilities: \n\nCovering different scientific areas. Combining laboratorial classes and development of final BsC and MSc projects. One lab fully dedicated for R&I activities.";

    void Start()
    {
        StartCoroutine("TypeWriter", myText);
    }

    private IEnumerator TypeWriter(string textType)
    {
        textUI.text = "";

        for (int letter = 0; letter < textType.Length; letter++ )
        {
            textUI.text = textUI.text + textType[letter];
            yield return new WaitForSeconds(delayWriter);
        }
    }
}
