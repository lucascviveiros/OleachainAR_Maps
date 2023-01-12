using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Touchscreen interaction with the olive tree points shown in the Mapbox satellite map
public class ShowPointInfo : MonoBehaviour
{
    [SerializeField]
    private Camera myCamera;
    private Vector3 touchPosWorld, clickPosWorld;
    private bool isHit;
    private GameObject touchedObject;

    [SerializeField]
    private TextMeshProUGUI myTouchedInfo;

    [SerializeField]
    private RectTransform rectPanelNotification;

    [SerializeField]
    private Button closeInfo;

    private LabelTextSetter labelTextSetter;

    private bool openPanel;

    private Vector2 touchPosWorld2D;

    private RaycastHit2D hitInformation;

    private void Start()
    {
        labelTextSetter = GetComponent<LabelTextSetter>();
        closeInfo.onClick.AddListener(ClosePanelInfo);
    }

    void Update()
    {
        /*
        if (Input.touchCount > 0)
        {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            touchedObject = hitInformation.transform.gameObject;
            labelTextSetter = touchedObject.GetComponentInParent<LabelTextSetter>();

            if (touchedObject.name.ToString() != "UserSurroundings")
            {
                UpdateInfo(labelTextSetter);
            }
        }*/

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Ended || Input.GetTouch(i).phase == TouchPhase.Stationary)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = myCamera.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    touchedObject = hit.transform.gameObject;
                    labelTextSetter = touchedObject.GetComponentInParent<LabelTextSetter>();

                    if (touchedObject.name.ToString() != "UserSurroundings")
                    {
                        UpdateInfo(labelTextSetter);
                    }
                }
            }
        }      
        
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) )
        {
            //Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //isHit = false;
                //Debug.Log("Clicked: " + hit.transform.name);

                touchedObject = hit.transform.gameObject;
                labelTextSetter = touchedObject.GetComponentInParent<LabelTextSetter>();

                if (touchedObject.name.ToString() != "UserSurroundings")
                {
                    UpdateInfo(labelTextSetter);
                }
            }
        }    
#endif
    }

    //It updates the information in the Canvas according to the tree clicked in the touchscreen
    private void UpdateInfo(LabelTextSetter labelText)
    {
        string ID = labelTextSetter.GetId();
        string date = labelTextSetter.GetDate();
        double Latitude = labelTextSetter.GetLatitude();
        double Longitude = labelText.GetLongitude();
        myTouchedInfo.text = "Oliveira ID: " + ID + "\n" + "Date: " + date + "\n" + "Latitude: " + Latitude.ToString() + "\n" + "Longitude: " + Longitude;
        StartCoroutine(ShowPanelInfo());
    }

    private IEnumerator ShowPanelInfo()
    {
        openPanel = true;
        LeanTween.move(rectPanelNotification, new Vector3(0.5f, 0.0f, 0f), 1f).setCanvasMove();
        yield return new WaitForSecondsRealtime(1f);
        openPanel = false;
    }

    private void ClosePanelInfo()
    {
        if(openPanel != true)
        {
            LeanTween.move(rectPanelNotification, new Vector3(-440.0f, 0.0f, 0f), 1f).setCanvasMove();
        }
    }
}
