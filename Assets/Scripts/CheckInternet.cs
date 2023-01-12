using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CheckInternet : MonoBehaviour
{
    private Text t_internetStatus;
    private RectTransform rectPanelNotification;
    private float f_timerCounter, f_waitTime = 9f;
    private bool b_Connected = false, b_NotConnected = false, b_InternetStatus = true;
    private LeanTweenValues leanTweenValue = new LeanTweenValues();

    private void Awake() 
    {
        rectPanelNotification = GameObject.Find("PanelNotificationConnection").GetComponent<RectTransform>();    
        t_internetStatus = GameObject.Find("TextInternetStatus").GetComponent<Text>();
    }

    private void Start()
    {
        StartCoroutine(waitNotification());
    }

    private void LaterUpdate()
    {
        f_timerCounter += Time.deltaTime;
        if (f_timerCounter > f_waitTime)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                b_Connected = false;
                t_internetStatus.text = "No Internet";
                t_internetStatus.color = Color.red;
                if (b_NotConnected == false)
                {
                    b_NotConnected = true;
                    b_InternetStatus = true;
                }
            }
            else
            {
                b_NotConnected = false;
                t_internetStatus.text = "Internet Connection Successful";
                t_internetStatus.color = Color.white;
                if (b_Connected == false)
                {
                    b_Connected = true;
                    b_InternetStatus = true;
                }
            }

            f_timerCounter = 0f;
        }

        //Internet Connection Not Found
        if (b_NotConnected == true && b_InternetStatus == true) 
        {
            //LeanTween.move(rectPanelNotification, new Vector3(0.0f, leanTweenValue.GetValueShowNotification(), 0f), 1f).setCanvasMove().setDelay(0.2f);
            LeanTweenNotification();
            StartCoroutine(waitNotification());
        }

        else if (b_Connected == true && b_InternetStatus == true)
        {
            //LeanTween.move(rectPanelNotification, new Vector3(0.0f, leanTweenValue.GetValueShowNotification(), 0f), 1f).setCanvasMove().setDelay(0.2f);
            LeanTweenNotification();
            StartCoroutine(waitNotification());
        }

        if (b_NotConnected == true && b_Connected == true || b_NotConnected == false && b_Connected == false)
        {
            b_InternetStatus = false;
        }
    } 

    IEnumerator waitNotification()
    {
        b_InternetStatus = false;
        yield return new WaitForSecondsRealtime(6.0f);
        LeanTweenNotification();
        //LeanTween.move(rectPanelNotification, new Vector3(0.0f, leanTweenValue.GetValueHideNotification(), 0f), 1f).setCanvasMove().setDelay(0.2f); //Hide notification
    }

    /*

    IEnumerator CheckInternetConnection()
    {
        UnityWebRequest request = new UnityWebRequest("http://www.google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            t_internetStatus.text = "No Internet connection";
            t_internetStatus.color = Color.red;
        }

        else
        {
            t_internetStatus.text = "Internet connection successful";
            t_internetStatus.color = Color.green;
        }
    } */

    private void LeanTweenNotification()
    {
        LeanTween.move(rectPanelNotification, new Vector3(0.0f, leanTweenValue.GetValueHideNotification(), 0f), 1f).setCanvasMove().setDelay(0.2f); //Hide notification
    }
}
