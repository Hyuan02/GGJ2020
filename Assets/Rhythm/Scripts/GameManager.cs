using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    internal static GameManager _instance;

    public AudioSource theMusic;

    public bool startPlaying;

    public BeatScroller bs;

    public int currentScore;
    
    public int scorePerNote = 100;

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
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                bs.hasStarted = true;
                theMusic.Play();
            }
        }
    }


    public void NoteMiss()
    {
        Debug.Log("Missed");
    }

    public void NoteHit()
    {
        Debug.Log("Hitted");
        currentScore += scorePerNote;
        scoreText.text = "Score: " + currentScore;
    }
}
