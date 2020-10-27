using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum PlayerProgress
{
    robot = 1, ciborg, human
}


public class SystemController : MonoBehaviour
{
    public static SystemController _instance;  

    internal PlayerProgress PlayerState = PlayerProgress.robot;

    [SerializeField]
    internal int PlayerHP = 100;

    internal int PhasesComplete; 
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(_instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        PlayerState = PlayerProgress.robot;
        PlayerHP = 100;
        PhasesComplete = 0;
        LoadProgressScene(PhasesComplete);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void LoadProgressScene(int phaseNumber)
    {
        switch (phaseNumber)
        {
            case 0:
                SceneManager.LoadScene("FirstPhase");
            break;
        }  
    }

}
