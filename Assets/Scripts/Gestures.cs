using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gestures : MonoBehaviour
{
    private Vector2 fingerStart;
    private Vector2 fingerEnd;

    public int leftRight = 0;
    public int upDown = 0;

    public enum Movement
    {
        Left,
        Right,
        Up,
        Down
    };

    public List<Movement> movements = new List<Movement>();

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {

            if (touch.phase == TouchPhase.Began)
            {
                fingerStart = touch.position;
                fingerEnd = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                fingerEnd = touch.position;

                if ((fingerStart.x - fingerEnd.x) > 80 ||
                   (fingerStart.x - fingerEnd.x) < -80) // Side to side Swipe
                {
                    leftRight++;
                }
                else if ((fingerStart.y - fingerEnd.y) < -80 ||
                        (fingerStart.y - fingerEnd.y) > 80) // top to bottom swipe
                {
                    upDown++;

                }
                if (leftRight >= 3)
                {

                    leftRight = 0;
                }
                if (upDown >= 4)
                {

                    upDown = 0;
                }

                //After the checks are performed, set the fingerStart & fingerEnd to be the same
                fingerStart = touch.position;

            }
            if (touch.phase == TouchPhase.Ended)
            {
                leftRight = 0;
                upDown = 0;
                fingerStart = Vector2.zero;
                fingerEnd = Vector2.zero;
            }
        }

        //Example usage in Update. Note how I use Input.GetMouseButton instead of Input.touch

        //GetMouseButtonDown(0) instead of TouchPhase.Began
        if (Input.GetMouseButtonDown(0))
        {
            fingerStart = Input.mousePosition;
            fingerEnd = Input.mousePosition;
        }

        //GetMouseButton instead of TouchPhase.Moved
        //This returns true if the LMB is held down in standalone OR
        //there is a single finger touch on a mobile device
        if (Input.GetMouseButton(0))
        {
            fingerEnd = Input.mousePosition;

            //There was some movement! The tolerance variable is to detect some useful movement
            //i.e. an actual swipe rather than some jitter. This is the same as the value of 80
            //you used in your original code.
          //  if (Mathf.Abs(fingerEnd.x - fingerStart.x) > tolerance ||
          //     Mathf.Abs(fingerEnd.y - fingerStart.y) > tolerance)
            {

                //There is more movement on the X axis than the Y axis
                if (Mathf.Abs(fingerStart.x - fingerEnd.x) > Mathf.Abs(fingerStart.y - fingerEnd.y))
                {
                    //Right Swipe
                    if ((fingerEnd.x - fingerStart.x) > 0)
                        movements.Add(Movement.Right);
                    //Left Swipe
                    else
                        movements.Add(Movement.Left);
                }

                //More movement along the Y axis than the X axis
                else
                {
                    //Upward Swipe
                    if ((fingerEnd.y - fingerStart.y) > 0)
                        movements.Add(Movement.Up);
                    //Downward Swipe
                    else
                        movements.Add(Movement.Down);
                }

                //After the checks are performed, set the fingerStart & fingerEnd to be the same
                fingerStart = fingerEnd;

                //Now let's check if the Movement pattern is what we want
                //In this example, I'm checking whether the pattern is Left, then Right, then Left again
//                Debug.Log(CheckForPatternMove(0, 3, new List<Movement>() { Movement.Left, Movement.Right, Movement.Left }));
            }
        }

        //GetMouseButtonUp(0) instead of TouchPhase.Ended
        if (Input.GetMouseButtonUp(0))
        {
            fingerStart = Vector2.zero;
            fingerEnd = Vector2.zero;
            movements.Clear();
        }
    }
}