using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopulateList : MonoBehaviour
{
    private GameObject myContentManager;
    [SerializeField]
    private GameObject myImagePrefab;

    private bool listFilled = false;

    void Start()
    {
        myContentManager = GameObject.FindGameObjectWithTag("myContentManager");
    }

    public void Populate(int listLength, double[] myActualDistanceList)
    {
       
        if (listFilled == true)
        {
            //Destroying the list of previous near trees distance
            foreach (Transform child in myContentManager.transform)
            {
                Destroy(child.gameObject);
            }

            listFilled = false;
        }

        if (listFilled == false) 
        {
            //Debug.Log("Populate List");
            int x = 0;
            //while (x <= listLength - 1)
            while (x <= 10)

            {
                var myNewImage = Instantiate(myImagePrefab);

                myNewImage.transform.SetSiblingIndex(x);

                myNewImage.transform.SetParent(myContentManager.transform);

                TextMeshProUGUI imageText = myNewImage.GetComponentInChildren<TextMeshProUGUI>();
                imageText.text = myActualDistanceList[x].ToString();
                x++;
            }

            listFilled = !listFilled;
        }

    }

}
