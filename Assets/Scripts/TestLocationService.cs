using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

/// TestLocationService class is used to verify Geoposition Systems on mobile cell phones
/// it verifies if the device has embedded GPS or if the user has allowed being used by the application
public class TestLocationService : MonoBehaviour
{
    public Text debug;

    [SerializeField]
    private TextMeshProUGUI t_Mod1;
    [SerializeField]
    private TextMeshProUGUI t_Mod1sub;

    [SerializeField]
    private TextMeshProUGUI t_Mod2;
    [SerializeField]
    private TextMeshProUGUI t_Mod2sub;

    [SerializeField]
    private Button b_Mod1;

    [SerializeField]
    private Button b_Mod2;

    private Color activeColor, unableColor, blackColor;

    private bool startListening;

    //Used by HideInfo class after finishing video introduction
    public void SetGPSlistening(bool value)
    {
        startListening = value;
    }

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#323232", out blackColor);
        ColorUtility.TryParseHtmlString("#C39B26", out activeColor);
        ColorUtility.TryParseHtmlString("#C39B26", out unableColor);

        unableColor.a = 0.43f;
        activeColor.a = 0.43f;

        b_Mod1 = GameObject.Find("ButtonMod1").GetComponent<Button>();
        b_Mod2 = GameObject.Find("ButtonMod2").GetComponent<Button>();
        b_Mod1.GetComponent<Image>().color = activeColor;
        b_Mod2.GetComponent<Image>().color = activeColor;

        t_Mod1 = GameObject.Find("Canvas/PanelModules/Text_Mod1").GetComponent<TextMeshProUGUI>();
        t_Mod1sub = GameObject.Find("Canvas/PanelModules/Text_Subtitle1").GetComponent<TextMeshProUGUI>();
        t_Mod2 = GameObject.Find("Canvas/PanelModules/Text_Mod2").GetComponent<TextMeshProUGUI>();
        t_Mod2sub = GameObject.Find("Canvas/PanelModules/Text_Subtitle2").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        OnEnableGPSbyUser();
    }

    private void Update()
    {
        if (startListening)
        {
#if UNITY_EDITOR
            StartCoroutine(LoadSceneAR());
#endif
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                StartCoroutine(LoadSceneAR());
            }

            if (Input.location.isEnabledByUser)
            {
                debug.text = "GPS location recognized";
                StartCoroutine(LoadSceneGPS());
            }
            else
            {
                debug.text = "Enable your GPS location";
            }
        }
    }

    //If the device has GPS it loads Mapbox satellite map on the ZoomableMap scene
    private IEnumerator LoadSceneGPS()
    {
        EnableMod1();
        yield return new WaitForSecondsRealtime(4.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ZoomableMap");
    }

    //If the device does not have GPS it loads the QRCodeScan scene
    private IEnumerator LoadSceneAR()
    {
        EnableMod2();
        yield return new WaitForSecondsRealtime(4.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("QRCodeScan");
    }

    //Check GPS permission 
    private void OnEnableGPSbyUser()
    {
        //GPS Permission Dialog Request 
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
            debug.text = "Android Permission";
        }
        //If the user already gave permission, check if the GPS service is enabled in the cell phone functions
        if (!UnityEngine.Input.location.isEnabledByUser)
        {
            //debug.text = "Android and Location not enabled";
            debug.text = "Enable your GPS location";
            debug.color = blackColor;
            DisableMod();
        }
        else
        {
            debug.text = "GPS location Recognized";
            EnableMod1();
        }
    }
    
    private void DisableMod()
    {
        unableColor.a = 0.43f;
        t_Mod1.color = unableColor;
        t_Mod1sub.color = unableColor;
        t_Mod2.color = unableColor;
        t_Mod2sub.color = unableColor;
    }

    private void EnableMod1()
    {
        activeColor.a = 1f;
        t_Mod1.color = activeColor;
        t_Mod1sub.color = activeColor;
        b_Mod1.GetComponent<Image>().color = activeColor;
    }

    private void EnableMod2()
    {
        activeColor.a = 1f;
        t_Mod2.color = activeColor;
        t_Mod2sub.color = activeColor;
        b_Mod2.GetComponent<Image>().color = activeColor;
    }

    IEnumerator LocationCoroutine()
    {

        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation)) {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
        }

        // First, check if user has location service enabled
        if (!UnityEngine.Input.location.isEnabledByUser) {
            Debug.LogFormat("Android and Location not enabled");
            debug.text = "Android and Location not enabled";
            yield break;
        }

        
#if UNITY_IOS
        if (!UnityEngine.Input.location.isEnabledByUser) {
            // TODO Failure
            Debug.LogFormat("IOS and Location not enabled");
            yield break;
        }
#endif
        
        // Start service before querying location
        UnityEngine.Input.location.Start(500f, 500f);

        // Wait until service initializes
        int maxWait = 15;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            debug.text = "LocationService Initializing";

            maxWait--;
        }

        // Editor has a bug which doesn't set the service status to Initializing. So extra wait in Editor.
#if UNITY_EDITOR
        int editorMaxWait = 15;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Stopped && editorMaxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            debug.text = "LocationService Stopped";

            editorMaxWait--;
        }
#endif

        // Service didn't initialize in 15 seconds
        if (maxWait < 1)
        {
            // TODO Failure
            Debug.LogFormat("Timed out");
            debug.text = "Time out";

            yield break;
        }

        // Connection has failed
        if (UnityEngine.Input.location.status != LocationServiceStatus.Running)
        {
            // TODO Failure
            Debug.LogFormat("Unable to determine device location. Failed with status {0}", UnityEngine.Input.location.status);
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.LogFormat("Location: "
                + UnityEngine.Input.location.lastData.latitude + " "
                + UnityEngine.Input.location.lastData.longitude + " "
                + UnityEngine.Input.location.lastData.altitude + " "
                + UnityEngine.Input.location.lastData.horizontalAccuracy + " "
                + UnityEngine.Input.location.lastData.timestamp);

            var _latitude = UnityEngine.Input.location.lastData.latitude;
            var _longitude = UnityEngine.Input.location.lastData.longitude;
            // TODO success do something with location
        }

        // Stop service if there is no need to query location updates continuously
        UnityEngine.Input.location.Stop();
    }

}

