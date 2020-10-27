using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NotesSpawner : MonoBehaviour
{
    public string midiPath;
    private List<int> allNotes;
    private List<int> generalNotes;
    private List<long> times;

    public bool hasStarted = false;

    [SerializeField]
    private int minBlue;
    [SerializeField]
    private int maxBlue;
    [SerializeField]
    private int minRed;
    [SerializeField]
    private int maxRed;
    [SerializeField]
    private int minYellow;
    [SerializeField]
    private int maxYellow;
    [SerializeField]
    private int minGreen;
    [SerializeField]
    private int maxGreen;


    public GameObject[] notePrefabs;

    private void Awake()
    {

        Debug.Log(Application.dataPath);
        ParsingMidiTest.GetTimesAndNotes(Application.streamingAssetsPath + "/" + midiPath, out times, out allNotes, out generalNotes);

       

    }
    private void Start()
    {
       for(int i=0; i<allNotes.Count; i++)
        {
            Debug.Log("Seconds: " + times[i] / 1000000f);
            StartCoroutine(WaitForSpawn(allNotes[i], times[i] / 1000000f));
        }

        StartCoroutine(GameManager._instance.NotesChecker(allNotes.Count));
    }

    private void Update()
    {
        
    }

    private IEnumerator WaitForSpawn(int type, float seconds)
    {
        yield return new WaitUntil(() => hasStarted);
        yield return new WaitForSeconds(seconds);
        SpawnNote(type);
    }

    private void SpawnNote(int type)
    {
        if(type >= minBlue && type <= maxBlue)
        {
            GameObject g1 = Instantiate(notePrefabs[0], this.transform);

            BeatScroller bs = g1.AddComponent<BeatScroller>();
            bs.beatTempo = 125;
        }
        else if (type >= minRed && type <= maxRed)
        {
            GameObject g1 = Instantiate(notePrefabs[1], this.transform);
            g1.AddComponent<BeatScroller>().beatTempo = 125;

        }
        else if (type >= minYellow && type <= maxYellow)
        {
            GameObject g1 = Instantiate(notePrefabs[2], this.transform);
            g1.AddComponent<BeatScroller>().beatTempo = 125;

        }
        else if (type >= minGreen && type <= maxGreen)
        {
            GameObject g1 = Instantiate(notePrefabs[3], this.transform);
            g1.AddComponent<BeatScroller>().beatTempo = 125;

        }

    }
}
