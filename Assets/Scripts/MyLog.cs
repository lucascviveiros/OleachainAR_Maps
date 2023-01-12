using UnityEngine;
using System.Collections;
public class MyLog : MonoBehaviour
{
    string myLog;
    Queue myLogQueue = new Queue();
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    private void Start()
    {
        Debug.Log("Log initiating (...) ");
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = " \n " + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    void OnGUI()
    {
        guiStyle.normal.textColor = Color.red;
        guiStyle.fontSize = 36; //change the font size
        GUILayout.Label(myLog, guiStyle);
           
    }
}