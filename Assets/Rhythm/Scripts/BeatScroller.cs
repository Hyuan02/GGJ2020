using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    private float velocity;

    public bool hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        velocity = beatTempo / 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            //if (Input.anyKeyDown) {
            //    hasStarted = true;
            //}
        }
        else
        {
            transform.position -= new Vector3(0, Time.deltaTime * velocity);
        }
    }
}
