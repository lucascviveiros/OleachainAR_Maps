using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
//    public delegate void OnButtonClick();
//    public static event OnButtonClick onButtonClick;


    public delegate void OnUserAllowGPS();
    public static event OnUserAllowGPS onUserAllowGPS;

}