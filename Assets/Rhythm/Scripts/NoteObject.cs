﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{

    public bool canBePressed;

    public KeyCode keyPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyPressed))
        {
            if (canBePressed)
            {
                GameManager._instance.NoteHit();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Activator")
        {
            canBePressed = true;
            GameManager._instance.IncrementNotes();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Activator")
        {
            canBePressed = false;

            GameManager._instance.NoteMiss();
            Destroy(gameObject);
        }
    }
}
