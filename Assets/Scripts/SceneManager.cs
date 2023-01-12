using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneManager : MonoBehaviour
{
    private Button b_ARPlaneRecognition, b_ARQRCode, b_VRMobile, b_ARFace, backMapButton, b_Mod2;
    [SerializeField]
    private GameObject panelVideo, panelMod, panelMap;
    private Scene currentScene;
    
    private void Start()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        //if the device have GPS it loads the Mapbox map. Else it loads the basic AR with vuforia
        if (sceneName == "Main") 
        {
            panelMod = GameObject.Find("PanelModules");
            panelVideo = GameObject.Find("PanelVideo");
            panelMod.SetActive(true);
            b_Mod2 = GameObject.Find("Canvas/PanelModules/ButtonMod2").GetComponent<Button>();
            b_Mod2.onClick.AddListener(Mod2);
            panelMod.SetActive(false);
            panelVideo.SetActive(true);
        }

        else if (sceneName == "ZoomableMap") 
        {
            ZoomableMap();
        }
        else if (sceneName == "BasicAR")
        {
            b_ARPlaneRecognition = GameObject.Find("ButtonMap").GetComponent<Button>();
            b_ARPlaneRecognition.onClick.AddListener(ARScene);
        }
        else if (sceneName == "VRMobile")
        {
            b_VRMobile = GameObject.Find("ButtonBack").GetComponent<Button>();
            b_VRMobile.onClick.AddListener(VRScene);
        }
    }

    private void Update()
    {
#if !UNITY_EDITOR && !UNITY_IOS
  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string sceneName = currentScene.name;
            if (sceneName == "ZoomableMap" || sceneName == "Main")
            {
                MinimizeApp();
            }   

            else if (sceneName == "ARFace" || sceneName == "QRCodeScan" || sceneName == "ARSessionWithPlayerPrefsCEDRI" || sceneName == "ARSession_1" || sceneName == "ARSession_2")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("ZoomableMap");
            }
        }
#endif
    }

    private void MinimizeApp()
    {
#if UNITY_ANDROID
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call<bool>("moveTaskToBack", true);
#endif
    }

    //Unsupported GPS devices
    private void Mod2()
    {
        ARQRCodeScene();
    }

    private void ARScene()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
       
        UnityEngine.SceneManagement.SceneManager.LoadScene("ARSessionWithPlayerPrefsCEDRI");
    }

    private void ARQRCodeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("QRCodeScan");
    }

    private void ARScene2()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "ZoomableMap")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ARFace"); 
        }

        if (sceneName == "BasicAR")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ZoomableMap");
        }
    }

    private void VRScene()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if(sceneName == "ZoomableMap")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("VRMobile");
        }

        else if(sceneName == "VRMobile")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ZoomableMap");
        }
    }

    private void ZoomableMap()
    {
        panelMap = GameObject.Find("PanelMap");

        b_ARPlaneRecognition = GameObject.Find("ButtonAR").GetComponent<Button>();
        b_ARPlaneRecognition.onClick.AddListener(ARScene);

        b_ARQRCode = GameObject.Find("ButtonQRCode").GetComponent<Button>();
        b_ARQRCode.onClick.AddListener(ARQRCodeScene);

        b_VRMobile = GameObject.Find("ButtonVR").GetComponent<Button>();
        b_VRMobile.onClick.AddListener(VRScene);

        b_ARFace = GameObject.Find("ButtonARFace").GetComponent<Button>();
        b_ARFace.onClick.AddListener(ARScene2);

    }
}
