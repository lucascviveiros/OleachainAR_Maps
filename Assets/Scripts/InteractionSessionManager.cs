using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class InteractionSessionManager : MonoBehaviour
{
    private List<GameObject> myDetectedPlanes = new List<GameObject>();
    private List<GameObject> myDetectedPlanesV = new List<GameObject>();
    [SerializeField]
    private GameObject ARPlacementInteractable, ARSessionOrigin;

    public UnityEngine.UI.Button turnOffPlanesButton;

    public GameObject myCanvas;
    private ARPlaneMeshVisualizer ARPlaneMeshVisualizer;
    private ARPlaneManager ARPlaneManager;

    public bool TurnOffThePlanes = false;
    private bool once = true;

    private void Awake()
    {
        ARSessionOrigin = GameObject.FindGameObjectWithTag("ARSessionOrigem");
        ARPlaneManager = ARSessionOrigin.GetComponent<ARPlaneManager>();

        ARPlacementInteractable = GameObject.FindGameObjectWithTag("ARPlacementInteractable");
        //ARPlaneManager = ARSessionOrigin.GetComponent<ARPlaneManager>();

        //myCanvas = GameObject.Find("IntroARCanvas");
        //myClickCanvas = GameObject.Find("PlaceARCanvas");
        myCanvas.SetActive(true);
       // myClickCanvas.SetActive(false);
    }

    private void Start()
    {
        turnOffPlanesButton.onClick.AddListener(ExitCedriApp);
        StartCoroutine(StopPlanes());
    }

    private IEnumerator StopPlanes()
    {
        yield return new WaitForSecondsRealtime(9.0f);
        myCanvas.SetActive(false);
       // myClickCanvas.SetActive(true);
        StartCoroutine(HideARPlaceCanvas());
    }

    private IEnumerator HideARPlaceCanvas()
    {
        yield return new WaitForSecondsRealtime(2.0f);
     //   myClickCanvas.SetActive(false);
    }

    void Update()
    {
        GameObject found = GameObject.Find("ARPlane");
        GameObject found2 = GameObject.Find("ARPlaneVisualizer"); 
   
        if (found)
        {
            found.GetComponent<MeshRenderer>().enabled = false;
            myDetectedPlanes.Add(found);
            Debug.Log("plane added " + "q: " + myDetectedPlanes.Count);
        }

        else if (found2)
        {
            found2.GetComponent<MeshRenderer>().enabled = false;
            myDetectedPlanesV.Add(found2);
            Debug.Log("plane added " + "q: " + myDetectedPlanesV.Count);
        }

        if (TurnOffThePlanes && once)
        {
            once = false;
            TurnOffThePlanesMesh();
        }
    }

    private void TurnOffThePlanesMesh()
    {
        ARPlaneManager.enabled = false;
        GameObject[] arplaneVisualizer = GameObject.FindGameObjectsWithTag("ARPlaneV"); //looking for ARPlaneV tag added on planes

        foreach (GameObject go in arplaneVisualizer)
        {
            go.GetComponent<MeshRenderer>().enabled = false;
            go.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
            go.GetComponent<LineRenderer>().enabled = false;
            ARPlaneMeshVisualizer = go.GetComponent<ARPlaneMeshVisualizer>();
            ARPlaneMeshVisualizer.SendMessage("SetVisible", false);
            //go.GetComponent<ARPlaneMeshVisualizer>().BroadcastMessage("UpdateVisibility", false);
        }
    }

    public void TurnOffBool()
    {
        TurnOffThePlanes = true;
    }

    private void ExitCedriApp()
    {
        Application.Quit();
    }

    
}
