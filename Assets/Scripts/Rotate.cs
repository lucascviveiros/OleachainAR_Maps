using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 45f);
        //Vector3 myVector = new Vector3 (0.0f , transform.position.y  , 0.0f );
        //transform.Rotate(myVector, timer * 0.5f);
    }
}
