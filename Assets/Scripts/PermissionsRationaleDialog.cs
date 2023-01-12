using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class PermissionsRationaleDialog : MonoBehaviour
{
    const int kDialogWidth = 800;
    const int kDialogHeight = 400;
    private bool windowOpen = true;
    
    //GUI Style
    private GUIStyle guiStyle = new GUIStyle(); 

    void DoMyWindow(int windowID)
    {
        GUI.Label(new Rect(10, 20, kDialogWidth - 20, kDialogHeight - 50), "Habilite sua localização de GPS porfavor.", guiStyle);

        if (GUI.Button(new Rect(kDialogWidth - 110, kDialogHeight - 30, 300, 300), "OK"))
        {
#if PLATFORM_ANDROID
            Permission.RequestUserPermission(Permission.FineLocation);
#endif
            windowOpen = false;
        }
    }

    void OnGUI()
    {
        guiStyle.fontSize = 44;

        if (windowOpen)
        {
            Rect rect = new Rect((Screen.width/2) - (kDialogWidth/2), (Screen.height/2) - (kDialogHeight/2), kDialogWidth, kDialogHeight + 10);
            GUI.ModalWindow(0, rect, DoMyWindow, "");
        }
    }
}