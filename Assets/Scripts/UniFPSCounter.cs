using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniFPSCounter : MonoBehaviour
{
    private int screenLongSide;
    private Rect boxRect;
    private GUIStyle style = new GUIStyle();

    private int frameCount;
    private float elapsedTime;
    private double frameRate;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        UpdateUISize();
    }

    private void Update()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);
            frameCount = 0;
            elapsedTime = 0;

            if (screenLongSide != Mathf.Max(Screen.width, Screen.height))
            {
                UpdateUISize();
            }
        }
    }

    private void UpdateUISize()
    {
        screenLongSide = Mathf.Max(Screen.width, Screen.height);
        var rectLongSide = screenLongSide / 10;
        boxRect = new Rect(1, 1, rectLongSide, rectLongSide / 3);
        style.fontSize = (int)(screenLongSide / 36.8);
        style.normal.textColor = Color.white;
    }

    private void OnGUI()
    {
        GUI.Box(boxRect, "");
        GUI.Label(boxRect, " " + frameRate + "fps", style);
    }

}
