﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    internal static GameManager _instance;

    public AudioSource theMusic;

    public bool startPlaying;

    //public BeatScroller bs;

    public NotesSpawner ns;

    public int currentScore;
    
    public int scorePerNote = 100;

    public int notesMissed;




    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text multiplier;

    private void Awake()
    {
        GameManager._instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        startPlaying = true;
        ns.hasStarted = true;
        theMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void NoteMiss()
    {
        Debug.Log("Missed");
        notesMissed++;
        LostHP();
    }

    public void NoteHit()
    {
        Debug.Log("Hitted");
        currentScore += scorePerNote;
        scoreText.text = "Score: " + currentScore;
    }


    public void LostHP()
    {
        if(notesMissed > 6)
        {
            notesMissed = 0;
            SystemController._instance.PlayerHP -= 10;
            this.GetComponent<PlayerController>().UpdatePlayerStatus();


        }
    }
}
