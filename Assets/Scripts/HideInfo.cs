using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Used to hide information in the main scene
public class HideInfo : MonoBehaviour
{
    private TestLocationService testLocation;
    private GameObject panelVideo, panelApp;
    private Button closeVideo;

    private void Awake()
    {
        panelVideo = GameObject.Find("PanelVideo");
        panelApp = GameObject.Find("PanelModules");
        closeVideo = GameObject.Find("Canvas/PanelVideo/ButtonCloseVideo").GetComponent<Button>();
        testLocation = GetComponent<TestLocationService>();
    }

    void Start()
    {
        closeVideo.GetComponent<Button>();
        closeVideo.onClick.AddListener(CloseVideo);
        StartCoroutine("WaitVideo");
    }

    IEnumerator WaitVideo()
    {
        yield return new WaitForSecondsRealtime(69.0f);
        panelVideo.SetActive(false);
        panelApp.SetActive(true);
        testLocation.SetGPSlistening(true); //Start verify GPS
    }

    private void CloseVideo()
    {
        panelVideo.SetActive(false);
        panelApp.SetActive(true);
        testLocation.SetGPSlistening(true);//Start verify GPS
    }
}
