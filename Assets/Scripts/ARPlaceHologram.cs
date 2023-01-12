using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceHologram : MonoBehaviour
{
    // The prefab to instantiate on touch.
    [SerializeField]
    private GameObject _prefabToPlace;

    public Camera myCam;

    public InteractionSessionManager interactionSession;

    private bool firstRay = false;

    // Cache ARRaycastManager GameObject from ARCoreSession
    private ARRaycastManager _raycastManager;

    // List for raycast hits is re-used by raycast manager
    private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    private float waitTime = 6f;
    private float timer;

    void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        //Touch touch;
        //if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) { return; }

        // Perform AR raycast to any kind of trackable
        /*if (_raycastManager.Raycast(touch.position, Hits, TrackableType.Planes))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            var hitPose = Hits[0].pose;

            // Instantiate the prefab at the given position
            // Note: the object is not anchored yet!
            Instantiate(_prefabToPlace, hitPose.position, hitPose.rotation);

            // Debug output what we actually hit
            Debug.Log($"Instantiated on: {Hits[0].hitType}");
        }*/


        //Auto placement when recognize a plane
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            Ray ray = myCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("I'm looking at " + hit.transform.name);

                if (!firstRay)
                {
                    if (hit.transform.name.Contains("ARPlane"))
                    {
                        Instantiate(_prefabToPlace, hit.transform.position, transform.rotation);
                        _prefabToPlace.transform.LookAt(myCam.transform.position);
                        interactionSession.TurnOffBool();
                        //Debug.Log("Placed!");
                        firstRay = true;

                    }
                }
            }
        }

     

    }
}
