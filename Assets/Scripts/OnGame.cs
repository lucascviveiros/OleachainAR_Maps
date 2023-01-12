using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class OnGame : MonoBehaviour
{
    public InteractionSessionManager interactionSession;
    public ARPlacementInteractable aRPlacement;
    public GameObject ARSession;
    public GameObject goARPlace;
    
    /// <summary>
    /// Auto-search for AR prefabs: when instantied in scene, it searchs for ARFoundation and XRInteraction (i.e, ARSession ARPlacementInteractable
    /// Once found the plane it sets the planes' mesh render to false
    /// </summary>
    void Start()
    {
        ARSession = GameObject.FindGameObjectWithTag("ARSession");
        goARPlace = GameObject.FindGameObjectWithTag("ARPlacementInteractable");

        //interactionSession = ARSession.gameObject.GetComponent<InteractionSessionManager>();
        //interactionSession.TurnOffBool();

        aRPlacement = goARPlace.GetComponent<ARPlacementInteractable>();
        aRPlacement.enabled = false;
    }   

}
