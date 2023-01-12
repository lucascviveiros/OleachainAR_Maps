using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Vuforia;
using TMPro;
using System;

public class ARListManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ARListObj;

    //[SerializeField]
    //private TextMeshProUGUI trackedText2;

    /*
    [SerializeField]
    private TextMeshProUGUI tID;

    [SerializeField]
    private TextMeshProUGUI tProducao;
    
    [SerializeField]
    private TextMeshProUGUI tLat;
    
    [SerializeField]
    private TextMeshProUGUI tLong;

    [SerializeField]
    private GameObject myArrow;

    [SerializeField]
    private DistancePoints distancePoints;
        private float spaceArrows = 6.0f;
 */

    private GameObject textMeshObj;


    // Lista com nomes dos Markers      
    private List<string> ARListNames = new List<string>();

    //Image Tracking Status
    //private ObserverBehaviour myCustomObserver;
    //protected new TargetStatus mPreviousTargetStatus = TargetStatus.NotObserved;

    void Awake()
    {
        ARListObj = GameObject.Find("ARList");
        textMeshObj = GameObject.Find("TextMarkerRecog");
        //trackedText2 = textMeshObj.gameObject.GetComponent<TextMeshProUGUI>();
        ARListNames = GetAllChilds(ARListObj);
    }
 
    public List<string> GetAllChilds(GameObject myARList)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < myARList.transform.childCount; i++)
        {
            list.Add(myARList.transform.GetChild(i).gameObject.name.ToString());
        }
        return list;
    }
    /*
    public void NoTracking()
    {
        tID.text = "";
        //tProducao.text = "";
        tLat.text = "";
        tLong.text = "";
    }*/

    public void CurrentTracking(string QRcode)
    {
        //trackedText2.text = QRcode;
        /*
        if(QRcode == "ARvore1")
        {
            tID.text = "ID: 001";
            //tProducao.text = "Produ��o/Ano: 180";
            tLat.text = "Lat: 41.797185";
            tLong.text = "Lon: -6.770038";
        }

        else if (QRcode == "ARvore2")
        {
            tID.text = "ID: 002";
            //tProducao.text = "Produ��o/Ano: 100";
            tLat.text = "Lat: 41.796782";
            tLong.text = "Lon: -6.769779";
        }
        else
        {
          
        }*/

    }

    private void RotaSeta(int quantArrows)
    {
        for (int i = 0; i < quantArrows; i++)
        {
            //Instantiate(myArrow, new Vector3(i * spaceArrows, 0, 0), Quaternion.identity);
        }
    }



}
