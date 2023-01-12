using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Location;
using UnityEngine.SceneManagement;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class CheckGPS : MonoBehaviour
{
    GameObject dialog = null;

    [SerializeField]
    private Text textLat;

    [SerializeField]
    private Text textLon;

    [SerializeField]
    private Text textDebug;

    [SerializeField]
    private Button btnTestGPS;

    [SerializeField]
    private Button btnBack;

    void Start()
    {
        btnTestGPS.onClick.AddListener(OnTestGPSButtonClick);
        btnBack.onClick.AddListener(BackScene);

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }
#endif
    }

    private void BackScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("myZoomableMap");
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);

        OnTestGPSButtonClick();
    }

    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            textDebug.text = "Location Service Status Failed";
        }
        if (Input.location.isEnabledByUser)
        {
            textDebug.text = Input.location.isEnabledByUser.ToString();
            Debug.Log("isEnabledbyUser: " + Input.location.isEnabledByUser.ToString());
        }
        else
        {
            textDebug.text = Input.location.isEnabledByUser.ToString();
            Debug.Log("isEnabledbyUser: " + Input.location.isEnabledByUser.ToString());
        }
    }

    private void OnTestGPSButtonClick()
    {
        StartCoroutine(TestMyGPS());
    }

    private IEnumerator TestMyGPS()
    {
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            textDebug.text = "Unable to determine device location";
            textLat.text = "Unable to determine device location";
            textLon.text = "Unable to determine device location";

            yield break;
        }
        else
        {
            textLat.text = Input.location.lastData.latitude.ToString(); //Unity LocationService
            textLon.text = Input.location.lastData.longitude.ToString();//Unity LocationService

        }
    }

    void OnGUI()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else if (dialog != null)
        {
            Destroy(dialog);
        }
#endif
    }

}